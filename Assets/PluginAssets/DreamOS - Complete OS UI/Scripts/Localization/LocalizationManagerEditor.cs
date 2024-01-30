#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using static Michsky.DreamOS.DateAndTimeManager;

namespace Michsky.DreamOS
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LocalizationManager))]
    public class LocalizationManagerEditor : Editor
    {
        private LocalizationManager lmTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            lmTarget = (LocalizationManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = DreamOSEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = DreamOSEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            var UIManagerAsset = serializedObject.FindProperty("UIManagerAsset");
            var languageSelectors = serializedObject.FindProperty("languageSelectors");

            var updateMode = serializedObject.FindProperty("updateMode");
            var setLanguageOnAwake = serializedObject.FindProperty("setLanguageOnAwake");
            var updateItemsOnSet = serializedObject.FindProperty("updateItemsOnSet");
            var saveLanguageChanges = serializedObject.FindProperty("saveLanguageChanges");

            var localizedItems = serializedObject.FindProperty("localizedItems");

            DreamOSEditorHandler.DrawHeader(customSkin, "Header_Resources", 6);
            DreamOSEditorHandler.DrawProperty(UIManagerAsset, customSkin, "UI Manager");
            GUILayout.BeginVertical();
            EditorGUI.indentLevel = 1;
            EditorGUILayout.PropertyField(languageSelectors, new GUIContent("Language Selectors"), true);
            EditorGUI.indentLevel = 0;
            GUILayout.EndVertical();

            DreamOSEditorHandler.DrawHeader(customSkin, "Header_Settings", 10);
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Update Mode"), customSkin.FindStyle("Text"), GUILayout.Width(120));
            EditorGUILayout.PropertyField(updateMode, new GUIContent(""));

            GUILayout.EndHorizontal();

            if (lmTarget.updateMode == LocalizationManager.UpdateMode.Realtime)
            {
                EditorGUILayout.HelpBox("Realtime mode finds all available localized objects at runtime. " +
                    "It is more dynamic, but drains more performance (only on awake). " +
                    "This is the default and recommended option.", MessageType.Info);
            }

            else
            {
                EditorGUILayout.HelpBox("Baked mode only affects localized objects that are available in the 'Localized Items' list. " +
                    "You can add localized objects manually or click the button below.", MessageType.Info);
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(localizedItems, new GUIContent("Localized Items"), true);
                EditorGUI.indentLevel = 0;
                if (GUILayout.Button("Find Localized Items", customSkin.button)) { FindLocalizedItems(); }
            }

            GUILayout.EndVertical();

            setLanguageOnAwake.boolValue = DreamOSEditorHandler.DrawToggle(setLanguageOnAwake.boolValue, customSkin, "Set Language On Awake");
            updateItemsOnSet.boolValue = DreamOSEditorHandler.DrawToggle(updateItemsOnSet.boolValue, customSkin, "Update Items On Language Set");
            saveLanguageChanges.boolValue = DreamOSEditorHandler.DrawToggle(saveLanguageChanges.boolValue, customSkin, "Save Language Changes");
            LocalizationManager.enableLogs = DreamOSEditorHandler.DrawToggle(LocalizationManager.enableLogs, customSkin, "Enable Logs");

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }

        void FindLocalizedItems()
        {
            lmTarget.localizedItems.Clear();
            foreach (LocalizedObject go in Resources.FindObjectsOfTypeAll(typeof(LocalizedObject)) as LocalizedObject[])
            {
                if (go.gameObject.scene.name != null) { lmTarget.localizedItems.Add(go); }
            }
        }
    }
}
#endif