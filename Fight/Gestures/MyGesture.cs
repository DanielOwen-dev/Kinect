using LightBuzz.Vitruvius.Gestures;
using Microsoft.Kinect;
using System;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Represents a Kinect <see cref="Gesture"/>.
    /// </summary>
    class MyGesture
    {
        #region Constants

        /// <summary>
        /// The window size.
        /// </summary>
        readonly int WINDOW_SIZE = 50;

        /// <summary>
        /// The maximum number of frames allowed for a paused gesture.
        /// </summary>
        readonly int MAX_PAUSE_COUNT = 10;

        #endregion

        #region Members

        /// <summary>
        /// The segments which form the current gesture.
        /// </summary>
        IGestureSegment[] _segments;

        /// <summary>
        /// The current gesture segment we are matching against.
        /// </summary>
        int _currentSegment = 0;

        /// <summary>
        /// The number of frames to pause for when a pause is initiated.
        /// </summary>
        int _pausedFrameCount = 10;

        /// <summary>
        /// The current frame.
        /// </summary>
        int _frameCount = 0;

        /// <summary>
        /// Are we paused?
        /// </summary>
        bool _paused = false;

        /// <summary>
        /// The name of the current gesture.
        /// </summary>
        string _name;

        //动作标识
        int _ID;
        #endregion

        #region Events

        /// <summary>
        /// Occurs when a gesture is recognised.
        /// </summary>
        public event EventHandler<MyGestureEventArgs> GestureRecognized;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of <see cref="Gesture"/>.
        /// </summary>
        /// <param name="name">The name of gesture.</param>
        /// <param name="segments">The segments of the gesture.</param>
        public MyGesture(string name, int ID ,IGestureSegment[] segments)
        {
            _name = name;
            _segments = segments;
            _ID = ID;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="skeleton">The skeleton data.</param>
        public void Update(Skeleton skeleton)
        {
            if (_paused)//为真 加快帧计数
            {
                if (_frameCount == _pausedFrameCount)
                {
                    _paused = false;
                }

                _frameCount++;
            }

            GesturePartResult result = _segments[_currentSegment].Update(skeleton);

            if (result == GesturePartResult.Succeeded)
            {
                if (_currentSegment + 1 < _segments.Length)
                {
                    _currentSegment++;
                    _frameCount = 0;
                    _pausedFrameCount = MAX_PAUSE_COUNT;
                    _paused = true;
                }
                else
                {
                    if (GestureRecognized != null)//识别成功 发送消息
                    {
                        GestureRecognized(this, new MyGestureEventArgs(_name, skeleton.TrackingId, _ID));
                        Reset();
                    }
                }
            }
            else if (result == GesturePartResult.Failed || _frameCount == WINDOW_SIZE)//识别失败
            {
                Reset();
            }
            else
            {
                _frameCount++;
                _pausedFrameCount = MAX_PAUSE_COUNT / 2;
                _paused = true;
            }
        }

        /// <summary>
        /// Resets the current gesture.
        /// </summary>
        public void Reset()
        {
            _currentSegment = 0;
            _frameCount = 0;
            _pausedFrameCount = MAX_PAUSE_COUNT / 2;
            _paused = true;
        }

        #endregion
    }
}
