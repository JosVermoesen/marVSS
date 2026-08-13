using System.Collections;
using System.Windows.Forms;

namespace marVSS2028
{
    /// <summary>
    /// Comparer for sorting ListView items by a specified column and sort order.
    /// </summary>
    public class ListViewItemComparer : IComparer
    {
        private readonly int _column;
        private readonly SortOrder _order;

        public ListViewItemComparer(int column, SortOrder order)
        {
            _column = column;
            _order = order;
        }

        public int Compare(object x, object y)
        {
            var itemX = (ListViewItem)x;
            var itemY = (ListViewItem)y;

            string textX = _column < itemX.SubItems.Count ? itemX.SubItems[_column].Text : string.Empty;
            string textY = _column < itemY.SubItems.Count ? itemY.SubItems[_column].Text : string.Empty;

            int result = string.Compare(textX, textY, System.StringComparison.OrdinalIgnoreCase);

            return _order == SortOrder.Descending ? -result : result;
        }
    }
}
