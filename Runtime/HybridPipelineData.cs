using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Rendering;
using UnityEditor.ProjectWindowCallback;

#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif
using UnityEngine;
using Alice.Rendering;

namespace Alice.Rendering.Hybrid
{
    public class HybridPipelineData : ScriptableObject
    {
        [SerializeField]
        public List<string> RenderPasses = new List<string>();
        [SerializeField]
        public List<string> RenderTargets = new List<string>();
        [SerializeField]
        [HideInInspector]
        public List<UnityEngine.RenderTextureFormat> RenderTargetFormats = new List<UnityEngine.RenderTextureFormat>();
        [SerializeField]
        [HideInInspector]
        public List<Rect> RenderPassRects = new List<Rect>();
        [SerializeField]
        [HideInInspector]
        public List<Rect> RenderTargetRects = new List<Rect>();
        [SerializeField]
        [HideInInspector]
        public List<String> RenderPassColor0Outputs = new List<String>();
        [SerializeField]
        [HideInInspector]
        public List<String> RenderPassDSOutputs = new List<String>();
        [SerializeField]
        [HideInInspector]
        public List<EnterRenderPassAction> RenderPassOnEnterActions = new List<EnterRenderPassAction>();
        [SerializeField]
        [HideInInspector]
        public List<RenderBufferLoadAction> RenderPassColor0LoadActions = new List<RenderBufferLoadAction>();
        [SerializeField]
        [HideInInspector]
        public List<RenderBufferStoreAction> RenderPassColor0StoreActions = new List<RenderBufferStoreAction>();
        [SerializeField]
        [HideInInspector]
        public List<RenderBufferLoadAction> RenderPassDSLoadActions = new List<RenderBufferLoadAction>();
        [SerializeField]
        [HideInInspector]
        public List<RenderBufferStoreAction> RenderPassDSStoreActions = new List<RenderBufferStoreAction>();
        [SerializeField]
        [HideInInspector]
        public List<string> RenderPassLightModes = new List<string>();
        [SerializeField]
        [HideInInspector]
        public List<string> RenderPassIDs = new List<string>();
        [SerializeField]
        [HideInInspector]
        public List<string> RenderTargetIDs = new List<string>();
        [SerializeField]
        [HideInInspector]
        public List<string> RenderPassQueues = new List<string>();
        [SerializeField]
        [HideInInspector]
        public List<int> RenderPassLayerMasks = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<string> RenderPassScriptNames = new List<string>();
        [SerializeField]
        [HideInInspector]
        public List<string> Links = new List<string>();
#if UNITY_EDITOR
        internal class CreateHybridPipelineDataAction : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                AssetDatabase.CreateAsset(CreateInstance<HybridPipelineData>(),pathName);
            }
        }

        [MenuItem("Assets/Create/Rendering/Hybrid Pipeline Data")]
        static void CreateHybridPipelineData()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreateHybridPipelineDataAction>(),
                "New Pipeline Data.asset", null, null);
        }
#endif
    }
}
