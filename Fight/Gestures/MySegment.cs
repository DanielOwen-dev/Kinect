using LightBuzz.Vitruvius;
using Microsoft.Kinect;

namespace Fight.Gestures
{
    class MySegment : IGestureSegment
    {
        private MySegment_condition[] Joint_Condition;
        public MySegment( MySegment_condition[] _Joint_Condition)
        {
            Joint_Condition=_Joint_Condition;
        }

        public GesturePartResult Update(Skeleton skeleton)
        {
            foreach( MySegment_condition a in Joint_Condition)
            {
                if (!a.Judge(skeleton)) return GesturePartResult.Failed;
            }
            return GesturePartResult.Succeeded;
        }
    }
}
