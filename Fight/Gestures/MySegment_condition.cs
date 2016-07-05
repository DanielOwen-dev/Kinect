using Microsoft.Kinect;

namespace Fight.Gestures
{
    class MySegment_condition
    {
        int type;//0-2
        private JointType[] Joint;
        int num;
        public MySegment_condition(int _type, JointType _Joint1, JointType _Joint2, int _num)
        {
            type = _type;
            Joint = new JointType[2];
            Joint[0] = _Joint1;
            Joint[1] = _Joint2;
        }

        public bool Judge(Skeleton skeleton)
        {
            float a, b;
            a = 0; b = 0;
            if (type == 0)//Z
            {
                a = skeleton.Joints[Joint[0]].Position.Z;
                b = skeleton.Joints[Joint[1]].Position.Z;
            }
            else if (type == 1)//Y
            {
                a = skeleton.Joints[Joint[0]].Position.Y;
                b = skeleton.Joints[Joint[1]].Position.Y;
            }
            else if (type == 2)//X
            {
                a = skeleton.Joints[Joint[0]].Position.X;
                b = skeleton.Joints[Joint[1]].Position.X;
            }
            else return false;
            return a > b + num;
        }
    }
}
