using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fight.Response
{
    public static class Mouse
    {
        //鼠标键盘事件
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        //移动鼠标 
        const int MOUSEEVENT_MOVE = 0x0001;
        //模拟鼠标左键按下 
        const int MOUSEEVENT_LEFTDOWN = 0x0002;
        //模拟鼠标左键抬起 
        const int MOUSEEVENT_LEFTUP = 0x0004;
        //模拟鼠标右键按下 
        const int MOUSEEVENT_RIGHTDOWN = 0x0008;
        //模拟鼠标右键抬起 
        const int MOUSEEVENT_RIGHTUP = 0x0010;
        //模拟鼠标中键按下 
        const int MOUSEEVENT_MIDDLEDOWN = 0x0020;
        //模拟鼠标中键抬起 
        const int MOUSEEVENT_MIDDLEUP = 0x0040;
        //标示是否采用绝对坐标 
        const int MOUSEEVENT_ABSOLUTE = 0x8000;
    }
}
