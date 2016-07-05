using Microsoft.Kinect;

namespace LightBuzz.Vitruvius.Gestures
{
    public class left : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            if (skeleton.Joints[JointType.HandLeft].Position.X < skeleton.Joints[JointType.ElbowLeft].Position.X)
                return GesturePartResult.Succeeded;
            else
                return GesturePartResult.Failed;
        }
    }
}
