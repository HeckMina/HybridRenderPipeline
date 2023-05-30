using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Alice.Rendering
{
    public class Link
    {
        // 0 ：start point, 1 : end point, 2 : control point 1, 3 : control point 2
        protected Vector3 [] mPoints=new Vector3[4];
        public string mStartRP, mEndRP;
        public Link(Vector3 inPoint0,Vector3 inPoint1,Vector3 inPoint2,Vector3 inPoint3)
        {
            mPoints[0] = inPoint0;
            mPoints[1] = inPoint1;
            mPoints[2] = inPoint2;
            mPoints[3] = inPoint3;
        }

        public void SetPoint(int inIndex,Vector3 inPoint)
        {
            mPoints[inIndex] = inPoint;
        }
        virtual public void Draw() { }
    }
}
