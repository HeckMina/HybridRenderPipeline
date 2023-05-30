using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Alice.Rendering
{
    public class RTNode : Node
    {
        public UnityEngine.RenderTextureFormat mFormat;
        public RTNode(float inX, float inY, float inWidth, float inHeight):base(inX,inY,inWidth,inHeight)
        {
            mName="RenderTarget";
        }
        GUIStyle mTextStyle = null;
        override public void Draw()
        {
            if (mTextStyle == null)
            {
                mTextStyle = new GUIStyle(GUI.skin.label);
                mTextStyle.alignment = TextAnchor.MiddleCenter;
            }
            Handles.DrawSolidRectangleWithOutline(mRect, new Color(0.1f, 0.4f, 0.6f), new Color(0.2f,0.8f,1.0f));
            if (mIsEditingName)
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
