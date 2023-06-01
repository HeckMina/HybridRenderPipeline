using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace Alice.Rendering
{
    public class Link
    {
        // 0 ：start point, 1 : end point, 2 : control point 1, 3 : control point 2
        public Vector3 [] mPoints=new Vector3[4];
        public string mStartRP, mEndRP;
        public Link(Link inOther){
            mStartRP=inOther.mStartRP;
            mEndRP=inOther.mEndRP;
            mPoints[0]=inOther.mPoints[0];
            mPoints[1]=inOther.mPoints[1];
            mPoints[2]=inOther.mPoints[2];
            mPoints[3]=inOther.mPoints[3];
        }
        public Link(Vector3 inPoint0,Vector3 inPoint1)
        {
            mPoints[0] = inPoint0;
            mPoints[1] = inPoint1;
            mPoints[2] = inPoint0;
            mPoints[3] = inPoint1;
        }

        public void SetPoint(int inIndex,Vector3 inPoint)
        {
            mPoints[inIndex] = inPoint;
        }
        virtual public void Draw() { }
    }
}
