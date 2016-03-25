using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using QTP.Infra;

namespace QTP.Main
{
    public partial class ExeRunUC : UserControl
    {
        private Process process;

        public ExeRunUC()
        {
            InitializeComponent();
        }

        public void Close()
        {
            try
            {
                if (process != null)
                    process.Kill();
            }
            catch
            { }
        }

        public void Open(string exePath, string name, string args)
        {
            // Wait for process to be created and enter idle condition 
            ProcessStartInfo info = new ProcessStartInfo(string.Format("{0}\\{1}", exePath, name), args);
            info.WorkingDirectory = exePath;
            info.WindowStyle = ProcessWindowStyle.Minimized;

            process = Process.Start(info);
            
            process.WaitForInputIdle();

            // Change process to son of this
            Thread.Sleep(300);
            WinAPI.SetParent(process.MainWindowHandle, this.Handle);

            Int32 wndStyle = WinAPI.GetWindowLong(process.MainWindowHandle, WinAPI.GWL_STYLE);
            wndStyle &= ~WinAPI.WS_BORDER;
            wndStyle &= ~WinAPI.WS_THICKFRAME;
            WinAPI.SetWindowLong(process.MainWindowHandle, WinAPI.GWL_STYLE, wndStyle);

        }

        protected override void OnResize(EventArgs e)
        {
            if (process != null && process.MainWindowHandle != IntPtr.Zero)
                WinAPI.MoveWindow(process.MainWindowHandle, 0, 0, this.Width, this.Height, true);
            base.OnResize(e);
        }

    }
}
