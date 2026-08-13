using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace YourSha256Com
{
    [ComVisible(true)]
    [Guid("D5A27EE5-23E0-4A6E-9AF6-1F2D965E9B8F")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ISha256Com
    {
        string ComputeSha256(string filePath);
    }

    [ComVisible(true)]
    [Guid("A3F15E3C-2EB6-42B5-A6F0-4F3A9B3A0A8C")]
    [ProgId("YourSha256Com.Class")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(ISha256Com))]
    public class Sha256Com : ISha256Com
    {
        public string ComputeSha256(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("filePath is required.", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found.", filePath);

            using (var stream = File.OpenRead(filePath))
            using (var sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(stream);
                var sb = new StringBuilder(hashBytes.Length * 2);
                foreach (byte b in hashBytes)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }
    }
}
