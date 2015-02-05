
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JLQ.Common
{
    class Test
    {
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        public const int WH_CBT = 5;
        public const int HCBT_ACTIVATE = 5;
        IntPtr hookHandle = IntPtr.Zero;

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int hookid, HookProc pfnhook, IntPtr hinst, int threadid);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wparam, IntPtr lparam);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string modName);

        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hhook);

        public delegate bool WNDENUMPROC(IntPtr hwnd, int lParam);
        [DllImport("user32.dll")]
        public static extern int EnumChildWindows(IntPtr hWndParent, WNDENUMPROC lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern void SetWindowText(IntPtr hwnd, string lpString);

        public bool EnumChild(IntPtr hwnd, int lParam)
        {
            StringBuilder vBuffer = new StringBuilder(256);
            GetClassName(hwnd, vBuffer, vBuffer.Capacity);
            if (vBuffer.ToString().ToLower() == "button") // 按钮
            {
                StringBuilder vText = new StringBuilder(256);
                GetWindowText(hwnd, vText, vText.Capacity);
                if (vText.ToString().ToLower().IndexOf("&a") >= 0) // 终止
                    SetWindowText(hwnd, "停不要动");
                if (vText.ToString().ToLower().IndexOf("&r") >= 0) // 重试
                    SetWindowText(hwnd, "再来一次");
                if (vText.ToString().ToLower().IndexOf("&i") >= 0) // 忽略
                    SetWindowText(hwnd, "就这样吧");
            }
            return true;
        }

        private IntPtr CBTHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            switch (nCode)
            {
                case HCBT_ACTIVATE:
                    EnumChildWindows(wParam, new WNDENUMPROC(EnumChild), 0);
                    UnhookWindowsHookEx(hookHandle);
                    break;
            }
            return CallNextHookEx(hookHandle, nCode, wParam, lParam);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            hookHandle = SetWindowsHookEx(WH_CBT, new HookProc(CBTHookCallback), GetModuleHandle(null), 0);
            MessageBox.Show("Zswang 路过", "提示", MessageBoxButtons.AbortRetryIgnore);
        }
    }
}
