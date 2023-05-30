using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

namespace Alice.Rendering
{
    public class NodeEditor : Node
    {
        public NodeEditor(float inX,float inY,float inWidth,float inHeight):base(inX,inY,inWidth,inHeight)
        {
        }
        public void OnEditName()
        {
            mIsEditingName = true;
            GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Keyboard);
            EditorGUI.FocusTextInControl(null);
        }
        public void OnFinishedEditName()
        {
            mIsEditingName = false;
            GUIUtility.hotControl = 0;
        }
    }
}
