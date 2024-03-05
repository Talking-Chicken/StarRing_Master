using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NorthLab
{

    /// <summary>
    /// Custom editor class for the VolumetricWindow.
    /// </summary>
    [CustomEditor(typeof(VolumetricWindow))]
    public class VolumetricWindowEditor : Editor
    {

        private enum FillType { Rect, Circle, Mesh, MeshFilter };

        private SerializedProperty probe;
        private SerializedProperty vertices;
        private SerializedProperty gizmoColor;
        private VolumetricWindow script;

        private bool verticesFoldOut;
        private bool editMode;
        private FillType windowType;
        private Mesh windowMesh;
        private MeshFilter windowMeshFilter;
        private List<Vector3> deltas = new List<Vector3>();
        private Vector3[] verticesChanged;

        private void OnEnable()
        {
            probe = serializedObject.FindProperty("probe");
            vertices = serializedObject.FindProperty("vertices");
            gizmoColor = serializedObject.FindProperty("gizmoColor");

            script = (VolumetricWindow)target;
        }

        private void OnDisable()
        {
            Tools.hidden = false;
        }

        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(probe);
            EditorGUILayout.PropertyField(gizmoColor);

            //foldoutstyle
            GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.fontStyle = FontStyle.Bold;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            verticesFoldOut = EditorGUILayout.Foldout(verticesFoldOut, "Vertices", foldoutStyle);
            EditorGUI.indentLevel--;

            if (verticesFoldOut)
            {
                float oldWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 100;
                EditorGUI.indentLevel++;
                for (int i = 0; i < vertices.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(vertices.GetArrayElementAtIndex(i));
                    GUI.enabled = i > 0;
                    if (GUILayout.Button("▲", GUILayout.MaxWidth(24)))
                    {
                        Undo.RegisterFullObjectHierarchyUndo(script, "Changed pivot");
                        script.Swap(i, i - 1);
                    }
                    GUI.enabled = i < vertices.arraySize - 1;
                    if (GUILayout.Button("▼", GUILayout.MaxWidth(24)))
                    {
                        Undo.RegisterFullObjectHierarchyUndo(script, "Changed pivot");
                        script.Swap(i, i + 1);
                    }
                    GUI.enabled = true;
                    if (GUILayout.Button("Set Pivot", GUILayout.MaxWidth(64)))
                    {
                        Undo.RegisterFullObjectHierarchyUndo(script, "Changed pivot");
                        script.SetPivot(script.transform.TransformPoint(vertices.GetArrayElementAtIndex(i).vector3Value));
                    }
                    if (GUILayout.Button("X", GUILayout.MaxWidth(24)))
                    {
                        vertices.DeleteArrayElementAtIndex(i);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Add vertex"))
                {
                    vertices.InsertArrayElementAtIndex(vertices.arraySize);
                    vertices.GetArrayElementAtIndex(vertices.arraySize - 1).vector3Value += Vector3.right * 0.5f;
                }
                GUI.enabled = vertices.arraySize > 0;
                if (GUILayout.Button("Clear"))
                {
                    vertices.ClearArray();
                }
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Center pivot"))
                {
                    if (vertices.arraySize > 0)
                    {
                        Vector3 pos = vertices.GetArrayElementAtIndex(0).vector3Value;
                        for (int i = 1; i < vertices.arraySize; i++)
                        {
                            pos += vertices.GetArrayElementAtIndex(i).vector3Value;
                        }
                        pos /= vertices.arraySize;
                        pos = script.transform.TransformPoint(pos);
                        Undo.RegisterFullObjectHierarchyUndo(script, "Changed pivot");
                        script.SetPivot(pos);
                    }
                }
                GUI.enabled = true;

                if (vertices.arraySize <= 0)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Fill", GUILayout.MaxWidth(128)))
                    {
                        Undo.RegisterFullObjectHierarchyUndo(script, "Fill");

                        switch (windowType)
                        {
                            case FillType.Rect:
                                script.Fill(VolumetricWindow.RectangleShape);
                                break;

                            case FillType.Circle:
                                script.Fill(VolumetricWindow.CircleShape);
                                break;

                            case FillType.Mesh:
                                if (windowMesh != null)
                                    script.Fill(windowMesh);
                                break;

                            case FillType.MeshFilter:
                                if (windowMeshFilter != null && windowMeshFilter.sharedMesh != null)
                                    script.Fill(windowMeshFilter.sharedMesh);
                                break;
                        }
                    }

                    windowType = (FillType)EditorGUILayout.EnumPopup("", windowType, GUILayout.MaxWidth(150));
                    if (windowType == FillType.Mesh)
                    {
                        windowMesh = (Mesh)EditorGUILayout.ObjectField(windowMesh, typeof(Mesh), true, GUILayout.MaxWidth(170));
                    }
                    else if (windowType == FillType.MeshFilter)
                    {
                        windowMeshFilter = (MeshFilter)EditorGUILayout.ObjectField(windowMeshFilter, typeof(MeshFilter), true, GUILayout.MaxWidth(170));
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.indentLevel--;

                EditorGUIUtility.labelWidth = oldWidth;
            }

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();

            editMode = EditorGUILayout.Toggle("Edit mode", editMode);
        }

        private void OnSceneGUI()
        {
            Tools.hidden = editMode;

            if (Application.isPlaying)
                return;

            SceneView.RepaintAll();

            Handles.color = gizmoColor.colorValue;

            Vector3[] worldVerts = script.WorldVertices();
            for (int i = 0; i < worldVerts.Length; i++)
            {
                float size = HandleUtility.GetHandleSize(worldVerts[i]) * 0.05f;
                Handles.DrawWireDisc(worldVerts[i], Vector3.up, size);
                Handles.DrawWireDisc(worldVerts[i], Vector3.right, size);
                Handles.DrawWireDisc(worldVerts[i], Vector3.forward, size);
            }

            if (!editMode)
                return;

            EditorGUI.BeginChangeCheck();
            deltas.Clear();
            GUIStyle style = new GUIStyle();
            style.normal.textColor = gizmoColor.colorValue;
            verticesChanged = script.GetVertices();
            for (int i = 0; i < worldVerts.Length; i++)
            {
                Handles.Label(worldVerts[i] + Camera.current.transform.right * 0.1f, i.ToString(), style);
                deltas.Add(verticesChanged[i] - script.transform.InverseTransformPoint(Handles.PositionHandle(worldVerts[i], Quaternion.identity)));
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(script, "Changed vertex pos");
                for (int i = 0; i < vertices.arraySize; i++)
                {
                    verticesChanged[i] = verticesChanged[i] - deltas[i];
                }
                script.SetVertices(verticesChanged);
            }
        }

    }
}