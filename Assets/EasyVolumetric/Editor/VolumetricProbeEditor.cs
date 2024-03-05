using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace NorthLab
{
    /// <summary>
    /// Custom editor class for the VolumetricProbe.
    /// </summary>
    [CustomEditor(typeof(VolumetricProbe))]
    public class VolumetricProbeEditor : Editor
    {

        private SerializedProperty islands;
        private SerializedProperty windows;

        private SerializedProperty lightSource;
        private SerializedProperty overrideTransform;
        private SerializedProperty overrideIntensity;
        private SerializedProperty overrideColor;
        private SerializedProperty isStatic;
        private SerializedProperty forceUpdate;

        private SerializedProperty usePhysics;
        private SerializedProperty rayDist;
        private SerializedProperty layerMask;

        private SerializedProperty material;
        private SerializedProperty useLightColor;
        private SerializedProperty useLightIntensity;
        private SerializedProperty lightIntensityMultiplier;
        private SerializedProperty lightIntensityClamp;
        private SerializedProperty startColor;
        private SerializedProperty endColor;
        private SerializedProperty particles;

        private VolumetricProbe script;
        private bool islandsFoldOut;
        private List<bool> islandFoldOut;
        private VolumetricWindow.WindowTypes windowType;
        private Mesh windowMesh;
        private MeshFilter windowMeshFilter;

        private void OnEnable()
        {
            islands = serializedObject.FindProperty("islands");
            windows = serializedObject.FindProperty("windows");

            lightSource = serializedObject.FindProperty("lightSource");
            overrideTransform = serializedObject.FindProperty("overrideTransform");
            overrideIntensity = serializedObject.FindProperty("overrideIntensity");
            overrideColor = serializedObject.FindProperty("overrideColor");
            isStatic = serializedObject.FindProperty("isStatic");
            forceUpdate = serializedObject.FindProperty("forceUpdate");

            usePhysics = serializedObject.FindProperty("usePhysics");
            rayDist = serializedObject.FindProperty("rayDist");
            layerMask = serializedObject.FindProperty("layerMask");

            material = serializedObject.FindProperty("material");
            useLightColor = serializedObject.FindProperty("useLightColor");
            useLightIntensity = serializedObject.FindProperty("useLightIntensity");
            lightIntensityMultiplier = serializedObject.FindProperty("lightIntensityMultiplier");
            lightIntensityClamp = serializedObject.FindProperty("lightIntensityClamp");
            startColor = serializedObject.FindProperty("startColor");
            endColor = serializedObject.FindProperty("endColor");
            particles = serializedObject.FindProperty("particles");

            islandFoldOut = new List<bool>();
            for (int i = 0; i < islands.arraySize; i++)
                islandFoldOut.Add(true);

            script = (VolumetricProbe)target;
        }

        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //foldoutstyle
            GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.fontStyle = FontStyle.Bold;

            #region Obsolete
            if (islands.arraySize > 0)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.indentLevel++;
                islandsFoldOut = EditorGUILayout.Foldout(islandsFoldOut, "Islands", foldoutStyle);
                EditorGUI.indentLevel--;

                if (islandsFoldOut)
                {
                    float oldWidth;
                    for (int i = 0; i < islands.arraySize; i++)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.BeginHorizontal();
                        EditorGUI.indentLevel++;
                        islandFoldOut[i] = EditorGUILayout.Foldout(islandFoldOut[i], "Island " + i, foldoutStyle);
                        if (GUILayout.Button("Delete island"))
                        {
                            islands.DeleteArrayElementAtIndex(i);
                            break;
                        }
                        EditorGUI.indentLevel--;
                        EditorGUILayout.EndHorizontal();

                        oldWidth = EditorGUIUtility.labelWidth;
                        EditorGUIUtility.labelWidth = 80;
                        if (islandFoldOut[i])
                        {
                            EditorGUILayout.Space();
                            EditorGUI.indentLevel++;
                            int arraySize = islands.GetArrayElementAtIndex(i).FindPropertyRelative("vertices").arraySize;
                            for (int j = 0; j < arraySize; j++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.PropertyField(islands.GetArrayElementAtIndex(i).FindPropertyRelative("vertices").GetArrayElementAtIndex(j), new GUIContent("Vertex " + j));
                                GUI.enabled = j > 0;
                                if (GUILayout.Button("▲", GUILayout.MaxWidth(20)))
                                {
                                    script.Islands[i].Swap(j, j - 1);
                                }
                                GUI.enabled = j < arraySize - 1;
                                if (GUILayout.Button("▼", GUILayout.MaxWidth(20)))
                                {
                                    script.Islands[i].Swap(j, j + 1);
                                }
                                GUI.enabled = true;
                                if (GUILayout.Button("X", GUILayout.MaxWidth(20)))
                                {
                                    islands.GetArrayElementAtIndex(i).FindPropertyRelative("vertices").DeleteArrayElementAtIndex(j);
                                    break;
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                            if (GUILayout.Button("Add vertex"))
                            {
                                islands.GetArrayElementAtIndex(i).FindPropertyRelative("vertices").InsertArrayElementAtIndex(islands.GetArrayElementAtIndex(i).FindPropertyRelative("vertices").arraySize);
                            }
                            EditorGUI.indentLevel--;
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUIUtility.labelWidth = oldWidth;
                    }

                    oldWidth = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = 80;
                    EditorGUILayout.Space();
                    EditorGUIUtility.labelWidth = oldWidth;
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.HelpBox("Islands are obsolete. A new replacement for them is the VolumetricWindow. You still can see them in the inspector but they are not used.", MessageType.Warning);
                if (GUILayout.Button("Convert islands to windows"))
                {
                    script.ConvertIslandsToWindows();
                }
                EditorGUILayout.Space();
            }
            #endregion

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(windows, true);
            if (windows.isExpanded)
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Add Window", GUILayout.MaxWidth(128)))
                {
                    AddWindow();
                }
                windowType = (VolumetricWindow.WindowTypes)EditorGUILayout.EnumPopup("", windowType, GUILayout.MaxWidth(150));
                if (windowType == VolumetricWindow.WindowTypes.Mesh)
                {
                    windowMesh = (Mesh)EditorGUILayout.ObjectField(windowMesh, typeof(Mesh), true, GUILayout.MaxWidth(170));
                }
                else if (windowType == VolumetricWindow.WindowTypes.MeshFilter)
                {
                    windowMeshFilter = (MeshFilter)EditorGUILayout.ObjectField(windowMeshFilter, typeof(MeshFilter), true, GUILayout.MaxWidth(170));
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            EditorGUILayout.PropertyField(lightSource);
            EditorGUILayout.PropertyField(overrideTransform);
            if (overrideTransform.objectReferenceValue != null)
            {
                if (lightSource.objectReferenceValue != null)
                    EditorGUILayout.HelpBox("The lightSource have more priority over the point!", MessageType.Info);

                EditorGUILayout.PropertyField(overrideIntensity);
                EditorGUILayout.PropertyField(overrideColor);
            }
            EditorGUILayout.PropertyField(isStatic);
            if (!isStatic.boolValue)
                EditorGUILayout.PropertyField(forceUpdate);

            EditorGUILayout.PropertyField(usePhysics);
            EditorGUILayout.PropertyField(rayDist);
            EditorGUILayout.PropertyField(layerMask);

            EditorGUILayout.PropertyField(material);
            EditorGUILayout.PropertyField(useLightColor);
            EditorGUILayout.PropertyField(useLightIntensity);
            if (useLightIntensity.boolValue)
            {
                EditorGUILayout.PropertyField(lightIntensityMultiplier);
                EditorGUILayout.PropertyField(lightIntensityClamp);
            }
            EditorGUILayout.PropertyField(startColor);
            EditorGUILayout.PropertyField(endColor);

            EditorGUILayout.PropertyField(particles);
            if (particles.objectReferenceValue)
            {
                Component p_component = particles.objectReferenceValue as Component;
                if (p_component && !p_component.gameObject.scene.IsValid())
                {
                    EditorGUILayout.HelpBox("Selected particles object is not present in the scene. So it is prefab and it will be instantiated on start.", MessageType.Info);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void AddWindow()
        {
            Undo.RegisterFullObjectHierarchyUndo(script, "Added window");

            VolumetricWindow window = null;
            switch (windowType)
            {
                case VolumetricWindow.WindowTypes.Duplicate:
                    if (windows.arraySize > 0)
                    {
                        window = script.AddWindow(script.Windows[script.Windows.Length - 1]);
                    }
                    else
                    {
                        window = script.AddWindow();
                    }
                    break;

                case VolumetricWindow.WindowTypes.Rect:
                    window = script.AddWindow(VolumetricWindow.RectangleShape);
                    break;

                case VolumetricWindow.WindowTypes.Circle:
                    window = script.AddWindow(VolumetricWindow.CircleShape);
                    break;

                case VolumetricWindow.WindowTypes.Mesh:
                    if (windowMesh)
                        window = script.AddWindow(windowMesh);
                    break;

                case VolumetricWindow.WindowTypes.MeshFilter:
                    if (windowMeshFilter && windowMeshFilter.sharedMesh)
                        window = script.AddWindow(windowMeshFilter);
                    break;

                default:
                    window = script.AddWindow();
                    break;
            }

            Selection.activeGameObject = script.Windows[script.Windows.Length - 1].gameObject;
            script.UpdateMesh(false);

            if (window != null)
                Undo.RegisterCreatedObjectUndo(window.gameObject, "Created new window");
        }

    }
}