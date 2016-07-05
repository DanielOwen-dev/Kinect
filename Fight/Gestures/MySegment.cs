using LightBuzz.Vitruvius;
using Microsoft.Kinect;

namespace LightBuzz.Vitruvius
{
    class MySegment : IGestureSegment
    {
        private MySegment_condition[] MySegmentCondition;
        public MySegment( MySegment_condition[] _MySegmentCondition)
        {
            MySegmentCondition = _MySegmentCondition;
        }

        public GesturePartResult Update(Skeleton skeleton)
        {
            foreach( MySegment_condition a in MySegmentCondition)
            {
                if (!a.Judge(skeleton)) return GesturePartResult.Failed;
            }
            return GesturePartResult.Succeeded;
        }
    }
}
