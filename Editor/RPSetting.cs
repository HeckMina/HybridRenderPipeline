using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using System.Reflection;
using System;
using System.Linq;

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
        int AdvanceVertical(int inCurrentYPos){
            return inCurrentYPos+mPadding;
        }
        int DrawTitle(Vector2 inContainerSize){
            int currentPosY = mPadding;
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent(mCurrentEdittingNode.mName + " Settings"));
            return AdvanceVertical(currentPosY+20);
        }
        int DrawDependentRTs(Vector2 inContainerSize,int inYStartPosition,List<string> inRenderTargets){
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, inYStartPosition, inContainerSize.x - mPadding, 20), new GUIContent("依赖的RT："));
            inYStartPosition += 20 + mPadding;
            GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width + mPadding, inYStartPosition, mRect.width - mPadding*2, 20));
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
                inYStartPosition += 20 + mPadding;
                GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width + mPadding, inYStartPosition, mRect.width - mPadding * 3 - 20, 20));
                selectedOption = inRenderTargets.IndexOf(dependentRT);
                selectedOption = EditorGUILayout.Popup(selectedOption, inRenderTargets.ToArray());
                GUILayout.EndArea();
                GUILayout.BeginArea(new Rect(inContainerSize.x - 24 + mPadding, inYStartPosition, 20, 20));
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
            return AdvanceVertical(inYStartPosition+20);
        }
        int DrawRenderTargetSettings(Vector2 inContainerSize,int inYStartPosition,List<string> inRenderTargets){
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, inYStartPosition, inContainerSize.x - mPadding, 20), new GUIContent("颜色输出"));
            GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width/2.0f, inYStartPosition, mRect.width/2.0f - mPadding, 20));
            int selectedOption = inRenderTargets.IndexOf(mCurrentEdittingNode.mOutputColor0);
            selectedOption = EditorGUILayout.Popup(selectedOption, inRenderTargets.ToArray());
            mCurrentEdittingNode.mOutputColor0 = inRenderTargets.ToArray()[selectedOption];
            GUILayout.EndArea();
            inYStartPosition += 20 + mPadding;
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, inYStartPosition, inContainerSize.x - mPadding, 20), new GUIContent("深度/蒙版输出"));
            GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width/2.0f, inYStartPosition, mRect.width/2.0f - mPadding, 20));
            selectedOption = inRenderTargets.IndexOf(mCurrentEdittingNode.mOutputDS);
            selectedOption = EditorGUILayout.Popup(selectedOption, inRenderTargets.ToArray());
            mCurrentEdittingNode.mOutputDS = inRenderTargets.ToArray()[selectedOption];
            GUILayout.EndArea();
            inYStartPosition += 20 + mPadding;
            //color rt load / store action
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, inYStartPosition, inContainerSize.x - mPadding, 20), new GUIContent("ColorBufferLoadAction"));
            GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width/2, inYStartPosition, mRect.width/2.0f - mPadding, 20));
            selectedOption = (int)mCurrentEdittingNode.mColorRTLoadAction;
            selectedOption = EditorGUILayout.Popup(selectedOption, typeof(RenderBufferLoadAction).GetEnumNames());
            mCurrentEdittingNode.mColorRTLoadAction = (RenderBufferLoadAction)selectedOption;
            GUILayout.EndArea();
            inYStartPosition += 20 + mPadding;
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, inYStartPosition, inContainerSize.x - mPadding, 20), new GUIContent("ColorBufferStoreAction"));
            GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width/2, inYStartPosition, mRect.width/2.0f - mPadding, 20));
            selectedOption = (int)mCurrentEdittingNode.mColorRTStoreAction;
            selectedOption = EditorGUILayout.Popup(selectedOption, typeof(RenderBufferStoreAction).GetEnumNames());
            mCurrentEdittingNode.mColorRTStoreAction = (RenderBufferStoreAction)selectedOption;
            GUILayout.EndArea();
            inYStartPosition += 20 + mPadding;
            //ds rt load / store action
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, inYStartPosition, inContainerSize.x - mPadding, 20), new GUIContent("DSBufferLoadAction"));
            GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width/2, inYStartPosition, mRect.width/2.0f - mPadding, 20));
            selectedOption = (int)mCurrentEdittingNode.mColorRTLoadAction;
            selectedOption = EditorGUILayout.Popup(selectedOption, typeof(RenderBufferLoadAction).GetEnumNames());
            mCurrentEdittingNode.mDSRTLoadAction = (RenderBufferLoadAction)selectedOption;
            GUILayout.EndArea();
            inYStartPosition += 20 + mPadding;
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, inYStartPosition, inContainerSize.x - mPadding, 20), new GUIContent("DSBufferStoreAction"));
            GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width/2, inYStartPosition, mRect.width/2.0f - mPadding, 20));
            selectedOption = (int)mCurrentEdittingNode.mDSRTStoreAction;
            selectedOption = EditorGUILayout.Popup(selectedOption, typeof(RenderBufferStoreAction).GetEnumNames());
            mCurrentEdittingNode.mDSRTStoreAction = (RenderBufferStoreAction)selectedOption;
            GUILayout.EndArea();
            return AdvanceVertical(inYStartPosition+20);
        }
        int DrawAttachedRenderScript(Vector2 inContainerSize,int inYStartPosition){
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, inYStartPosition, inContainerSize.x - mPadding, 20), new GUIContent("选择RenderPass"));
            GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width/2.0f, inYStartPosition, mRect.width/2.0f - mPadding, 20));

            List<string> scriptableRenderPasses=new List<string>();
            {
                Assembly assembly = Assembly.LoadFrom("E:/UnityProjects/URPAdapter/Library/ScriptAssemblies/Unity.RenderPipelines.Universal.Runtime.dll");
                Type typeScriptableRenderPass = assembly.GetType("UnityEngine.Rendering.Universal.ScriptableRenderPass");
                if (typeScriptableRenderPass != null)
                {
                    Type[] subTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeScriptableRenderPass)).ToArray();
                    foreach (Type subType in subTypes)
                    {
                        scriptableRenderPasses.Add(subType.GetTypeInfo().Name);
                    }
                }else{
                    Debug.Log("Canonot find UnityEngine.Rendering.Universal.ScriptableRenderPass");
                }
            }
            int selectedOption=0;
            selectedOption = EditorGUILayout.Popup(selectedOption, scriptableRenderPasses.ToArray());
            GUILayout.EndArea();
            return AdvanceVertical(inYStartPosition+20);
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
            int contentHeight=0;
            mRect = new Rect(inContainerSize.x - mRect.width, 0, mRect.width,mRect.height);
            GUI.DrawTexture(mRect, mBackgroundTexture);
            List<string> renderTargetNames = new List<string>();
            renderTargetNames.Add("None");
            RPEditor.mInstance.mNodes.ForEach((node) => { if (node.GetType() == typeof(RTNode)) { renderTargetNames.Add(node.mName); } });
            //draw title
            int currentPosY=DrawTitle(inContainerSize);
            //dependent prior rts
            currentPosY=DrawDependentRTs(inContainerSize,currentPosY,renderTargetNames);
            //render target
            currentPosY=DrawRenderTargetSettings(inContainerSize,currentPosY,renderTargetNames);
            //render script
            currentPosY=DrawAttachedRenderScript(inContainerSize,currentPosY);
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, currentPosY, inContainerSize.x - mPadding, 20), new GUIContent("其它设置待迭代..."));
            contentHeight=AdvanceVertical(currentPosY+20);
            if(contentHeight!=mRect.height){
                mRect.height=contentHeight;
                GUI.changed=true;
            }
        }
    }
}