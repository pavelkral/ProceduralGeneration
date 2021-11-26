using UnityEngine;
using UnityEditor;

using System.Collections;

namespace ProceduralGeometry
{

    public class SaveMeshUtil : EditorWindow {

        string folder = "Assets/ProceduralGeometry/Meshes/";
        string assetName = "GeneratedMesh";
        MeshFilter filter;

        [MenuItem("ProceduralGeometry/SaveMesh")]
        static void Save () {
            SaveMeshUtil window = EditorWindow.GetWindow(typeof(SaveMeshUtil)) as SaveMeshUtil;
            window.Show();
        }

        void OnGUI () {
            GUILayout.Label("Save Mesh", EditorStyles.boldLabel);

            using (new GUILayout.HorizontalScope()) {
                GUILayout.Label("File name");
                assetName = GUILayout.TextField(assetName, GUILayout.Width(200f));
            }

            using (new GUILayout.HorizontalScope()) {
                GUILayout.Label("Target MeshFilter in Scene");
                filter = UnityEditor.EditorGUILayout.ObjectField(filter, typeof(MeshFilter), true, GUILayout.Width(200f)) as MeshFilter;
            }

            if (assetName.Length > 0 && filter != null && GUILayout.Button("Save")) {
                AssetDatabase.CreateAsset(filter.mesh, folder + assetName + ".asset");
                AssetDatabase.SaveAssets();
            }
        }

    }

}


