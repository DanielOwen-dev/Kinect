using Microsoft.Kinect;

namespace LightBuzz.Vitruvius.Gestures
{
    public class right : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            if (skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.ElbowRight].Position.X)
                return GesturePartResult.Succeeded;
            else
                return GesturePartResult.Failed;
        }
    }
}
