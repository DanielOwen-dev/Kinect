using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fight.Response
{
    public static class Keyboard
    {
        const int KEY_DOWN = 0;
        const int KEY_UP = 2;

        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern void keybd_event(
            byte bVk, //虚拟键值
            byte bScan,// 一般为0
            int dwFlags, //这里是整数类型 0 为按下，2为释放
            int dwExtraInfo //这里是整数类型 一般情况下设成为 0
            );
    }
}
