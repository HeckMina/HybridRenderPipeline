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

namespace Alice.Rendering.Hybrid
{
    [ExcludeFromPreset]
    public class PipelineData : ScriptableObject
    {
#if UNITY_EDITOR
        internal class CreateHybridPipelineDataAction : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                AssetDatabase.CreateAsset(CreateInstance<PipelineData>(),pathName);
            }
        }

        [MenuItem("Assets/Create/Rendering/Hybrid Pipeline Data")]
        static void CreateHybridPipelineData()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreateHybridPipelineDataAction>(),
                "New Pipeline Data.asset", null, null);
        }
#endif
        internal bool isInvalidated { get; set; }
        public new void SetDirty()
        {
            isInvalidated = true;
        }
        protected virtual void OnValidate()
        {
            SetDirty();
        }
        protected virtual void OnEnable()
        {
            SetDirty();
        }
    }
}
