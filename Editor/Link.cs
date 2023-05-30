using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace SSJJ
{
    public class Link
    {
        // 0 ：start point, 1 : end point, 2 : control point 1, 3 : control point 2
        Vector3 [] mPoints=new Vector3[4];
        public string mStartRP, mEndRP;
        Texture2D mBackgroundTexture = null;
        public Link(Vector3 inPoint0,Vector3 inPoint1,Vector3 inPoint2,Vector3 inPoint3)
        {
            mPoints[0] = inPoint0;
            mPoints[1] = inPoint1;
            mPoints[2] = inPoint2;
            mPoints[3] = inPoint3;
        }

        public void Draw(Vector2 inContainerSize)
        {
            Rect drawArea = UpdateRect();
            mPoints[2].x = mPoints[0].x + 100;
            mPoints[2].y = mPoints[0].y;
            mPoints[3].x = mPoints[1].x - 100;
            mPoints[3].y = mPoints[1].y;
            /*if (mBackgroundTexture == null)
            {
                mBackgroundTexture = new Texture2D(1, 1);
                mBackgroundTexture.SetPixel(0, 0, new Color(0.1f, 0.4f, 0.6f, 0.5f));
                mBackgroundTexture.Apply();
            }
            GUI.DrawTexture(drawArea, mBackgroundTexture);
            Handles.DrawSolidDisc(mPoints[0], Vector3.forward, 2);
            Handles.DrawSolidDisc(mPoints[1], Vector3.forward, 2);
            Handles.DrawSolidDisc(mPoints[2], Vector3.forward, 4);
            Handles.DrawSolidDisc(mPoints[3], Vector3.forward, 4);*/
            //Debug.Log(mPoints[0]+","+mPoints[1] + "," + mPoints[2] + "," + mPoints[3]+","+ inContainerSize);
            Handles.DrawBezier(mPoints[0], mPoints[1], mPoints[2], mPoints[3], Color.green, null, 4.0f);
        }
        public void SetPoint(int inIndex,Vector3 inPoint)
        {
            mPoints[inIndex] = inPoint;
        }
        Rect UpdateRect()
        {
            float x = Mathf.Min(mPoints[0].x,mPoints[1].x);
            float y = Mathf.Min(mPoints[0].y, mPoints[1].y);
            float width = Mathf.Abs(mPoints[0].x - mPoints[1].x);
            float height = Mathf.Abs(mPoints[0].y - mPoints[1].y);
            return new Rect(x,y,width,height);
        }
    }
}
