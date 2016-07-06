using System;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// The gesture event arguments.
    /// </summary>
    public class MyGestureEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the name of the gesture.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the skeleton tracking ID for the gesture.
        /// </summary>
        public int TrackingID { get; private set; }

        public int ID { get; private set; }

        public int Player {get;set;}
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="GestureEventArgs"/>.
        /// </summary>
        public MyGestureEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GestureEventArgs"/>.
        /// </summary>
        /// <param name="type">The gesture type.</param>
        /// <param name="trackingID">The tracking ID.</param>
        public MyGestureEventArgs(string name,int trackingID,int id)
        {
            Name = name;
            TrackingID = trackingID;
            ID = id;
        }
        
        #endregion
    }
}
