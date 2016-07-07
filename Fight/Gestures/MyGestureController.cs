using LightBuzz.Vitruvius.Gestures;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Represents a gesture controller.
    /// </summary>
    public class MyGestureController
    {
        #region Members

        /// <summary>
        /// A list of all the gestures the controller is searching for.
        /// </summary>
        private List<MyGesture> _gestures = new List<MyGesture>();
        int Player;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="GestureController"/>.
        /// </summary>
        public MyGestureController(string file)
        {
            LoadGesture(file);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GestureController"/>.
        /// </summary>
        /// <param name="type">The gesture type to recognize. Set to GesureType.All for instantly adding all of the predefined gestures.</param>
        #endregion

        #region Events

        /// <summary>
        /// Occurs when a gesture is recognized.
        /// </summary>
        public event EventHandler<MyGestureEventArgs> GestureRecognized;

        #endregion

        #region Methods

        /// <summary>
        /// Updates all gestures.
        /// </summary>
        /// <param name="skeleton">The skeleton data.</param>
        public void Update(Skeleton skeleton,int player)
        {
            Player = player;
            foreach (MyGesture gesture in _gestures)
            {
                gesture.Update(skeleton);
            }
        }

        public void InitializeGesture()
        {
            _gestures.Clear();
        }
        /// <summary>
        /// Adds the specified gesture for recognition.
        /// </summary>
        /// <param name="type">The predefined <see cref="GestureType" />.</param>
        /// 

        
        public void LoadGesture(string file)
        {
            System.Xml.Linq.XDocument xdoc = System.Xml.Linq.XDocument.Load("Gesture.xml");
            System.Xml.Linq.XElement xeRoot = xdoc.Root;

            foreach (var e in xeRoot.Elements())
            {
                string name = e.Attribute("Name").Value;
                int ID = Convert.ToInt32(e.Attribute("ID").Value);

                int NumberOfSegments = Convert.ToInt32(e.Attribute("NumberOfSegments").Value);
                MySegment[] mySegment = new MySegment[NumberOfSegments];
                int NumSeg = 0;//当前段数

                foreach( var t in e.Elements() )
                {
                    int num = Convert.ToInt32(t.Attribute("Number").Value);
                    int NumberOfConditions = Convert.ToInt32(t.Attribute("NumberOfConditions").Value);
                    MySegment_condition[] myCondition = new MySegment_condition[NumberOfConditions];
                    int NumCon = 0;//当前条件
                    foreach (var x in t.Elements() )
                    {
                        int Type = Convert.ToInt32(x.Attribute("Type").Value);
                        int Joint0 = Convert.ToInt32(x.Attribute("Joint0").Value);
                        int Joint1 = Convert.ToInt32(x.Attribute("Joint1").Value);
                        float Number = (float)Convert.ToDouble(x.Attribute("Number").Value);
                        myCondition[NumCon] = new MySegment_condition(Type, (JointType)Joint0, (JointType)Joint1, Number);
                        NumCon++;
                    }
                    for (int i = 0; i < num; i++)
                    {
                        mySegment[NumSeg] = new MySegment(myCondition);
                        NumSeg++;
                    }
                }
                AddGesture(name, mySegment, ID);
            }
        }

        /// <summary>
        /// Adds the specified gesture for recognition.
        /// </summary>
        /// <param name="name">The gesture name.</param>
        /// <param name="segments">The gesture segments.</param>
        public void AddGesture(string name, IGestureSegment[] segments,int id)
        {
            MyGesture gesture = new MyGesture(name,id,segments);
            gesture.GestureRecognized += OnGestureRecognized;
            _gestures.Add(gesture);
        }
        #endregion

        #region Event handlers

        /// <summary>
        /// Handles the GestureRecognized event of the g control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KinectSkeltonTracker.GestureEventArgs"/> instance containing the event data.</param>
        /// 将消息从Gesture_Contronller传到Gesture
        private void OnGestureRecognized(object sender, MyGestureEventArgs e)
        {
            e.Player = Player;//玩家编号

            GestureRecognized?.Invoke(this, e);

            foreach (MyGesture gesture in _gestures)
            {
                gesture.Reset();
            }
        }

        #endregion
    }
}
