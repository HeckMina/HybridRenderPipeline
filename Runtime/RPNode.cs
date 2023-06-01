using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

namespace Alice.Rendering
{
    public enum EnterRenderPassAction{
        NoneAction,
        SetLoadStore
    }
    public class RPNode : Node
    {
        public List<string> mDependentRTs=new List<string>();
        public string mOutputColor0="None", mOutputDS="None";
        public EnterRenderPassAction mEnterRenderPassAction=EnterRenderPassAction.NoneAction;
        public RenderBufferLoadAction mColorRTLoadAction=RenderBufferLoadAction.Load;
        public RenderBufferStoreAction mColorRTStoreAction=RenderBufferStoreAction.Store;
        public RenderBufferLoadAction mDSRTLoadAction=RenderBufferLoadAction.Load;
        public RenderBufferStoreAction mDSRTStoreAction=RenderBufferStoreAction.Store;
        public List<string> mLightModes=new List<string>();
        public string mQueue="Opaque";
        public int mLayerMask { get; set; }
        public string mAttachedScriptName="None";
        public RPNode(float inX, float inY, float inWidth, float inHeight) : base(inX, inY, inWidth, inHeight)
        {
            mName = "Render Pass";
        }
    }
}