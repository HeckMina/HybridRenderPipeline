using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Alice.Rendering
{
    public class RPSetting
    {
        const int mPadding = 2;
        public string mName = "Render Pass Settings";
        public RPNode mCurrentEdittingNode;
        Texture2D mBackgroundTexture = null;
        public Rect mRect;
        public RPSetting()
        {
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
            mRect = new Rect(inContainerSize.x - 200, 0, inContainerSize.x, 400);
            List<string> rts = new List<string>();
            rts.Add("None");
            RPEditor.mInstance.mNodes.ForEach((node) => { if (node.GetType() == typeof(RTNode)) { rts.Add(node.mName); } });
            int currentPosY = mPadding;
            GUI.DrawTexture(mRect, mBackgroundTexture);
            GUI.Label(new Rect(inContainerSize.x - 200 + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent(mCurrentEdittingNode.mName + " Settings"));
            //dependent prior rts
            currentPosY += 20 + mPadding;
            GUI.Label(new Rect(inContainerSize.x - 200 + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent("依赖的RT："));
            currentPosY += 20 + mPadding;
            GUILayout.BeginArea(new Rect(inContainerSize.x - 200 + mPadding, currentPosY, 200 - mPadding*2, 20));
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
                GUILayout.BeginArea(new Rect(inContainerSize.x - 200 + mPadding, currentPosY, 200 - mPadding * 3 - 20, 20));
                selectedOption = rts.IndexOf(dependentRT);
                selectedOption = EditorGUILayout.Popup(selectedOption, rts.ToArray());
                mCurrentEdittingNode.mDependentRTs[i] = rts[selectedOption];
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
            //output
            currentPosY += 20 + mPadding;
            GUI.Label(new Rect(inContainerSize.x - 200 + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent("颜色输出"));
            GUILayout.BeginArea(new Rect(inContainerSize.x - 100, currentPosY, 100 - mPadding, 20));
            selectedOption = rts.IndexOf(mCurrentEdittingNode.mOutputColor0);
            selectedOption = EditorGUILayout.Popup(selectedOption, rts.ToArray());
            mCurrentEdittingNode.mOutputColor0 = rts.ToArray()[selectedOption];
            GUILayout.EndArea();
            currentPosY += 20 + mPadding;
            GUI.Label(new Rect(inContainerSize.x - 200 + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent("深度/蒙版输出"));
            GUILayout.BeginArea(new Rect(inContainerSize.x - 100, currentPosY, 100 - mPadding, 20));
            selectedOption = rts.IndexOf(mCurrentEdittingNode.mOutputDS);
            selectedOption = EditorGUILayout.Popup(selectedOption, rts.ToArray());
            mCurrentEdittingNode.mOutputDS = rts.ToArray()[selectedOption];
            GUILayout.EndArea();
            currentPosY += 20 + mPadding;
            GUI.Label(new Rect(inContainerSize.x - 200 + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent("其它设置待迭代..."));
        }
    }
}