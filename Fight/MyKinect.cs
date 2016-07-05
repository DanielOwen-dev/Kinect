using Microsoft.Kinect;
using System;
using System.Windows;

namespace Fight
{
    public class MyKinect
    {
        public MyKinect(Update _update)
        {
            update = _update;
            image = new MyKinectImage(update); //关联图像处理类

            //连接设备
            if ( !ConnectKinect() ){   MessageBox.Show("Kinect设备未找到"); return; }
            //配置图像类
            image.Config(sensor);
            
            //启动设备
            Start();
        }

        #region Data 
        //Kinect设备
        private Update update;
        public KinectSensor sensor;
        private MyKinectImage image;
        #endregion

        #region Method
        //连接Kinect设备
        private Boolean ConnectKinect()
        {
            //枚举Kinect设备，并将第一个连接成功的设备做为当前设备
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    sensor = potentialSensor;
                    return true;
                }
            }
            return false;
        }

        //启动设备
        private void Start()
        {
            //启动设备
            try
            {
                this.sensor.Start();
            }
            catch (System.IO.IOException)
            {
                this.sensor = null;
            }
        }

        //断开连接 程序关闭前调用
        public void Close(object sender, EventArgs e)
        {
            //停止设备
            if (null != sensor)
            {
                sensor.Stop();                 
            }
        }
              
        #endregion
    }
}
