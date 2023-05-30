using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Alice.Rendering
{
    public class RTSetting
    {
        const int mPadding = 2;
        public string mName= "Render Target Settings";
        public RTNode mCurrentEdittingNode;
        Texture2D mBackgroundTexture=null;
        public Rect mRect;
        public RTSetting()
        {
        }
        public void Draw(Vector2 inContainerSize, RTNode inRTNode)
        {
            if (mBackgroundTexture == null)
            {
                mBackgroundTexture = new Texture2D(1, 1);
                mBackgroundTexture.SetPixel(0, 0, new Color(0.0f, 0.0f, 0.0f, 0.5f));
                mBackgroundTexture.Apply();
            }
            mRect = new Rect(inContainerSize.x - 200, 0, inContainerSize.x, 400);
            mCurrentEdittingNode = inRTNode;
            int currentPosY = mPadding;
            GUI.DrawTexture(mRect, mBackgroundTexture);
            GUI.Label(new Rect(inContainerSize.x - 200 + mPadding, currentPosY, inContainerSize.x - mPadding, 20),new GUIContent(mCurrentEdittingNode.mName+" Settings"));
            currentPosY += 20+mPadding;
            GUI.Label(new Rect(inContainerSize.x - 200 + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent("Format"));
            GUILayout.BeginArea(new Rect(inContainerSize.x - 100, currentPosY, 100 - mPadding , 20));
            int selectedOption = (int)mCurrentEdittingNode.mFormat; 
            selectedOption = EditorGUILayout.Popup(selectedOption, typeof(RenderTextureFormat).GetEnumNames());
            mCurrentEdittingNode.mFormat = (RenderTextureFormat)(typeof(RenderTextureFormat).GetEnumValues().GetValue(selectedOption));
            GUILayout.EndArea();
            currentPosY += 20 + mPadding;
            GUI.Label(new Rect(inContainerSize.x - 200 + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent("其它设置待迭代..."));
        }
    }
}
