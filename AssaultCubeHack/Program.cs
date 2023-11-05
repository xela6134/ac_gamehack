using System.Diagnostics;
using System.Security.Principal;

namespace AssaultCubeHack
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            if (IsAdministrator() == false)
            {
                try
                {
                    ProcessStartInfo procInfo = new ProcessStartInfo();
                    procInfo.UseShellExecute = true;
                    procInfo.FileName = Application.ExecutablePath;
                    procInfo.WorkingDirectory = Environment.CurrentDirectory;
                    procInfo.Verb = "runas";
                    Process.Start(procInfo);
                }
                catch (Exception ex)
                {
                    // If user does not want to run with admin privileges
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            else // Program was originally being ran as Administrator
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
            }            
        }

        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            if (identity != null)
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            return false;
        }
    }
}