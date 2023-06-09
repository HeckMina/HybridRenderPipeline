using UnityEditor;
using UnityEngine;

namespace Alice.Rendering.Hybrid{
    [CustomEditor(typeof(HybridPipelineData))]
    public class PipelineDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("编辑渲染管线"))
            {
                EditPipelineData();
            }
        }

        private void EditPipelineData()
        {
            HybridPipelineData pipelineDataAsset = target as HybridPipelineData;
            RPEditor window = EditorWindow.GetWindow<RPEditor>();
            window.SetPipelineData(pipelineDataAsset);
            window.titleContent = new GUIContent("渲染管线编辑器");
            window.minSize = new Vector2(800,600);
            window.Show();
        }
    }
}