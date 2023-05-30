using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Alice.Rendering
{
    public class RPNode : Node
    {
        public List<string> mDependentRTs=new List<string>();
        public string mOutputColor0="None", mOutputDS="None";
        GUIStyle mTextStyle = null;
        public RPNode(float inX, float inY, float inWidth, float inHeight) : base(inX, inY, inWidth, inHeight)
        {
            mName = "Render Pass";
        }
        override public void Draw()
        {

            if (mTextStyle == null)
            {
                mTextStyle = new GUIStyle(GUI.skin.label);
                mTextStyle.alignment = TextAnchor.MiddleCenter;
            }
            Handles.DrawSolidRectangleWithOutline(mRect, new Color(0.1f, 0.1f, 0.1f), new Color(0.6f, 0.4f, 0.1f));
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