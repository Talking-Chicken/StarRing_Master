using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NorthLab
{

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(VolumetricBaker))]
    public class VolumetricBakerEditor : Editor
    {

        private VolumetricBaker script;

        private void OnEnable()
        {
            script = (VolumetricBaker)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Bake"))
            {
                if (EditorUtility.DisplayDialog("Bake confirmation", "This action will delete the Volumetric Probe component and its windows. Are you sure?", "Yes", "No"))
                    script.Bake();
            }
        }

    }
#endif

    /// <summary>
    /// Bakes VolumetricProbe mesh into the file.
    /// </summary>
    public class VolumetricBaker : MonoBehaviour
    {

        [SerializeField]
        private VolumetricProbe targetProbe = null;

#if UNITY_EDITOR
        private static string GetSceneFolderPath(out string sceneName)
        {
            UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            sceneName = scene.name;
            return scene.path.Substring(0, scene.path.Length - 6) + "/";
        }

        private string GetPath()
        {
            string path = GetSceneFolderPath(out string sceneName);

            if (!AssetDatabase.IsValidFolder(path.Substring(0, path.Length - 1)))
            {
                string parentFolder = path.Substring(0, path.Length - 2 - sceneName.Length);
                if (string.IsNullOrEmpty(AssetDatabase.CreateFolder(parentFolder, sceneName)))
                    return string.Empty;
            }

            path += targetProbe.name;

            int index = 1;
            string checkPath = path + $"_{index}.asset";
            while (AssetDatabase.AssetPathToGUID(checkPath) != string.Empty)
            {
                index++;
                checkPath = path + $"_{index}.asset";
            }
            return checkPath;
        }

        public void Bake()
        {
            if (targetProbe == null)
                return;

            MeshFilter mf = targetProbe.GetComponent<MeshFilter>();

            if (mf == null)
                return;

            string path = GetPath();
            if (string.IsNullOrEmpty(path))
            {
                throw new System.Exception($"Cannot find or create a folder of the current active scene \"{GetSceneFolderPath(out string _)}\"");
            }
            AssetDatabase.CreateAsset(mf.sharedMesh, path);
            AssetDatabase.SaveAssets();

            Undo.DestroyObjectImmediate(targetProbe);
            for (int i = targetProbe.Windows.Length - 1; i >= 0; i--)
            {
                Undo.DestroyObjectImmediate(targetProbe.Windows[i].gameObject);
            }
            targetProbe = null;

            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(path);
        }
#endif

    }

}