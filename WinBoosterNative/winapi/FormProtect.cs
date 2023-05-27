using System.Runtime.InteropServices;

namespace WinBoosterNative.winapi
{
    public static class FormProtect
    {
        [DllImport("user32.dll")]
        public static extern uint SetWindowDisplayAffinity(IntPtr hwnd, uint dwAffinity);
    }
}
