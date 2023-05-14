using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.InventoryEngine;
using UnityEditor;

[CustomEditor(typeof(InventorySlotPlus))]
public class InventorySlotPlusEditor : InventorySlotEditor
{
    protected SerializedProperty _quantityTextPro;
    protected SerializedProperty _itemNameTextPro;

    protected override void OnEnable()
    {
        base.OnEnable();

        _quantityTextPro = serializedObject.FindProperty("QuantityTextPro");
        _itemNameTextPro = serializedObject.FindProperty("ItemNameTextPro");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Extension", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_quantityTextPro);
        EditorGUILayout.PropertyField(_itemNameTextPro);

        serializedObject.ApplyModifiedProperties();
    }
}
