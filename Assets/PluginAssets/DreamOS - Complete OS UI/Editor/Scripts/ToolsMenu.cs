#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Michsky.DreamOS
{
    public class ToolsMenu : Editor
    {
        static string objectPath;

        #region Methods & Helpers
        static void GetObjectPath()
        {
            objectPath = AssetDatabase.GetAssetPath(Resources.Load("UI Manager/DreamOS UI Manager"));
            objectPath = objectPath.Replace("Resources/UI Manager/DreamOS UI Manager.asset", "").Trim();
            objectPath = objectPath + "Prefabs/";
        }

        static void MakeSceneDirty(GameObject source, string sourceName)
        {
            if (Application.isPlaying == false)
            {
                Undo.RegisterCreatedObjectUndo(source, sourceName);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }

        static void SelectErrorDialog()
        {
            EditorUtility.DisplayDialog("DreamOS", "Cannot create the object due to missing manager file. " +
                    "Make sure you have a valid 'DreamOS UI Manager' file in DreamOS > Resources > UI Manager folder.", "Okay");
        }

        static void UpdateCustomEditorPath()
        {
            string darkPath = AssetDatabase.GetAssetPath(Resources.Load("DreamOS-EditorDark"));
            string lightPath = AssetDatabase.GetAssetPath(Resources.Load("DreamOS-EditorLight"));

            EditorPrefs.SetString("DreamOS.CustomEditorDark", darkPath);
            EditorPrefs.SetString("DreamOS.CustomEditorLight", lightPath);
        }
        #endregion

        #region Tools Menu
        [MenuItem("Tools/DreamOS/Create Overlay Resources", false, 12)]
        static void CreateOverlayResources()
        {
            try
            {
                GetObjectPath();
                GameObject clone = AssetDatabase.LoadAssetAtPath(objectPath + "Main Sources/DreamOS Canvas.prefab", typeof(GameObject)) as GameObject;
                PrefabUtility.InstantiatePrefab(clone);
                clone.name = clone.name.Replace("(Clone)", "").Trim();
                MakeSceneDirty(clone, clone.name);
            }

            catch { SelectErrorDialog(); }
        }

        [MenuItem("Tools/DreamOS/Create World Space Resources", false, 12)]
        static void CreateWorldSpaceResources()
        {
            try
            {
                GetObjectPath();
                GameObject clone = AssetDatabase.LoadAssetAtPath(objectPath + "Main Sources/World Space Resources.prefab", typeof(GameObject)) as GameObject;
                PrefabUtility.InstantiatePrefab(clone);
                clone.name = clone.name.Replace("(Clone)", "").Trim();
                MakeSceneDirty(clone, clone.name);
            }

            catch { SelectErrorDialog(); }
        }

        [MenuItem("Tools/DreamOS/Create Multi Instance Manager", false, 12)]
        static void CreateMultiInstanceManager()
        {
            GameObject tempObj = new GameObject("Multi Instance Manager");
            tempObj.AddComponent<MultiInstanceManager>();
            Selection.activeObject = tempObj;
            MakeSceneDirty(tempObj, tempObj.name);
        }

        [MenuItem("Tools/DreamOS/Open UI Manager", false, 24)]
        static void OpenUIManager()
        {
            Selection.activeObject = Resources.Load("UI Manager/DreamOS UI Manager");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'DreamOS UI Manager'. Make sure you have a valid 'UI Manager' asset in Resources/UI Manager folder. " +
                    "You can create a new UI Manager asset or re-import the pack if you can't see the file.");
        }

        [MenuItem("Tools/DreamOS/Select App Library")]
        static void SelectAppLibrary()
        {
            Selection.activeObject = Resources.Load("Apps/App Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'App Library'. Make sure you have 'App Library' asset in Resources/Apps folder.");
        }

        [MenuItem("Tools/DreamOS/Select Chat Library")]
        static void SelectChatList()
        {
            Selection.activeObject = Resources.Load("Chats/Example Chat");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Example Chat'. Make sure you have 'Example Chat' asset in Resources/Chats folder.");
        }

        [MenuItem("Tools/DreamOS/Select Icon Library")]
        static void SelectIconLibrary()
        {
            Selection.activeObject = Resources.Load("Icons/Icon Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Icon Library'. Make sure you have 'Library' asset in Resources/Icons folder.");
        }

        [MenuItem("Tools/DreamOS/Select Mail Library")]
        static void SelectMailLibrary()
        {
            Selection.activeObject = Resources.Load("Mail/Example Mail");
        }

        [MenuItem("Tools/DreamOS/Select Music Library")]
        static void SelectMusicLibrary()
        {
            Selection.activeObject = Resources.Load("Music Player/Music Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Music Library'. Make sure you have 'Library' asset in Resources/Music Player folder.");
        }

        [MenuItem("Tools/DreamOS/Select Profile Picture Library")]
        static void SelectPPLibrary()
        {
            Selection.activeObject = Resources.Load("Profile Pictures/Profile Picture Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Profile Picture Library'. Make sure you have 'Library' asset in Resources/Profile Pictures folder.");
        }

        [MenuItem("Tools/DreamOS/Select Web Library")]
        static void SelectWebLibrary()
        {
            Selection.activeObject = Resources.Load("Web Browser/Web Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Web Library'. Make sure you have 'Library' asset in Resources/Web Browser folder.");
        }

        [MenuItem("Tools/DreamOS/Select Wallpaper Library")]
        static void SelectWPLibrary()
        {
            Selection.activeObject = Resources.Load("Wallpapers/Wallpaper Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Wallpaper Library'. Make sure you have 'Wallpaper Library' asset in Resources/Wallpapers folder.");
        }
        #endregion

        #region Object Creating
        static void CreateObject(string resourcePath)
        {
            try
            {
                GetObjectPath();
                UpdateCustomEditorPath();

                GameObject clone = Instantiate(AssetDatabase.LoadAssetAtPath(objectPath + resourcePath + ".prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;

                try
                {
                    if (Selection.activeGameObject == null)
                    {
                        var canvas = (Canvas)GameObject.FindObjectsOfType(typeof(Canvas))[0];
                        clone.transform.SetParent(canvas.transform, false);
                    }

                    else { clone.transform.SetParent(Selection.activeGameObject.transform, false); }

                    clone.name = clone.name.Replace("(Clone)", "").Trim();
                    MakeSceneDirty(clone, clone.name);
                }

                catch
                {
                    CreateCanvas();
                    var canvas = (Canvas)GameObject.FindObjectsOfType(typeof(Canvas))[0];
                    clone.transform.SetParent(canvas.transform, false);
                    clone.name = clone.name.Replace("(Clone)", "").Trim();
                    MakeSceneDirty(clone, clone.name);
                }

                Selection.activeObject = clone;
            }

            catch { SelectErrorDialog(); }
        }

        [MenuItem("GameObject/DreamOS/Canvas", false, 8)]
        static void CreateCanvas()
        {
            try
            {
                GetObjectPath();
                UpdateCustomEditorPath();

                GameObject clone = Instantiate(AssetDatabase.LoadAssetAtPath(objectPath + "UI Elements/Other/Canvas.prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                clone.name = clone.name.Replace("(Clone)", "").Trim();
                Selection.activeObject = clone;
              
                MakeSceneDirty(clone, clone.name);
            }

            catch { SelectErrorDialog(); }
        }
        #endregion

        #region Button
        [MenuItem("GameObject/DreamOS/Button/Default Button", false, 8)]
        static void DefaultButton()
        {
            CreateObject("UI Elements/Button/Default Button");
        }

        [MenuItem("GameObject/DreamOS/Button/Desktop Button", false, 8)]
        static void DesktopButton()
        {
            CreateObject("UI Elements/Button/Desktop Button");
        }

        [MenuItem("GameObject/DreamOS/Button/Navbar Button", false, 8)]
        static void NavbarButton()
        {
            CreateObject("UI Elements/Button/Navbar Button");
        }

        [MenuItem("GameObject/DreamOS/Button/Taskbar Button", false, 8)]
        static void TaskbarButton()
        {
            CreateObject("UI Elements/Button/Taskbar Button");
        }
        #endregion

        #region Input Field
        [MenuItem("GameObject/DreamOS/Input Field/Input Field (Standard)", false, 8)]
        static void StandardInputField()
        {
            CreateObject("UI Elements/Input Field/Input Field");
        }

        [MenuItem("GameObject/DreamOS/Input Field/Input Field (Alternative)", false, 8)]
        static void AltInputField()
        {
            CreateObject("UI Elements/Input Field/Input Field Alt");
        }
        #endregion

        #region Modal Window
        [MenuItem("GameObject/DreamOS/Modal Window/Standard", false, 8)]
        static void ModalWindow()
        {
            CreateObject("UI Elements/Modal Window/Standard Modal Window");
        }
        #endregion

        #region Scrollbar
        [MenuItem("GameObject/DreamOS/Scrollbar/Standard", false, 8)]
        static void Scrollbar()
        {
            CreateObject("UI Elements/Scrollbar/Scrollbar");
        }
        #endregion

        #region Selectors
        [MenuItem("GameObject/DreamOS/Selectors/Horizontal Selector", false, 8)]
        static void HorizontalSelector()
        {
            CreateObject("UI Elements/Selectors/Horizontal Selector");
        }

        [MenuItem("GameObject/DreamOS/Selectors/Vertical Selector", false, 8)]
        static void VerticalSelector()
        {
            CreateObject("UI Elements/Selectors/Vertical Selector");
        }
        #endregion

        #region Slider
        [MenuItem("GameObject/DreamOS/Slider/Standard", false, 8)]
        static void Slider()
        {
            CreateObject("UI Elements/Slider/Slider");
        }
        #endregion

        #region Spinners
        [MenuItem("GameObject/DreamOS/Spinners/Default Spinner", false, 8)]
        static void LoaderMaterial()
        {
            CreateObject("UI Elements/Spinner/Default Spinner");
        }
        #endregion

        #region Switch
        [MenuItem("GameObject/DreamOS/Switch/Standard", false, 8)]
        static void Switch()
        {
            CreateObject("UI Elements/Switch/Switch");
        }
        #endregion

        #region UIM
        [MenuItem("GameObject/DreamOS/UI Manager/Image", false, 8)]
        static void UIMImage()
        {
            CreateObject("UI Elements/UIM/Image");
        }

        [MenuItem("GameObject/DreamOS/UI Manager/Text (TMP)", false, 8)]
        static void UIMText()
        {
            CreateObject("UI Elements/UIM/Text (TMP)");
        }
        #endregion
    }
}
#endif