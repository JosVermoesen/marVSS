using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace marVSS2028.Classes
{
    internal static class ShellHelper // This class provides helper methods for executing shell commands and handling related diagnostics, including logging and error handling for ShellExecute operations.
    {
        private const int SW_SHOWNORMAL = 1;
        private const uint SEE_MASK_NOCLOSEPROCESS = 0x40;

        public static string ShellHelperLogPath;

        [DllImport("shell32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            int nShowCmd);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            public string lpVerb;
            public string lpFile;
            public string lpParameters;
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

        private static string GetDefaultLogPath()
        {
            if (!string.IsNullOrWhiteSpace(ShellHelperLogPath))
            {
                return ShellHelperLogPath;
            }

            try
            {
                var basePath = Globals.LOCATION_MYDOCUMENTS;
                if (string.IsNullOrWhiteSpace(basePath))
                {
                    basePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                }

                if (string.IsNullOrWhiteSpace(basePath))
                {
                    basePath = Application.StartupPath;
                }

                return Path.Combine(basePath, "ShellHelper.log");
            }
            catch
            {
                return "C:\\ShellHelper.log";
            }
        }

        private static void SHLog(string s)
        {
            try
            {
                var sPath = GetDefaultLogPath();
                var dir = Path.GetDirectoryName(sPath);
                if (!string.IsNullOrWhiteSpace(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.AppendAllText(sPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {s}{System.Environment.NewLine}");
            }
            catch
            {
                // ignore logging failures (VB6: On Error Resume Next)
            }
        }

        /// <summary>
        /// Simple Shell helper using Process.Start.
        /// </summary>
        public static bool ShellExecuteWithFallback(string target)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = target,
                    UseShellExecute = true
                });
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ShellExecuteWithFallback error: " + ex.Message);
                return false;
            }
        }

        public static bool ShellExecuteWithFallbackOld(string sTarget, string sParams = "", string sVerb = "open")
        {
            var op = sVerb ?? "open";
            var file = sTarget ?? string.Empty;
            var @params = sParams ?? string.Empty;

            SHLog($"Attempting ShellExecute. CurDir={System.Environment.CurrentDirectory} COMSPEC={System.Environment.GetEnvironmentVariable("COMSPEC")} PATH={System.Environment.GetEnvironmentVariable("PATH")}");
            SHLog($"ShellExecute parameters: verb={op} file={file} params={@params}");

            long ret;
            try
            {
                ret = ShellExecute(IntPtr.Zero, op, file, @params, null, SW_SHOWNORMAL).ToInt64();
            }
            catch
            {
                ret = 0;
            }

            SHLog($"ShellExecute returned: {ret} ({ShellExecuteErrorText(ret)})");

            if (ret > 32)
            {
                return true;
            }

            if (TryShellExecuteEx(op, file, @params))
            {
                return true;
            }

            if (TryCmdStart(file, @params))
            {
                return true;
            }

            return false;
        }

        private static bool TryShellExecuteEx(string sVerb, string sFile, string sParams)
        {
            var sei = new SHELLEXECUTEINFO
            {
                cbSize = Marshal.SizeOf(typeof(SHELLEXECUTEINFO)),
                fMask = SEE_MASK_NOCLOSEPROCESS,
                hwnd = IntPtr.Zero,
                lpVerb = sVerb,
                lpFile = sFile,
                lpParameters = sParams,
                lpDirectory = null,
                nShow = SW_SHOWNORMAL
            };

            bool res;
            try
            {
                res = ShellExecuteEx(ref sei);
            }
            catch
            {
                res = false;
            }

            SHLog($"ShellExecuteEx returned: {(res ? 1 : 0)} (hProcess={sei.hProcess})");
            return res;
        }

        private static bool TryCmdStart(string sFile, string sParams)
        {
            var comspec = System.Environment.GetEnvironmentVariable("COMSPEC");
            if (string.IsNullOrWhiteSpace(comspec))
            {
                SHLog("COMSPEC empty. Cannot use cmd fallback.");
                return false;
            }

            var args = $"/C start \"\" {Quote(sFile)}";
            if (!string.IsNullOrWhiteSpace(sParams))
            {
                args += " " + sParams;
            }

            SHLog($"Trying COMSPEC fallback. Command: \"{comspec}\" {args}");

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = comspec,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                var p = Process.Start(psi);
                SHLog($"Shell(cmd) returned pid: {(p != null ? p.Id.ToString() : "<null>")}");
                return p != null;
            }
            catch (Exception ex)
            {
                SHLog($"Shell(cmd) error: {ex.GetType().Name} - {ex.Message}");
                return false;
            }
        }

        public static string ShellExecuteErrorText(long code)
        {
            switch (code)
            {
                case 0: return "SE_ERR_Fail";
                case 2: return "SE_ERR_FileNotFound";
                case 3: return "SE_ERR_PathNotFound";
                case 5: return "SE_ERR_OOM or ACCESS_DENIED";
                case 8: return "SE_ERR_OOM";
                case 26: return "SE_ERR_DLLNOTFOUND";
                case 27: return "SE_ERR_NOASSOC or ASSOC_INCOMPLETE";
                case 28: return "SE_ERR_DDETIMEOUT";
                case 29: return "SE_ERR_DDEFAIL";
                case 30: return "SE_ERR_DDEBUSY";
                case 31: return "SE_ERR_NOUI";
                case 32: return "SE_ERR_DLLNOTFOUND";
                default:
                    if (code < 0) return "Negative return (unknown)";
                    if (code > 32) return "Success";
                    return "Unknown code";
            }
        }

        public static void LogDiagnostics(string sExtension = ".pdf")
        {
            try
            {
                SHLog("---- Diagnostics start ----");
                SHLog($"CurDir: {System.Environment.CurrentDirectory}");
                SHLog($"App.Path: {Application.StartupPath}");
                SHLog($"User: {System.Environment.GetEnvironmentVariable("USERNAME")} COMPUTERNAME: {System.Environment.GetEnvironmentVariable("COMPUTERNAME")}");
                SHLog($"COMSPEC: {System.Environment.GetEnvironmentVariable("COMSPEC")}");
                SHLog($"PATH: {System.Environment.GetEnvironmentVariable("PATH")}");
                SHLog($"PATHEXT: {System.Environment.GetEnvironmentVariable("PATHEXT")}");

                var comspec = System.Environment.GetEnvironmentVariable("COMSPEC");
                if (!string.IsNullOrWhiteSpace(comspec))
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = comspec,
                        Arguments = $"/C assoc {sExtension}",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };

                    var p = Process.Start(psi);
                    SHLog($"Launched assoc probe for {sExtension} (pid={(p != null ? p.Id.ToString() : "<null>")})");
                }
                else
                {
                    SHLog("COMSPEC not present, assoc probe skipped");
                }

                SHLog("---- Diagnostics end ----");
            }
            catch
            {
                // ignore
            }
        }

        private static string Quote(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "\"\"";
            }

            if (value.IndexOf(' ') >= 0 || value.IndexOf('\t') >= 0 || value.IndexOf('"') >= 0)
            {
                return "\"" + value.Replace("\"", "\\\"") + "\"";
            }

            return value;
        }
    }
}
