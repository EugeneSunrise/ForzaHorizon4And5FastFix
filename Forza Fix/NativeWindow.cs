using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GlobalHotKeys
{
    #region Constants
    public static class Constants
    {
        public const int NOMOD = 0x0000;
        public const int ALT = 0x0001;
        public const int CTRL = 0x0002;
        public const int SHIFT = 0x0004;
        public const int WIN = 0x0008;

        public const int WM_HOTKEY_MSG_ID = 0x0312;
    }
    #endregion

    #region HotKeyManager
    public sealed class HotkeyManager : NativeWindow, IDisposable
    {
        public HotkeyManager()
        {
            CreateHandle(new CreateParams());
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
            {
                if (m.WParam.ToInt32() == 666)
                {
                    // Kill explorer
                    Process myProcess = new Process();
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = "cmd";
                    myProcess.StartInfo.Arguments = "/K taskkill /f /im explorer.exe & exit";
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                }

                if (m.WParam.ToInt32() == 777)
                {
                    // Restart explorer
                    Process.Start(Environment.SystemDirectory + "\\..\\explorer.exe");
                }
            }
            base.WndProc(ref m);
        }

        public void Dispose()
        {
            DestroyHandle();
        }
    }
    #endregion
}