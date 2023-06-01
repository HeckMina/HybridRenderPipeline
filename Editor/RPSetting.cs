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
        const float mPadding = 2.0f;
        const float mItemHeight = 20.0f;
        public string mName = "Render Pass Settings";
        public RPNodeEditor mCurrentEdittingNode;
        Texture2D mBackgroundTexture = null;
        public Rect mRect;
        public RPSetting(float inX, float inY, float inWidth, float inHeight)
        {
            mRect=new Rect(inX,inY,inWidth,inHeight);
        }
        void DrawPopupSelections(float inX,float inY,float inContainerWidth,ref string inoutProperty,String inSelectedOptionName,List<string>inOptions){
            GUI.Label(new Rect(inX + mPadding, inY, inX + inContainerWidth - mPadding, mItemHeight), new GUIContent(inSelectedOptionName));
            GUILayout.BeginArea(new Rect(inX + inContainerWidth/2.0f, inY, inContainerWidth/2.0f - mPadding, mItemHeight));
            int selectedOption=inOptions.IndexOf(inoutProperty);
            selectedOption = EditorGUILayout.Popup(selectedOption, inOptions.ToArray());
            inoutProperty=inOptions[selectedOption];
            GUILayout.EndArea();
        }
        int DrawPopupBitMask(float inX,float inY,float inContainerWidth,int inoutProperty,String inSelectedOptionName,string[]inOptions){
            GUILayout.BeginArea(new Rect(inX + mPadding, inY, inContainerWidth - mPadding * 2.0f, mItemHeight));
            inoutProperty = EditorGUILayout.MaskField(inSelectedOptionName, inoutProperty, inOptions);
            GUILayout.EndArea();
            return inoutProperty;
        }
        float AdvanceVertical(float inCurrentYPos){
            return inCurrentYPos+mPadding;
        }
        float DrawTitle(Vector2 inContainerSize){
            float currentPosY = mPadding;
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, currentPosY, inContainerSize.x - mPadding, mItemHeight), new GUIContent(mCurrentEdittingNode.mName + " Settings"));
            return AdvanceVertical(currentPosY+20);
        }
        float DrawDependentRTs(Vector2 inContainerSize,float inYStartPosition,List<string> inRenderTargets){
            GUI.Label(new Rect(inContainerSize.x - mRect.width + mPadding, inYStartPosition, inContainerSize.x - mPadding, mItemHeight), new GUIContent("依赖的RT："));
            inYStartPosition += 20.0f + mPadding;
            GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width + mPadding, inYStartPosition, mRect.width - mPadding*2.0f, mItemHeight));
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
                GUILayout.BeginArea(new Rect(inContainerSize.x - mRect.width + mPadding, inYStartPosition, mRect.width - mPadding * 3 - mItemHeight, mItemHeight));
                selectedOption = inRenderTargets.IndexOf(dependentRT);
                selectedOption = EditorGUILayout.Popup(selectedOption, inRenderTargets.ToArray());
                GUILayout.EndArea();
                GUILayout.BeginArea(new Rect(inContainerSize.x - 24 + mPadding, inYStartPosition, mItemHeight, mItemHeight));
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
        float DrawRenderTargetSettings(float inX,float inY,float inContainerWidth,List<string> inRenderTargetNames,List<string> inRenderTargetIDs){
            GUI.Label(new Rect(inX + mPadding, inY, inX + inContainerWidth - mPadding, 20), new GUIContent("颜色输出"));
            GUILayout.BeginArea(new Rect(inX + mRect.width/2.0f, inY, mRect.width/2.0f - mPadding, 20));
            int selectedOption = inRenderTargetIDs.IndexOf(mCurrentEdittingNode.mOutputColor0);
            selectedOption = EditorGUILayout.Popup(selectedOption, inRenderTargetNames.ToArray());
            mCurrentEdittingNode.mOutputColor0 = inRenderTargetIDs.ToArray()[selectedOption];
            GUILayout.EndArea();
            inY = AdvanceVertical(inY+20);
            GUI.Label(new Rect(inX + mPadding, inY, inX + inContainerWidth - mPadding, 20), new GUIContent("深度/蒙版输出"));
            GUILayout.BeginArea(new Rect(inX + mRect.width/2.0f, inY, mRect.width/2.0f - mPadding, 20));
            selectedOption = inRenderTargetIDs.IndexOf(mCurrentEdittingNode.mOutputDS);
            selectedOption = EditorGUILayout.Popup(selectedOption, inRenderTargetNames.ToArray());
            mCurrentEdittingNode.mOutputDS = inRenderTargetIDs.ToArray()[selectedOption];
            GUILayout.EndArea();
            inY = AdvanceVertical(inY+20);
            string currentSelectedEnterRenderPassAction=mCurrentEdittingNode.mEnterRenderPassAction.ToString();
            DrawPopupSelections(inX,inY,inContainerWidth,ref currentSelectedEnterRenderPassAction,"OnStart操作",typeof(EnterRenderPassAction).GetEnumNames().ToList());
            mCurrentEdittingNode.mEnterRenderPassAction=(EnterRenderPassAction)typeof(EnterRenderPassAction).GetEnumNames().ToList().IndexOf(currentSelectedEnterRenderPassAction);
            //color rt load / store action
            if(mCurrentEdittingNode.mEnterRenderPassAction==EnterRenderPassAction.SetLoadStore){
                inY=AdvanceVertical(inY+20);
                GUI.Label(new Rect(inX + mPadding, inY, inX + inContainerWidth - mPadding, 20), new GUIContent("ColorBufferLoadAction"));
                GUILayout.BeginArea(new Rect(inX + mRect.width/2, inY, mRect.width/2.0f - mPadding, 20));
                selectedOption = (int)mCurrentEdittingNode.mColorRTLoadAction;
                selectedOption = EditorGUILayout.Popup(selectedOption, typeof(RenderBufferLoadAction).GetEnumNames());
                mCurrentEdittingNode.mColorRTLoadAction = (RenderBufferLoadAction)selectedOption;
                GUILayout.EndArea();
                inY = AdvanceVertical(inY+20);
                GUI.Label(new Rect(inX + mPadding, inY, inX + inContainerWidth - mPadding, 20), new GUIContent("ColorBufferStoreAction"));
                GUILayout.BeginArea(new Rect(inX + mRect.width/2, inY, mRect.width/2.0f - mPadding, 20));
                selectedOption = (int)mCurrentEdittingNode.mColorRTStoreAction;
                selectedOption = EditorGUILayout.Popup(selectedOption, typeof(RenderBufferStoreAction).GetEnumNames());
                mCurrentEdittingNode.mColorRTStoreAction = (RenderBufferStoreAction)selectedOption;
                GUILayout.EndArea();
                inY = AdvanceVertical(inY+20);
                //ds rt load / store action
                GUI.Label(new Rect(inX + mPadding, inY, inX + inContainerWidth - mPadding, 20), new GUIContent("DSBufferLoadAction"));
                GUILayout.BeginArea(new Rect(inX + mRect.width/2, inY, mRect.width/2.0f - mPadding, 20));
                selectedOption = (int)mCurrentEdittingNode.mDSRTLoadAction;
                selectedOption = EditorGUILayout.Popup(selectedOption, typeof(RenderBufferLoadAction).GetEnumNames());
                mCurrentEdittingNode.mDSRTLoadAction = (RenderBufferLoadAction)selectedOption;
                GUILayout.EndArea();
                inY = AdvanceVertical(inY+20);
                GUI.Label(new Rect(inX + mPadding, inY, inX + inContainerWidth - mPadding, 20), new GUIContent("DSBufferStoreAction"));
                GUILayout.BeginArea(new Rect(inX + mRect.width/2, inY, mRect.width/2.0f - mPadding, 20));
                selectedOption = (int)mCurrentEdittingNode.mDSRTStoreAction;
                selectedOption = EditorGUILayout.Popup(selectedOption, typeof(RenderBufferStoreAction).GetEnumNames());
                mCurrentEdittingNode.mDSRTStoreAction = (RenderBufferStoreAction)selectedOption;
                GUILayout.EndArea();
            }
            return AdvanceVertical(inY+20);
        }
        float DrawAttachedRenderScript(float inX,float inY,float inWidth){
            List<string> scriptableRenderPasses=new List<string>();
            scriptableRenderPasses.Add("None");
            Assembly assembly = Assembly.LoadFrom("Library/ScriptAssemblies/Unity.RenderPipelines.Universal.Runtime.dll");
            {
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
            {
                Type typeScriptableRenderFeature = assembly.GetType("UnityEngine.Rendering.Universal.ScriptableRendererFeature");
                if (typeScriptableRenderFeature != null)
                {
                    Type[] subTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeScriptableRenderFeature)).ToArray();
                    foreach (Type subType in subTypes)
                    {
                        scriptableRenderPasses.Add(subType.GetTypeInfo().Name);
                    }
                    {
                        assembly = Assembly.LoadFrom("Library/ScriptAssemblies/Assembly-CSharp.dll");
                        subTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeScriptableRenderFeature)).ToArray();
                        foreach (Type subType in subTypes)
                        {
                            scriptableRenderPasses.Add(subType.GetTypeInfo().Name);
                        }
                    }
                }else{
                    Debug.Log("Canonot find UnityEngine.Rendering.Universal.ScriptableRendererFeature");
                }
            }
            DrawPopupSelections(inX,inY,inWidth,ref mCurrentEdittingNode.mAttachedScriptName,"绑定的脚本",scriptableRenderPasses);
            inY=AdvanceVertical(inY+20);
            string [] queues={"Opaque","Transparent"};
            DrawPopupSelections(inX,inY,inWidth,ref mCurrentEdittingNode.mQueue,"Queue",queues.ToList());
            inY=AdvanceVertical(inY+20);
            mCurrentEdittingNode.mLayerMask=DrawPopupBitMask(inX,inY,inWidth,mCurrentEdittingNode.mLayerMask,"Layer Mask",UnityEditorInternal.InternalEditorUtility.layers);
            return AdvanceVertical(inY+20);
        }
        float DrawLigtModes(float inX,float inY,float inWidth){
            int validLightModeCount=mCurrentEdittingNode.mLightModes.Count;
            GUI.Label(new Rect(inX + mPadding, inY, inWidth - mPadding, mItemHeight), new GUIContent("LightMode："));
            inY=AdvanceVertical(inY+20);
            GUILayout.BeginArea(new Rect(inX + mPadding, inY, mRect.width - mPadding, mItemHeight));
            if (GUILayout.Button("+"))
            {
                mCurrentEdittingNode.mLightModes.Add("");
            }
            GUILayout.EndArea();
            inY=AdvanceVertical(inY+20);
            for(int i=0;i<validLightModeCount;++i){
                GUILayout.BeginArea(new Rect(inX+mPadding, inY, inWidth - mPadding, mItemHeight));
                string lightMode=mCurrentEdittingNode.mLightModes[i];
                lightMode = GUILayout.TextField(lightMode);
                mCurrentEdittingNode.mLightModes[i]=lightMode;
                GUILayout.EndArea();
                inY=AdvanceVertical(inY+20);
            }
            return inY;
        }
        public void Draw(Vector2 inContainerSize, RPNodeEditor inRPNode)
        {
            mCurrentEdittingNode = inRPNode;
            if (mBackgroundTexture == null)
            {
                mBackgroundTexture = new Texture2D(1, 1);
                mBackgroundTexture.SetPixel(0, 0, new Color(0.0f, 0.0f, 0.0f, 0.5f));
                mBackgroundTexture.Apply();
            }
            float contentHeight=0.0f;
            float contentWidth=mRect.width;
            float startX=inContainerSize.x-contentWidth;
            mRect = new Rect(inContainerSize.x - mRect.width, 0, mRect.width,mRect.height);
            GUI.DrawTexture(mRect, mBackgroundTexture);
            List<string> renderTargetNames = new List<string>();
            List<string> renderTargetIDs = new List<string>();
            renderTargetNames.Add("None");
            renderTargetIDs.Add("None");
            RPEditor.mInstance.mNodes.ForEach((node) => { 
                if (node.GetType() == typeof(RTNodeEditor)) { 
                    renderTargetNames.Add(node.mName); 
                    renderTargetIDs.Add(node.mID);
                }
            });
            //draw title
            float currentPosY=DrawTitle(inContainerSize);
            //dependent prior rts
            currentPosY=DrawDependentRTs(inContainerSize,currentPosY,renderTargetNames);
            //render target
            currentPosY=DrawRenderTargetSettings(startX,currentPosY,contentWidth,renderTargetNames,renderTargetIDs);
            //render script
            currentPosY=DrawAttachedRenderScript(startX,currentPosY,contentWidth);
            currentPosY=DrawLigtModes(startX,currentPosY,contentWidth);
            contentHeight=currentPosY;
            if(contentHeight!=mRect.height){
                mRect.height=contentHeight;
                GUI.changed=true;
            }
        }
    }
}