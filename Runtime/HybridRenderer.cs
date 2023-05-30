using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

namespace Alice.Rendering.Hybrid
{
    public class HybridRenderer : IDisposable
    {
        public void Dispose(){
            GC.SuppressFinalize(this);
        }
    }
}
