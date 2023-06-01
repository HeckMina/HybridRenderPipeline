using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Alice.Rendering
{
    public class RPNodeEditor : RPNode
    {
        public RPNodeEditor(RPNode inRPNode):base(inRPNode.mRect.x,inRPNode.mRect.y,inRPNode.mRect.width,inRPNode.mRect.height){

        }
        public RPNodeEditor(Rect inRect):base(inRect.x,inRect.y,inRect.width,inRect.height)
        {
        }
        public RPNodeEditor(float inX,float inY,float inWidth,float inHeight):base(inX,inY,inWidth,inHeight)
        {
        }
        override public void OnEditName()
        {
            GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Keyboard);
            EditorGUI.FocusTextInControl(null);
        }
        override public void OnFinishedEditName()
        {
            GUIUtility.hotControl = 0;
        }
        GUIStyle mTextStyle = null;
        override public void Draw(bool inIsEditingName)
        {
            if (mTextStyle == null)
            {
                mTextStyle = new GUIStyle(GUI.skin.label);
                mTextStyle.alignment = TextAnchor.MiddleCenter;
            }
            Handles.DrawSolidRectangleWithOutline(mRect, new Color(0.1f, 0.1f, 0.1f), new Color(0.6f, 0.4f, 0.1f));
            if (inIsEditingName)
            {
                GUIStyle style = new GUIStyle(GUI.skin.textField);
                style.alignment = TextAnchor.MiddleCenter;
                GUI.SetNextControlName("TextField");
                mName = EditorGUI.TextField(mRect, mName, style);
                GUI.FocusControl("TextField");
            }
            else
            {
                GUI.Label(mRect, new GUIContent(mName), mTextStyle);
            }
        }
    }
}
