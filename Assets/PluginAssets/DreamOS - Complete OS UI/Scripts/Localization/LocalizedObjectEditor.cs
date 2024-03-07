#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(LocalizedObject))]
    public class LocalizedObjectEditor : Editor
    {
        private LocalizedObject loTarget;
        private GUISkin customSkin;
        private int currentTab;

        private List<string> tableList = new List<string>();
        private string searchString;
        private string tempValue;
        Vector2 scrollPosition = Vector2.zero;

        private void OnEnable()
        {
            loTarget = (LocalizedObject)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = DreamOSEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = DreamOSEditorHandler.GetLightEditor(customSkin); }

            if (loTarget.LocalizationSettings == null) 
            {
                try { loTarget.localizationManager = (LocalizationManager)GameObject.FindObjectsOfType(typeof(LocalizationManager))[0]; } catch { }
                if (loTarget.localizationManager != null && loTarget.localizationManager.UIManagerAsset != null && loTarget.localizationManager.UIManagerAsset.localizationSettings != null)
                {
                    loTarget.LocalizationSettings = loTarget.localizationManager.UIManagerAsset.localizationSettings;
                }
            }

            // Update language Tab_Settings if it's driven by the manager
            else if (loTarget.localizationManager != null && loTarget.localizationManager.UIManagerAsset != null && loTarget.localizationManager.UIManagerAsset.localizationSettings != null)
            {
                loTarget.LocalizationSettings = loTarget.localizationManager.UIManagerAsset.localizationSettings;
            }

            RefreshTableDropdown();
        }

        public override void OnInspectorGUI()
        {
            DreamOSEditorHandler.DrawComponentHeader(customSkin, "TopHeader_Localization");

            GUIContent[] toolbarTabs = new GUIContent[2];
            toolbarTabs[0] = new GUIContent("Content");
            toolbarTabs[1] = new GUIContent("Settings");

            currentTab = DreamOSEditorHandler.DrawTabs(currentTab, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab_Content")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab_Settings")))
                currentTab = 1;

            GUILayout.EndHorizontal();

            var localizationManager = serializedObject.FindProperty("localizationManager");
            var LocalizationSettings = serializedObject.FindProperty("LocalizationSettings");
            var tableIndex = serializedObject.FindProperty("tableIndex");
            var objectType = serializedObject.FindProperty("objectType");
            var onLanguageChanged = serializedObject.FindProperty("onLanguageChanged");
            var rebuildLayoutOnUpdate = serializedObject.FindProperty("rebuildLayoutOnUpdate");
            var forceAddToManager = serializedObject.FindProperty("forceAddToManager");
            var updateMode = serializedObject.FindProperty("updateMode");
            var textObj = serializedObject.FindProperty("textObj");
            var localizationKey = serializedObject.FindProperty("localizationKey");

            if (loTarget.localizationManager != null && loTarget.localizationManager.UIManagerAsset != null && loTarget.localizationManager.UIManagerAsset.enableLocalization == false)
            {
                EditorGUILayout.HelpBox("Localization is disabled.", MessageType.Warning);
                return;
            }

            switch (currentTab)
            {
                case 0:
                    DreamOSEditorHandler.DrawHeader(customSkin, "Header_Content", 6);
                    DreamOSEditorHandler.DrawProperty(localizationManager, customSkin, "Manager");
                    if (loTarget.localizationManager != null) { GUI.enabled = false; }
                    DreamOSEditorHandler.DrawProperty(LocalizationSettings, customSkin, "Settings");
                    GUI.enabled = true;

                    if (loTarget.LocalizationSettings == null)
                    {
                        EditorGUILayout.HelpBox("LocalizationSettings is missing. You can assign a valid 'Localization Manager' " +
                            "or directly assign a localization Tab_Settings asset.", MessageType.Warning);
                        serializedObject.ApplyModifiedProperties();
                        return;
                    }

                    DreamOSEditorHandler.DrawProperty(updateMode, customSkin, "Update Mode");

                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(new GUIContent("Object Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(objectType, new GUIContent(""));
                    GUILayout.EndHorizontal();
                   
                    if (loTarget.objectType == LocalizedObject.ObjectType.TextMeshPro) 
                    {
                        DreamOSEditorHandler.DrawProperty(textObj, customSkin, "Text Object");
                        if (Application.isPlaying == false
                            && loTarget.showOutputOnEditor == true
                            && string.IsNullOrEmpty(tempValue) == false
                            && loTarget.textObj != null 
                            && GUILayout.Button(new GUIContent("Update Text"), customSkin.button)) 
                        { 
                            loTarget.textObj.text = tempValue;
                            loTarget.textObj.enabled = false;
                            loTarget.textObj.enabled = true;
                        }
                    }

                    GUILayout.EndVertical();

                    if (loTarget.LocalizationSettings.tables.Count != 0 && loTarget.tableIndex != -1)
                    {
                        DreamOSEditorHandler.DrawHeader(customSkin, "Header_Tables", 10);

                        // Selected table
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(new GUIContent("Selected Table"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        tableIndex.intValue = EditorGUILayout.Popup(loTarget.tableIndex, tableList.ToArray());
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button(new GUIContent("Edit Table"), customSkin.button))
                        {
                            LocalizationTableWindow.ShowWindow(loTarget.LocalizationSettings, loTarget.LocalizationSettings.tables[loTarget.tableIndex].localizationTable, loTarget.tableIndex);
                        }

                        if (loTarget.objectType != LocalizedObject.ObjectType.ComponentDriven)
                        {
                            GUILayout.BeginHorizontal(EditorStyles.helpBox);
                            EditorGUILayout.LabelField(new GUIContent("Localization Key"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                            EditorGUILayout.PropertyField(localizationKey, new GUIContent(""));
                            loTarget.showOutputOnEditor = GUILayout.Toggle(loTarget.showOutputOnEditor, new GUIContent("", "See output"), GUILayout.Width(15), GUILayout.Height(18));
                            GUILayout.EndHorizontal();

                            // Search for keys
                            GUILayout.BeginVertical(EditorStyles.helpBox);
                            EditorGUILayout.LabelField(new GUIContent("Search for keys in " + loTarget.LocalizationSettings.tables[loTarget.tableIndex].tableID), customSkin.FindStyle("Text"));

                            GUILayout.BeginHorizontal();
#if UNITY_2022_3_OR_NEWER && !UNITY_2022_3_0
                            searchString = GUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSearchTextField"));
#else
                            searchString = GUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));
#endif
#if UNITY_2022_3_OR_NEWER && !UNITY_2022_3_0
                            if (!string.IsNullOrEmpty(searchString) && GUILayout.Button(new GUIContent("", "Clear search bar"), GUI.skin.FindStyle("ToolbarSearchCancelButton"))) { searchString = ""; GUI.FocusControl(null); }
#else
                            if (!string.IsNullOrEmpty(searchString) && GUILayout.Button(new GUIContent("", "Clear search bar"), GUI.skin.FindStyle("ToolbarSeachCancelButton"))) { searchString = ""; GUI.FocusControl(null); }
#endif   
                            GUILayout.EndHorizontal();

                            if (!string.IsNullOrEmpty(searchString))
                            {
                                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.Height(132));
                                GUILayout.BeginVertical();

                                for (int i = 0; i < loTarget.LocalizationSettings.languages[0].localizationLanguage.tableList[loTarget.tableIndex].tableContent.Count; i++)
                                {
                                    if (loTarget.LocalizationSettings.languages[0].localizationLanguage.tableList[loTarget.tableIndex].tableContent[i].key.ToLower().Contains(searchString.ToLower()))
                                    {
                                        if (GUILayout.Button(new GUIContent(loTarget.LocalizationSettings.languages[0].localizationLanguage.tableList[loTarget.tableIndex].tableContent[i].key), customSkin.button))
                                        {
                                            loTarget.localizationKey = loTarget.LocalizationSettings.languages[0].localizationLanguage.tableList[loTarget.tableIndex].tableContent[i].key;
                                            searchString = "";
                                            GUI.FocusControl(null);
                                            EditorUtility.SetDirty(loTarget);
                                        }
                                    }
                                }

                                GUILayout.EndVertical();
                                GUILayout.EndScrollView();
                            }

                            GUILayout.EndVertical();

                            if (loTarget.showOutputOnEditor == true)
                            {
                                GUI.enabled = false;
                                for (int i = 0; i < loTarget.LocalizationSettings.languages.Count; i++)
                                {
                                    for (int x = 0; x < loTarget.LocalizationSettings.languages[i].localizationLanguage.tableList[loTarget.tableIndex].tableContent.Count; x++)
                                    {
                                        if (loTarget.LocalizationSettings.languages[i].localizationLanguage.tableList[loTarget.tableIndex].tableContent[x].key == loTarget.localizationKey)
                                        {
                                            GUILayout.BeginHorizontal();
                                            EditorGUILayout.LabelField(new GUIContent("[" + loTarget.LocalizationSettings.languages[i].languageID + "] " +
                                                loTarget.LocalizationSettings.languages[i].localizationLanguage.tableList[loTarget.tableIndex].tableContent[x].value), customSkin.FindStyle("Text"));
                                            GUILayout.EndHorizontal();

                                            // Used for Update Text button
                                            tempValue = loTarget.LocalizationSettings.languages[loTarget.LocalizationSettings.defaultLanguageIndex].localizationLanguage.tableList[loTarget.tableIndex].tableContent[x].value;
                                        }
                                    }
                                }
                                GUI.enabled = true;
                            }
                        }

                        GUILayout.EndVertical();
                    }
                    else if (loTarget.LocalizationSettings.tables.Count != 0 && loTarget.tableIndex == -1) { RefreshTableDropdown(); }

                    DreamOSEditorHandler.DrawHeader(customSkin, "Header_Events", 10);
                    EditorGUILayout.PropertyField(onLanguageChanged, new GUIContent("On Language Changed"), true);
                    break;

                case 1:
                    DreamOSEditorHandler.DrawHeader(customSkin, "Header_Settings", 6);
                    rebuildLayoutOnUpdate.boolValue = DreamOSEditorHandler.DrawToggle(rebuildLayoutOnUpdate.boolValue, customSkin, "Rebuild Layout On Update", "Force to rebuild layout on item update to prevent visual glitches.");
                    forceAddToManager.boolValue = DreamOSEditorHandler.DrawToggle(forceAddToManager.boolValue, customSkin, "Force Add To Manager", "Force to add this component to the manager on awake.");
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }

        private void RefreshTableDropdown()
        {
            if (loTarget.LocalizationSettings == null)
                return;

            for (int i = 0; i < loTarget.LocalizationSettings.tables.Count; i++)
            {
                if (loTarget.LocalizationSettings.tables[i].localizationTable != null)
                {
                    tableList.Add(loTarget.LocalizationSettings.tables[i].tableID);
                }
            }

            if (loTarget.LocalizationSettings.tables.Count == 0) { loTarget.tableIndex = -1; }
            else if (loTarget.tableIndex > loTarget.LocalizationSettings.tables.Count - 1) { loTarget.tableIndex = 0; }
            else if (loTarget.tableIndex == -1 && loTarget.LocalizationSettings.tables.Count != 0) { loTarget.tableIndex = 0; }
        }
    }
}
#endif