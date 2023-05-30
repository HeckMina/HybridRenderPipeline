using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

namespace SSJJ
{
    public class Node
    {
        public string mName = "Node";
        public string mID = System.Guid.NewGuid().ToString();
        public bool mIsEditingName = false;
        public Rect mRect;
        public Link mIncomingLink = null, mOutgoingLink = null;
        public Node(float inX,float inY,float inWidth,float inHeight)
        {
            mRect = new Rect(inX, inY, inWidth, inHeight);
        }
        virtual public void Draw() { }
        public void OnEditName()
        {
            mIsEditingName = true;
            GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Keyboard);
            EditorGUI.FocusTextInControl(null);
        }
        public void OnFinishedEditName()
        {
            mIsEditingName = false;
            GUIUtility.hotControl = 0;
        }
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
