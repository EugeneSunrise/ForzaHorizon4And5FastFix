using GlobalHotKeys;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TestVeryHotKeys
{
    public partial class SilentForm : Form
    {
        #region RegUnregHotKeys
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        #endregion

        // Hide a window from the Alt-Tab program switcher
        protected override CreateParams CreateParams
        {
            get
            {
                var Params = base.CreateParams;
                Params.ExStyle |= 0x80;

                return Params;
            }
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public SilentForm()
        {
            InitializeComponent();

            var HotKeyManager = new HotkeyManager();
            // RegisterHotKey (Hangle, Hotkey Identifier, Modifiers, Key)
            RegisterHotKey(HotKeyManager.Handle, 666, Constants.NOMOD, (int)Keys.CapsLock); // Kill explorer
            RegisterHotKey(HotKeyManager.Handle, 777, Constants.ALT, (int)Keys.Home); // Restart explorer

            // Silent start cmd and run forza
            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = "cmd";
            myProcess.StartInfo.Arguments = "/K explorer.exe shell:appsFolder\\Microsoft.SunriseBaseGame_8wekyb3d8bbwe!SunriseReleaseFinal & exit"; // check virtual windows folder and find forza
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.Start();

            // Some shit code
            bool forzaProcess = false;
            while (forzaProcess == false)
            {
                foreach (Process proc in Process.GetProcesses())
                    if (proc.ProcessName == "ForzaHorizon4")
                    {
                        forzaProcess = true;
                        proc.EnableRaisingEvents = true;
                        proc.Exited +=
                            (s, e) =>
                            {
                                // Exit if Forza was closed
                                Environment.Exit(0);
                            };
                        break;
                    }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Visible = false;
            this.WindowState = FormWindowState.Minimized;
        }

        #region ToolStripMenu
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private void hotKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Kill Explorer: CapsLock\nRestart Explorer: ALT + Home", "Hot Keys");
        }
        #endregion

    }
}
