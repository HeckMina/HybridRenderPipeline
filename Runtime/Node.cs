using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

namespace Alice.Rendering
{
    public class Node
    {
        public string mName = "Node";
        public string mID = System.Guid.NewGuid().ToString().Replace("-","");
        public Rect mRect;
        public Link mIncomingLink = null;
        public Link mOutgoingLink = null;
        public Node(float inX,float inY,float inWidth,float inHeight)
        {
            mRect = new Rect(inX, inY, inWidth, inHeight);
        }
        virtual public void Draw(bool inIsEditingName){}
        virtual public void OnEditName(){}
        virtual public void OnFinishedEditName(){}
        public Vector3 GetLeftConnectionPoint()
        {
            return new Vector3(mRect.xMin,mRect.yMin+mRect.height/2.0f,0.0f);
        }
        public Vector3 GetRightConnectionPoint()
        {
            return new Vector3(mRect.xMax, mRect.yMin + mRect.height / 2.0f, 0.0f);
        }
        public bool IsClickOnTheLeftPart(float x)
        {
            return mRect.xMin + mRect.width / 2.0 > x;
        }
    }
}
