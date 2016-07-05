using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Fight
{
    public static class WinIO
    {
        // WinIO64
        //****************************

        public const int KBC_KEY_CMD = 0x64;//输入键盘按下消息的端口
        
        public const int KBC_KEY_DATA = 0x60;//输入键盘弹起消息的端口

        [DllImport("WinIo64.dll")]
        public static extern bool InitializeWinIo();

        [DllImport("WinIo64.dll")]
        public static extern bool GetPortVal(IntPtr wPortAddr, out int pdwPortVal,

                    byte bSize);

        [DllImport("WinIo64.dll")]
        public static extern bool SetPortVal(uint wPortAddr, IntPtr dwPortVal,

                    byte bSize);

        [DllImport("WinIo64.dll")]
        public static extern byte MapPhysToLin(byte pbPhysAddr, uint dwPhysSize,

                        IntPtr PhysicalMemoryHandle);

        [DllImport("WinIo64.dll")]
        public static extern bool UnmapPhysicalMemory(IntPtr PhysicalMemoryHandle,

                        byte pbLinAddr);

        [DllImport("WinIo64.dll")]
        public static extern bool GetPhysLong(IntPtr pbPhysAddr, byte pdwPhysVal);

        [DllImport("WinIo64.dll")]
        public static extern bool SetPhysLong(IntPtr pbPhysAddr, byte dwPhysVal);

        [DllImport("WinIo64.dll")]
        public static extern void ShutdownWinIo();

        [DllImport("user32.dll")]
        public static extern int MapVirtualKey(uint Ucode, uint uMapType);


        //初始化，安装驱动？

        public static void Initialize()
        {
            if (InitializeWinIo())
            {
                KBCWait4IBE();
            }
            else
            {
                Console.Write("WinIO初始化失败！");
            }
        }

        //应该是调用结束要用的，卸载驱动？

        public static void Shutdown()
        {
            ShutdownWinIo();
            KBCWait4IBE();
        }


        ///等待键盘缓冲区为空

        public static void KBCWait4IBE()
        {
            int dwVal = 0;
            do
            {
                bool flag = GetPortVal((IntPtr)0x64, out dwVal, 1);
            }
            //while ((dwVal &amp;amp; 0x2) &amp;gt; 0);
            while ((dwVal & 0x2) > 0);
        }

        /// 模拟键盘标按下

        public static void KeyDown(Keys vKeyCoad)
        {
            int btScancode = 0;
            btScancode = MapVirtualKey((uint)vKeyCoad, 0);
            KBCWait4IBE();
            SetPortVal(KBC_KEY_CMD, (IntPtr)0xD2, 1);
            KBCWait4IBE();
            SetPortVal(KBC_KEY_DATA, (IntPtr)0x60, 1);
            KBCWait4IBE();
            SetPortVal(KBC_KEY_CMD, (IntPtr)0xD2, 1);
            KBCWait4IBE();
            SetPortVal(KBC_KEY_DATA, (IntPtr)btScancode, 1);
        }

        /// 模拟键盘弹出

        public static void KeyUp(Keys vKeyCoad)
        {
            int btScancode = 0;
            btScancode = MapVirtualKey((uint)vKeyCoad, 0);
            KBCWait4IBE();
            SetPortVal(KBC_KEY_CMD, (IntPtr)0xD2, 1);
            KBCWait4IBE();
            SetPortVal(KBC_KEY_DATA, (IntPtr)0x60, 1);
            KBCWait4IBE();
            SetPortVal(KBC_KEY_CMD, (IntPtr)0xD2, 1);
            KBCWait4IBE();
            SetPortVal(KBC_KEY_DATA, (IntPtr)(btScancode | 0x80), 1);
        }

        /// 模拟一次按键

        public static void KeyDownUp(Keys vKeyCoad)
        {
            //Thread.Sleep(1);
            //for (int i = 0; i < 30; i++)
            //{
                KeyDown(vKeyCoad); // 按下A
                Thread.Sleep(200);
                KeyUp(vKeyCoad); // 松开A
            //}
            
            
        }
    }
}