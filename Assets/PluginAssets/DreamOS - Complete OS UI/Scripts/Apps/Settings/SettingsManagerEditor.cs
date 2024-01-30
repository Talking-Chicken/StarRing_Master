#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(SettingsManager))]
    public class SettingsManagerEditor : Editor
    {
        private SettingsManager settingsManager;
        private GUISkin customSkin;
        private int currentTab;

        private void OnEnable()
        {
            settingsManager = (SettingsManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = DreamOSEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = DreamOSEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            DreamOSEditorHandler.DrawComponentHeader(customSkin, "TopHeader_Settings");

            GUIContent[] toolbarTabs = new GUIContent[2];
            toolbarTabs[0] = new GUIContent("Resources");
            toolbarTabs[1] = new GUIContent("Settings");

            currentTab = DreamOSEditorHandler.DrawTabs(currentTab, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab_Resources")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab_Settings")))
                currentTab = 1;

            GUILayout.EndHorizontal();

            var UIManagerAsset = serializedObject.FindProperty("UIManagerAsset");
            var accentColorList = serializedObject.FindProperty("accentColorList");
            var accentReversedColorList = serializedObject.FindProperty("accentReversedColorList");
            var userManager = serializedObject.FindProperty("userManager");
            var desktopDragger = serializedObject.FindProperty("desktopDragger");
            var timeManager = serializedObject.FindProperty("timeManager");
            var speechRecognition = serializedObject.FindProperty("speechRecognition");
            var profilePictureWindow = serializedObject.FindProperty("profilePictureWindow");
            var resetPasswordWindow = serializedObject.FindProperty("resetPasswordWindow");
            var wipeUserDataWindow = serializedObject.FindProperty("wipeUserDataWindow");

            switch (currentTab)
            {
                case 0:
                    DreamOSEditorHandler.DrawHeader(customSkin, "Header_Resources", 6);
                    DreamOSEditorHandler.DrawPropertyCW(UIManagerAsset, customSkin, "UI Manager", 150);
                    DreamOSEditorHandler.DrawPropertyCW(accentColorList, customSkin, "Accent Color List", 150);
                    DreamOSEditorHandler.DrawPropertyCW(accentReversedColorList, customSkin, "Accent Color R List", 150);
                    DreamOSEditorHandler.DrawPropertyCW(userManager, customSkin, "User Manager", 150);
                    DreamOSEditorHandler.DrawPropertyCW(desktopDragger, customSkin, "Desktop Dragger", 150);
                    DreamOSEditorHandler.DrawPropertyCW(timeManager, customSkin, "Time Manager", 150);
                    DreamOSEditorHandler.DrawPropertyCW(speechRecognition, customSkin, "Speech Recognition", 150);
                    DreamOSEditorHandler.DrawPropertyCW(profilePictureWindow, customSkin, "Profile Picture Window", 150);
                    DreamOSEditorHandler.DrawPropertyCW(resetPasswordWindow, customSkin, "Reset Password Window", 150);
                    DreamOSEditorHandler.DrawPropertyCW(wipeUserDataWindow, customSkin, "Wipe User Data Window", 150);
                    break;

                case 1:
                    DreamOSEditorHandler.DrawHeader(customSkin, "Header_Settings", 6);
             //       lockDesktopItems.boolValue = DreamOSEditorHandler.DrawToggle(lockDesktopItems.boolValue, customSkin, "Lock Desktop Items");
                    break;
            }

            if (Application.isPlaying == false) { this.Repaint(); }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif