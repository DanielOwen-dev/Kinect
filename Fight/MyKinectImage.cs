using Microsoft.Kinect;
using System;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using LightBuzz.Vitruvius;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Threading;

namespace Fight
{
    public class MyKinectImage
    {
        public MyKinectImage(Update _update)
        {
            update = _update;  //获取窗口数据
        }

        #region Data
            //用于处理三个数据
                private Image<Bgr, Byte> skeletonImage;

            //关联外部的数据
                private KinectSensor sensor;
                private Update update;
            //图像数据缓冲存储空间
                DepthImageFormat depthImageFormat;      //深度流图像格式
                private Skeleton[] skeletonData;
                private int depthWidth, depthHeight;
            //Gesture手势的控件
            MyGestureController _myGestureController;
        #endregion

        #region Method

        //配置Kinect
        public void Config(KinectSensor _sensor)
        {
            sensor = _sensor;

            //Kinect图像流设置           
            depthImageFormat = DepthImageFormat.Resolution640x480Fps30;
            this.sensor.DepthStream.Enable(depthImageFormat);
            sensor.SkeletonStream.Enable();

            //图像的宽度和高度
            depthWidth = sensor.DepthStream.FrameWidth;
            depthHeight = sensor.DepthStream.FrameHeight;


            //图像原始数据
            this.skeletonData = new Skeleton[sensor.SkeletonStream.FrameSkeletonArrayLength];
            skeletonImage = new Image<Bgr, byte>(depthWidth, depthHeight);

            //为数据流添加响应函数
            sensor.SkeletonFrameReady += SensorSkeletonFrameReady;

            _myGestureController = new MyGestureController();
            _myGestureController.LoadGesture();
            _myGestureController.GestureRecognized += GestureController_GestureRecognized;
        }



        //骨骼
        public void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {


            Boolean flag = false;
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletonFrame.CopySkeletonDataTo(skeletonData);
                    flag = true;
                }
            }

            try
            {
                foreach (Skeleton skeleton in skeletonData)
                {
                    if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        _myGestureController.Update(skeleton);
                    }
                }
            }
            catch { }
            

            if (flag)
            { 
                skeletonImage.Draw(new Rectangle(0, 0, skeletonImage.Width, skeletonImage.Height), new Bgr(0.0, 0.0, 0.0), -1);
                DrawSkeletons(skeletonImage, 0);
                //update.change(skeletonImage.Bitmap);
                update.SkeletonImage=ToBitmapSource(skeletonImage);
            }
        }












        //标记所有正确Tracked的关节点
        private void DrawSkeletons(Image<Bgr, Byte> img, int depthOrColor)
        {
            foreach (Skeleton skeleton in this.skeletonData)
            {
                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {
                    DrawTrackedSkeletonJoints(img, skeleton.Joints, depthOrColor);
                }
            }
        }

        //连接关节点
        private void DrawTrackedSkeletonJoints(Image<Bgr, Byte> img, JointCollection jointCollection, int depthOrColor)
        {
            // Render Head and Shoulders
            DrawBone(img, jointCollection[JointType.Head], jointCollection[JointType.ShoulderCenter], depthOrColor);
            DrawBone(img, jointCollection[JointType.ShoulderCenter], jointCollection[JointType.ShoulderLeft], depthOrColor);
            DrawBone(img, jointCollection[JointType.ShoulderCenter], jointCollection[JointType.ShoulderRight], depthOrColor);

            // Render Left Arm
            DrawBone(img, jointCollection[JointType.ShoulderLeft], jointCollection[JointType.ElbowLeft], depthOrColor);
            DrawBone(img, jointCollection[JointType.ElbowLeft], jointCollection[JointType.WristLeft], depthOrColor);
            DrawBone(img, jointCollection[JointType.WristLeft], jointCollection[JointType.HandLeft], depthOrColor);

            // Render Right Arm
            DrawBone(img, jointCollection[JointType.ShoulderRight], jointCollection[JointType.ElbowRight], depthOrColor);
            DrawBone(img, jointCollection[JointType.ElbowRight], jointCollection[JointType.WristRight], depthOrColor);
            DrawBone(img, jointCollection[JointType.WristRight], jointCollection[JointType.HandRight], depthOrColor);

            // Render other bones...
            DrawBone(img, jointCollection[JointType.ShoulderCenter], jointCollection[JointType.Spine], depthOrColor);

            DrawBone(img, jointCollection[JointType.Spine], jointCollection[JointType.HipRight], depthOrColor);
            DrawBone(img, jointCollection[JointType.KneeRight], jointCollection[JointType.HipRight], depthOrColor);
            DrawBone(img, jointCollection[JointType.KneeRight], jointCollection[JointType.AnkleRight], depthOrColor);
            DrawBone(img, jointCollection[JointType.FootRight], jointCollection[JointType.AnkleRight], depthOrColor);

            DrawBone(img, jointCollection[JointType.Spine], jointCollection[JointType.HipLeft], depthOrColor);
            DrawBone(img, jointCollection[JointType.KneeLeft], jointCollection[JointType.HipLeft], depthOrColor);
            DrawBone(img, jointCollection[JointType.KneeLeft], jointCollection[JointType.AnkleLeft], depthOrColor);
            DrawBone(img, jointCollection[JointType.FootLeft], jointCollection[JointType.AnkleLeft], depthOrColor);
        }

        //绘制骨骼情况区分
        private void DrawBone(Image<Bgr, Byte> img, Joint jointFrom, Joint jointTo, int depthOrColor)
        {
            //无法识别的
            if (jointFrom.TrackingState == JointTrackingState.NotTracked ||
            jointTo.TrackingState == JointTrackingState.NotTracked)
            {
                return; // nothing to draw, one of the joints is not tracked
            }

            //可推测的
            if (jointFrom.TrackingState == JointTrackingState.Inferred ||
            jointTo.TrackingState == JointTrackingState.Inferred)
            {
                DrawBoneLine(img, jointFrom.Position, jointTo.Position, 1, depthOrColor);
            }

            //被识别的
            if (jointFrom.TrackingState == JointTrackingState.Tracked &&
            jointTo.TrackingState == JointTrackingState.Tracked)
            {
                DrawBoneLine(img, jointFrom.Position, jointTo.Position, 3, depthOrColor);
            }
        }

        //绘制骨骼线的具体实现
        private void DrawBoneLine(Image<Bgr, Byte> img, SkeletonPoint p1, SkeletonPoint p2, int lineWidth, int depthOrColor)
        {
            System.Drawing.Point p_1, p_2;
            
            p_1 = SkeletonPointToDepthScreen(p1);
            p_2 = SkeletonPointToDepthScreen(p2);

            img.Draw(new LineSegment2D(p_1, p_2), new Bgr(255, 255, 0), lineWidth);
            img.Draw(new CircleF(p_1, 5), new Bgr(0, 0, 255), -1);
        }

        //骨骼图到深度图
        private System.Drawing.Point SkeletonPointToDepthScreen(SkeletonPoint skelpoint)
        {
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, depthImageFormat);
            return new System.Drawing.Point(depthPoint.X, depthPoint.Y);
        }




        [System.Runtime.InteropServices.DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap();

                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ptr);
                return bs;
            }
        }


        


        //手势识别与键盘、鼠标关联
        void GestureController_GestureRecognized(object sender, MyGestureEventArgs e)
        {
            
        }
        #endregion
    }
}
