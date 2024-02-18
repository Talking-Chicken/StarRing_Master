using System.Collections.Generic;
using UnityEngine;

namespace Michsky.DreamOS
{
    [DisallowMultipleComponent]
    public class LocalizationManager : MonoBehaviour
    {
        // Resources
        public UIManager UIManagerAsset;
        public List<HorizontalSelector> languageSelectors = new List<HorizontalSelector>();

        // Tab_Settings
        public UpdateMode updateMode;
        public bool setLanguageOnAwake = true;
        public bool updateItemsOnSet = true;
        public bool saveLanguageChanges = true;
        public static bool enableLogs = true;

        // Helpers
        public string currentLanguage;
        public LocalizationLanguage currentLanguageAsset;
        public List<LocalizedObject> localizedItems = new List<LocalizedObject>();
        DreamOSDataManager.DataCategory dataCat = DreamOSDataManager.DataCategory.System;

        public enum UpdateMode { Realtime, Baked }

        void Awake()
        {
            if (UIManagerAsset == null) { UIManagerAsset = (UIManager)Resources.FindObjectsOfTypeAll(typeof(UIManager))[0]; }
            if (UIManagerAsset == null || UIManagerAsset.enableLocalization == false) { return; }
            if (updateMode == UpdateMode.Realtime)
            {
                localizedItems.Clear();
                foreach (LocalizedObject go in Resources.FindObjectsOfTypeAll(typeof(LocalizedObject)) as LocalizedObject[])
                {
                    if (go.gameObject.scene.name != null) { localizedItems.Add(go); }
                }
            }

            if (setLanguageOnAwake == true) 
            {
                if (DreamOSDataManager.ContainsJsonKey(dataCat, "Language")) { currentLanguage = DreamOSDataManager.ReadStringData(dataCat, "Language"); }
                else { currentLanguage = UIManagerAsset.localizationSettings.defaultLanguageID; }

                SetLanguage(currentLanguage);
            }

            // Populate language selectors
            foreach (HorizontalSelector hs in languageSelectors)
            {
                if (hs == null)
                    continue;

                hs.items.Clear();

                for (int i = 0; i < UIManagerAsset.localizationSettings.languages.Count; i++)
                {
                    hs.CreateNewItem(UIManagerAsset.localizationSettings.languages[i].localizedName);

                    string tempID = UIManagerAsset.localizationSettings.languages[i].languageID;
                    hs.items[i].onItemSelect.AddListener(() => SetLanguage(tempID));

                    if (UIManagerAsset.localizationSettings.languages[i].localizationLanguage == currentLanguageAsset)
                    {
                        hs.index = i;
                        hs.defaultIndex = i;
                    }
                }

                hs.UpdateUI();
            }       
        }

        public void SetLanguageByIndex(int index)
        {
            SetLanguage(UIManagerAsset.localizationSettings.languages[index].languageID);
        }

        public void SetLanguage(string langID)
        {
            if (UIManagerAsset == null || UIManagerAsset.enableLocalization == false)
            {
                UIManager.isLocalizationEnabled = false;
                return;
            }

            currentLanguageAsset = null;

            for (int i = 0; i < UIManagerAsset.localizationSettings.languages.Count; i++)
            {
                if (UIManagerAsset.localizationSettings.languages[i].languageID == langID) { currentLanguageAsset = UIManagerAsset.localizationSettings.languages[i].localizationLanguage; break; }
                else if (UIManagerAsset.localizationSettings.languages[i].languageName + " (" + UIManagerAsset.localizationSettings.languages[i].languageID + ")" == langID) { currentLanguageAsset = UIManagerAsset.localizationSettings.languages[i].localizationLanguage; break; }
                else if (UIManagerAsset.localizationSettings.languages[i].languageName == langID + ")") { currentLanguageAsset = UIManagerAsset.localizationSettings.languages[i].localizationLanguage;break; }
            }

            if (currentLanguageAsset == null) { Debug.Log("<b>[Localization Manager]</b> No language named <b>" + langID + "</b> found.", this); return; }
            else { currentLanguage = currentLanguageAsset.languageName + " (" + currentLanguageAsset.languageID + ")"; }
            
            if (updateItemsOnSet == true)
            {
                for (int i = 0; i < localizedItems.Count; i++)
                {
                    if (localizedItems[i] == null) { localizedItems.RemoveAt(i); }
                    else if (localizedItems[i].gameObject.activeInHierarchy && localizedItems[i].updateMode != LocalizedObject.UpdateMode.OnDemand) { localizedItems[i].UpdateItem(); }
                }
            }

            if (saveLanguageChanges == true)
            {
                DreamOSDataManager.WriteStringData(dataCat, "Language", currentLanguageAsset.languageID);
            }

            UIManagerAsset.currentLanguage = currentLanguageAsset;
            UIManager.isLocalizationEnabled = true;
        }

        public static void SetLanguageWithoutNotify(string langID)
        {
            DreamOSDataManager.WriteStringData(DreamOSDataManager.DataCategory.System, "Language", langID);
        }
    }
}