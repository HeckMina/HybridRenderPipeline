using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

namespace Alice.Rendering
{
    public class RPSetting
    {
        const int mPadding = 2;
        public string mName = "Render Pass Settings";
        public RPNode mCurrentEdittingNode;
        Texture2D mBackgroundTexture = null;
        public Rect mRect;
        public RPSetting(float inX, float inY, float inWidth, float inHeight)
        {
            mRect=new Rect(inX,inY,inWidth,inHeight);
        }
        public void Draw(Vector2 inContainerSize, RPNode inRPNode)
        {
            mCurrentEdittingNode = inRPNode;
            if (mBackgroundTexture == null)
            {
                mBackgroundTexture = new Texture2D(1, 1);
                mBackgroundTexture.SetPixel(0, 0, new Color(0.0f, 0.0f, 0.0f, 0.5f));
                mBackgroundTexture.Apply();
            }
            mRect = new Rect(inContainerSize.x - mRect.width, 0, mRect.width,mRect.height);
            List<string> rts = new List<string>();
            rts.Add("None");
            RPEditor.mInstance.mNodes.ForEach((node) => { if (node.GetType() == typeof(RTNode)) { rts.Add(node.mName); } });
            int currentPosY = mPadding;
            GUI.DrawTexture(mRect, mBackgroundTexture);
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent(mCurrentEdittingNode.mName + " Settings"));
            //dependent prior rts
            currentPosY += 20 + mPadding;
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent("依赖的RT："));
            currentPosY += 20 + mPadding;
            GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width + mPadding, currentPosY, mRect.width - mPadding*2, 20));
            if (GUILayout.Button("+"))
            {
                mCurrentEdittingNode.mDependentRTs.Add("None");
            }
            GUILayout.EndArea();
            int selectedOption = 0;
            int dependentRTToDelete = -1;
            for (int i=0;i<mCurrentEdittingNode.mDependentRTs.Count;i++)
            {
                string dependentRT = mCurrentEdittingNode.mDependentRTs[i];
                currentPosY += 20 + mPadding;
                GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width + mPadding, currentPosY, mRect.width - mPadding * 3 - 20, 20));
                selectedOption = rts.IndexOf(dependentRT);
                selectedOption = EditorGUILayout.Popup(selectedOption, rts.ToArray());
                //mCurrentEdittingNode.mDependentRTs[i] = rts[selectedOption];
                GUILayout.EndArea();
                GUILayout.BeginArea(new Rect(inContainerSize.x - 24 + mPadding, currentPosY, 20, 20));
                if (GUILayout.Button("x"))
                {
                    dependentRTToDelete = i;
                }
                GUILayout.EndArea();
            }
            if (dependentRTToDelete != -1)
            {
                mCurrentEdittingNode.mDependentRTs.RemoveAt(dependentRTToDelete);
            }
            //render target
            currentPosY += 20 + mPadding;
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent("颜色输出"));
            GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width/2.0f, currentPosY, mRect.width/2.0f - mPadding, 20));
            selectedOption = rts.IndexOf(mCurrentEdittingNode.mOutputColor0);
            selectedOption = EditorGUILayout.Popup(selectedOption, rts.ToArray());
            mCurrentEdittingNode.mOutputColor0 = rts.ToArray()[selectedOption];
            GUILayout.EndArea();
            currentPosY += 20 + mPadding;
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent("深度/蒙版输出"));
            GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width/2.0f, currentPosY, mRect.width/2.0f - mPadding, 20));
            selectedOption = rts.IndexOf(mCurrentEdittingNode.mOutputDS);
            selectedOption = EditorGUILayout.Popup(selectedOption, rts.ToArray());
            mCurrentEdittingNode.mOutputDS = rts.ToArray()[selectedOption];
            GUILayout.EndArea();
            //color rt load / store action
            currentPosY += 20 + mPadding;
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent("ColorBufferLoadAction"));
            GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width/2, currentPosY, mRect.width/2.0f - mPadding, 20));
            selectedOption = (int)mCurrentEdittingNode.mColorRTLoadAction;
            selectedOption = EditorGUILayout.Popup(selectedOption, typeof(RenderBufferLoadAction).GetEnumNames());
            mCurrentEdittingNode.mColorRTLoadAction = (RenderBufferLoadAction)selectedOption;
            GUILayout.EndArea();
            currentPosY += 20 + mPadding;
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent("ColorBufferStoreAction"));
            GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width/2, currentPosY, mRect.width/2.0f - mPadding, 20));
            selectedOption = (int)mCurrentEdittingNode.mColorRTStoreAction;
            selectedOption = EditorGUILayout.Popup(selectedOption, typeof(RenderBufferStoreAction).GetEnumNames());
            mCurrentEdittingNode.mColorRTStoreAction = (RenderBufferStoreAction)selectedOption;
            GUILayout.EndArea();
            //ds rt load / store action
            currentPosY += 20 + mPadding;
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent("DSBufferLoadAction"));
            GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width/2, currentPosY, mRect.width/2.0f - mPadding, 20));
            selectedOption = (int)mCurrentEdittingNode.mColorRTLoadAction;
            selectedOption = EditorGUILayout.Popup(selectedOption, typeof(RenderBufferLoadAction).GetEnumNames());
            mCurrentEdittingNode.mDSRTLoadAction = (RenderBufferLoadAction)selectedOption;
            GUILayout.EndArea();
            currentPosY += 20 + mPadding;
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent("DSBufferStoreAction"));
            GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width/2, currentPosY, mRect.width/2.0f - mPadding, 20));
            selectedOption = (int)mCurrentEdittingNode.mDSRTStoreAction;
            selectedOption = EditorGUILayout.Popup(selectedOption, typeof(RenderBufferStoreAction).GetEnumNames());
            mCurrentEdittingNode.mDSRTStoreAction = (RenderBufferStoreAction)selectedOption;
            GUILayout.EndArea();
            currentPosY += 20 + mPadding;
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent("其它设置待迭代..."));
        }
    }
}