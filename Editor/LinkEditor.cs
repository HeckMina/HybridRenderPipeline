using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Alice.Rendering
{
    public class LinkEditor : Link
    {
        public LinkEditor(Vector3 inPoint0,Vector3 inPoint1,Vector3 inPoint2,Vector3 inPoint3):base(inPoint0,inPoint1,inPoint2,inPoint3)
        {
        }
        public void Draw(Vector2 inContainerSize)   
        {
            mPoints[2].x = mPoints[0].x + 100;
            mPoints[2].y = mPoints[0].y;
            mPoints[3].x = mPoints[1].x - 100;
            mPoints[3].y = mPoints[1].y;
            Handles.DrawBezier(mPoints[0], mPoints[1], mPoints[2], mPoints[3], Color.green, null, 4.0f);
        }
    }
}
