using System;
using Unity.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Rendering.Universal;
#endif
using UnityEngine.Scripting.APIUpdating;
using Lightmapping = UnityEngine.Experimental.GlobalIllumination.Lightmapping;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;
namespace Alice.Rendering.Hybrid
{
    struct HybridRenderPass{

    }
    //interface for engine callback
    public class HybridRenderPipeline : RenderPipeline
    {
        List<HybridRenderPass> mRenderPasses;
        static RenderTargetHandle m_ActiveCameraColorAttachment;
        static RenderTargetHandle m_ActiveCameraDepthAttachment;
        static RenderBufferLoadAction m_ActiveColorLoadAction = RenderBufferLoadAction.Clear;
        static RenderBufferStoreAction m_ActiveColorStoreAction = RenderBufferStoreAction.Store;
        static RenderBufferLoadAction m_ActiveDSLoadAction = RenderBufferLoadAction.Clear;
        static RenderBufferStoreAction m_ActiveDSStoreAction = RenderBufferStoreAction.Store;
        static RenderTargetIdentifier m_CameraColorTarget;
        static RenderTargetIdentifier m_CameraDepthTarget;
        static RenderTargetHandle m_ColorTexture;
        static RenderTargetHandle m_DepthTexture;
        HybridPipelineData mCurrentPipelineData;
        public static readonly ProfilingSampler beginContextRendering = new ProfilingSampler("HybridPipeline.beginContextRendering");
        public static readonly ProfilingSampler endContextRendering = new ProfilingSampler("HybridPipeline.endContextRendering");
        public static readonly ProfilingSampler beginCameraRendering = new ProfilingSampler("HybridPipeline.beginCameraRendering");
        public static readonly ProfilingSampler endCameraRendering = new ProfilingSampler("HybridPipeline.endCameraRendering");
        public static readonly ProfilingSampler setupPerFrameShaderConstants = new ProfilingSampler("HybridPipeline.SetupPerFrameShaderConstants");
        public static int maxPerObjectLights
        {
            get => (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2) ? 4 : 8;
        }
        public HybridRenderPipeline()
        {
        }
        public void SetPipelineData(HybridPipelineData inPipelineData){
            mCurrentPipelineData=inPipelineData;
        }
        static void SetupPerFrameShaderConstants()
        {
            using var profScope = new ProfilingScope(null, setupPerFrameShaderConstants);

            // When glossy reflections are OFF in the shader we set a constant color to use as indirect specular
            SphericalHarmonicsL2 ambientSH = RenderSettings.ambientProbe;
            Color linearGlossyEnvColor = new Color(ambientSH[0, 0], ambientSH[1, 0], ambientSH[2, 0]) * RenderSettings.reflectionIntensity;
            Color glossyEnvColor = CoreUtils.ConvertLinearToActiveColorSpace(linearGlossyEnvColor);
            Shader.SetGlobalVector(ShaderPropertyId.glossyEnvironmentColor, glossyEnvColor);

            // Used as fallback cubemap for reflections
            Shader.SetGlobalVector(ShaderPropertyId.glossyEnvironmentCubeMapHDR, ReflectionProbe.defaultTextureHDRDecodeValues);
            Shader.SetGlobalTexture(ShaderPropertyId.glossyEnvironmentCubeMap, ReflectionProbe.defaultTexture);

            // Ambient
            Shader.SetGlobalVector(ShaderPropertyId.ambientSkyColor, CoreUtils.ConvertSRGBToActiveColorSpace(RenderSettings.ambientSkyColor));
            Shader.SetGlobalVector(ShaderPropertyId.ambientEquatorColor, CoreUtils.ConvertSRGBToActiveColorSpace(RenderSettings.ambientEquatorColor));
            Shader.SetGlobalVector(ShaderPropertyId.ambientGroundColor, CoreUtils.ConvertSRGBToActiveColorSpace(RenderSettings.ambientGroundColor));

            // Used when subtractive mode is selected
            Shader.SetGlobalVector(ShaderPropertyId.subtractiveShadowColor, CoreUtils.ConvertSRGBToActiveColorSpace(RenderSettings.subtractiveShadowColor));
        }
        bool NeedRenderCameraStack(Camera inCamera){
            return inCamera.cameraType==CameraType.Game||inCamera.cameraType==CameraType.VR;
        }
        Comparison<Camera> cameraSort=(inCamera1,inCamera2)=>{return (int)inCamera1.depth-(int)inCamera2.depth;};
        protected override void Render(ScriptableRenderContext inRenderContext, Camera[] inCameras)
        {
            List<Camera> cameraList=new List<Camera>(inCameras);
            using (new ProfilingScope(null, beginContextRendering))
            {
                BeginContextRendering(inRenderContext, cameraList);
            }
            SetupPerFrameShaderConstants();
            //sort camera
            if(cameraList.Count>1){
                cameraList.Sort(cameraSort);
            }
            for (int i = 0; i < cameraList.Count; ++i)
            {
                var camera = cameraList[i];
                if (NeedRenderCameraStack(camera))
                {
                    RenderCameraStack(inRenderContext, camera);
                }
                else
                {
                    using (new ProfilingScope(null, beginCameraRendering))
                    {
                        BeginCameraRendering(inRenderContext, camera);
                    }
                    RenderSingleCamera(inRenderContext, camera);
                    using (new ProfilingScope(null, endCameraRendering))
                    {
                        EndCameraRendering(inRenderContext, camera);
                    }
                }
            }
            using (new ProfilingScope(null, endContextRendering))
            {
                EndContextRendering(inRenderContext, cameraList);
            }
        }
        void SetRenderTarget(CommandBuffer inCommandBuffer,
            RenderTargetHandle inColorRT,RenderBufferLoadAction inColorRTLoadAction,RenderBufferStoreAction inColorStoreAction,
            RenderTargetHandle inDSRT,RenderBufferLoadAction inDSRTLoadAction,RenderBufferStoreAction inDSStoreAction){
            
        }
        static void RenderCameraStack(ScriptableRenderContext context, Camera inCamera)
        {
        }

        void RenderSingleCamera(ScriptableRenderContext context, Camera inCamera)
        {
            inCamera.TryGetCullingParameters(false, out ScriptableCullingParameters cullingParams);
            CommandBuffer cmd = CommandBufferPool.Get();
            UniversalAdditionalCameraData additionalCameraData = null;
            if (NeedRenderCameraStack(inCamera))
                inCamera.gameObject.TryGetComponent(out additionalCameraData);

            if (additionalCameraData != null && additionalCameraData.renderType != CameraRenderType.Base)
            {
                Debug.LogWarning("Only Base cameras can be rendered with standalone RenderSingleCamera. Camera will be skipped.");
                return;
            }
            UniversalRenderPipeline.InitializeCameraData(inCamera, additionalCameraData, true, out var cameraData);

        }
    }
}