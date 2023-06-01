using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Alice.Rendering
{
    public class RTNode : Node
    {
        public UnityEngine.RenderTextureFormat mFormat;
        public RTNode(float inX, float inY, float inWidth, float inHeight):base(inX,inY,inWidth,inHeight)
        {
            mName="RenderTarget";
        }
    }
}
