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

    static class ShaderPropertyId
    {
        public static readonly int glossyEnvironmentColor;
        public static readonly int projectionMatrix;
        public static readonly int viewAndProjectionMatrix;
        public static readonly int inverseViewMatrix;
        public static readonly int inverseProjectionMatrix;
        public static readonly int inverseViewAndProjectionMatrix;
        public static readonly int cameraProjectionMatrix;
        public static readonly int inverseCameraProjectionMatrix;
        public static readonly int worldToCameraMatrix;
        public static readonly int cameraToWorldMatrix;
        public static readonly int cameraWorldClipPlanes;
        public static readonly int billboardNormal;
        public static readonly int billboardTangent;
        public static readonly int billboardCameraParams;
        public static readonly int sourceTex;
        public static readonly int scaleBias;
        public static readonly int viewMatrix;
        public static readonly int screenSize;
        public static readonly int globalMipBias;
        public static readonly int orthoParams;
        public static readonly int subtractiveShadowColor;
        public static readonly int glossyEnvironmentCubeMap;
        public static readonly int glossyEnvironmentCubeMapHDR;
        public static readonly int ambientSkyColor;
        public static readonly int ambientEquatorColor;
        public static readonly int ambientGroundColor;
        public static readonly int time;
        public static readonly int scaleBiasRt;
        public static readonly int sinTime;
        public static readonly int deltaTime;
        public static readonly int timeParameters;
        public static readonly int scaledScreenParams;
        public static readonly int worldSpaceCameraPos;
        public static readonly int screenParams;
        public static readonly int projectionParams;
        public static readonly int zBufferParams;
        public static readonly int cosTime;
        public static readonly int rendererColor;
    }
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
