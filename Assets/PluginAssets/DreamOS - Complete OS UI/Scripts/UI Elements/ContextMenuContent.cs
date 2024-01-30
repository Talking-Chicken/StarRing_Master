using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace Michsky.DreamOS
{
    [AddComponentMenu("DreamOS/UI Elements/Context Menu Content")]
    public class ContextMenuContent : MonoBehaviour, IPointerClickHandler
    {
        [Header("Resources")]
        public ContextMenuManager contextManager;
        public Transform itemParent;

        [Header("Items")]
        public List<MenuItem> menuItems = new List<MenuItem>();

        GameObject selectedItem;
        Image setItemImage;
        TextMeshProUGUI setItemText;
        Sprite imageHelper;

        [System.Serializable]
        public class MenuItem
        {
            public string itemText;
            public string localizationKey;
            public Sprite itemIcon;
            public ContextItemType contextItemType;
            public UnityEvent onClick;
        }

        public enum ContextItemType { Button, Separator }

        void Awake()
        {
            // Check if raycasting is available
            if (gameObject.GetComponent<Image>() == null)
            {
                Image raycastImg = gameObject.AddComponent<Image>();
                raycastImg.color = new Color(0, 0, 0, 0);
                raycastImg.raycastTarget = true;
            }
        }

        void Start()
        {
            if (contextManager == null)
            {
                try { contextManager = (ContextMenuManager)FindObjectsOfType(typeof(ContextMenuManager))[0]; }
                catch { Debug.Log("<b>[Context Menu]</b> Context Manager is missing.", this); return; }
            }

            itemParent = contextManager.contentRect.transform.Find("Item List").transform;
            foreach (Transform child in itemParent) { Destroy(child.gameObject); }
        }

        public void ProcessContent()
        {
            foreach (Transform child in itemParent) { Destroy(child.gameObject); }
            for (int i = 0; i < menuItems.Count; ++i)
            {
                bool skipProcess = false;

                if (menuItems[i].contextItemType == ContextItemType.Button && contextManager.buttonPreset != null) { selectedItem = contextManager.buttonPreset; }
                else if (menuItems[i].contextItemType == ContextItemType.Separator && contextManager.separatorPreset != null) { selectedItem = contextManager.separatorPreset; }
                else
                {
                    Debug.LogError("<b>[Context Menu]</b> At least one of the item presets is missing.", this);
                    skipProcess = true;
                }

                if (!skipProcess)
                {
                    GameObject go = Instantiate(selectedItem, new Vector3(0, 0, 0), Quaternion.identity);
                    go.transform.SetParent(itemParent, false);

                    if (menuItems[i].contextItemType == ContextItemType.Button)
                    {
                        setItemText = go.GetComponentInChildren<TextMeshProUGUI>();

                        // Check for localization
                        LocalizedObject tempLoc = setItemText.gameObject.GetComponent<LocalizedObject>();
                        if (string.IsNullOrEmpty(menuItems[i].localizationKey) || tempLoc == null || !tempLoc.CheckLocalizationStatus()) { setItemText.text = menuItems[i].itemText; }
                        else if (tempLoc != null) { setItemText.text = tempLoc.GetKeyOutput(menuItems[i].localizationKey); }

                        Transform goImage = go.gameObject.transform.Find("Icon");
                        setItemImage = goImage.GetComponent<Image>();
                        imageHelper = menuItems[i].itemIcon;
                        setItemImage.sprite = imageHelper;

                        if (imageHelper == null) { setItemImage.color = new Color(0, 0, 0, 0); }

                        ButtonManager itemButton = go.GetComponent<ButtonManager>();
                        itemButton.onClick.AddListener(menuItems[i].onClick.Invoke);
                        itemButton.onClick.AddListener(contextManager.Close);
                    }

                    StopCoroutine("ExecuteAfterTime");
                    StartCoroutine("ExecuteAfterTime", 0.01f);
                }
            }

            contextManager.SetContextMenuPosition();
            contextManager.Open();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (contextManager.isOn) { contextManager.Close(); }
            else if (eventData.button == PointerEventData.InputButton.Right && !contextManager.isOn) { ProcessContent(); }
        }

        IEnumerator ExecuteAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            itemParent.gameObject.SetActive(false);
            itemParent.gameObject.SetActive(true);
        }

        public void CreateNewButton(string title, Sprite icon)
        {
            MenuItem item = new MenuItem();
            item.itemText = title;
            item.itemIcon = icon;
            menuItems.Add(item);
        }
    }
}