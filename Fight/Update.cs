using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;

namespace Fight
{
    public class Update:INotifyPropertyChanged
    //后台更新前台信息类
    {
        public Update()
        {
            myKinect = new MyKinect(this);
            Gesture = "我是输入的手势动作";
        }
        //通过名字来更改前台值 数据绑定
        private void OnPropertyChanged(string aPropertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName)); }
        public event PropertyChangedEventHandler PropertyChanged;


        #region data
        //图像绑定元素
        public BitmapSource SkeletonImage { get { return _SkeletonImage; } set { if (_SkeletonImage == value) return; _SkeletonImage = value; OnPropertyChanged(nameof(SkeletonImage)); } }
        private BitmapSource _SkeletonImage;
        public String Gesture { get { return _Gesture; } set { if (_Gesture == value) return; _Gesture = value; OnPropertyChanged(nameof(Gesture)); } }
        private String _Gesture;

        //其他变量
        public MyKinect myKinect;
        #endregion
    }
}
