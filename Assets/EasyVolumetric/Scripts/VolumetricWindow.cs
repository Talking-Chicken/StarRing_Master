using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NorthLab
{

    /// <summary>
    /// This class contains positions of the vertices of the volumetric "window". Replaces old "Islands".
    /// Uses object's transform.
    /// </summary>
    [ExecuteInEditMode]
    public class VolumetricWindow : MonoBehaviour
    {

        /// <summary>
        /// Types of windows that showed in the editor.
        /// </summary>
        public enum WindowTypes { Empty, Duplicate, Rect, Circle, Mesh, MeshFilter };

        /// <summary>
        /// Rectangle shape definition.
        /// </summary>
        public static readonly Vector3[] RectangleShape = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
            new Vector3(0, 0, 1)
        };

        /// <summary>
        /// Circle shape definition.
        /// </summary>
        public static readonly Vector3[] CircleShape = new Vector3[]
        {
            new Vector3(0.5f, 0, 0),
            new Vector3(0.43f, 0, 0.25f),
            new Vector3(0.25f, 0, 0.43f),
            new Vector3(0, 0, 0.5f),
            new Vector3(-0.25f, 0, 0.43f),
            new Vector3(-0.43f, 0, 0.25f),
            new Vector3(-0.5f, 0, 0),
            new Vector3(-0.43f, 0, -0.25f),
            new Vector3(-0.25f, 0, -0.43f),
            new Vector3(0, 0, -0.5f),
            new Vector3(0.25f, 0, -0.43f),
            new Vector3(0.43f, 0, -0.25f)
        };

        private readonly static Color[] WindowsColors = { Color.green, Color.yellow, Color.red, Color.blue, Color.magenta, Color.cyan, Color.white };
        private static int c;

        [SerializeField, Tooltip("This window will send messages to update probe's mesh when changed.")]
        private VolumetricProbe probe = null;
        /// <summary>
        /// Volumetric probe thats this window is assigned to.
        /// </summary>
        public VolumetricProbe Probe => probe;

        [SerializeField]
        private Vector3[] vertices = new Vector3[0];
        /// <summary>
        /// Count of the points.
        /// </summary>
        public int VertexCount => vertices.Length;
        /// <summary>
        /// Positions of the points.
        /// </summary>
        public Vector3[] GetVertices()
        {
            Vector3[] result = new Vector3[vertices.Length];
            vertices.CopyTo(result, 0);
            return result;
        }

        /// <summary>
        /// Change points. This method checks the window as changed.
        /// </summary>
        public void SetVertices(Vector3[] vertices)
        {
            this.vertices = vertices;
            Changed = true;
        }

        /// <summary>
        /// Are vertices were changed in this frame?
        /// </summary>
        public bool Changed { get; private set; } = false;

        [SerializeField]
        private Color gizmoColor = Color.white;

        private Vector3[] worldVertices = new Vector3[0];
        private Vector3[] relativeVertices = new Vector3[0];

        private void OnValidate()
        {
            if (!probe)
                return;

            if (Application.isPlaying && probe.IsStatic)
                return;

            probe.UpdateMesh(false);
        }

        private void Update()
        {
            if (probe && (transform.hasChanged || Changed))
            {
                transform.hasChanged = false;
                if (!probe.IsStatic)
                    probe.UpdateMesh(false);
            }
            Changed = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;

            for (int i = 0; i < worldVertices.Length; i++)
            {
                if (i < worldVertices.Length - 1)
                    Gizmos.DrawLine(worldVertices[i], worldVertices[i + 1]);
                else Gizmos.DrawLine(worldVertices[i], worldVertices[0]);
            }
        }

        private void Reset()
        {
            gizmoColor = WindowsColors[c];
            if (++c >= WindowsColors.Length)
                c = 0;
        }

        /// <summary>
        /// Returns positions of the points in the world space.
        /// </summary>
        public Vector3[] WorldVertices()
        {
            if (worldVertices.Length != vertices.Length)
                System.Array.Resize(ref worldVertices, vertices.Length);

            for (int i = 0; i < vertices.Length; i++)
            {
                worldVertices[i] = transform.TransformPoint(vertices[i]);
            }
            return worldVertices;
        }

        /// <summary>
        /// Converts world vertices to local relative to the other transform.
        /// </summary>
        public Vector3[] RelativeVertices(Transform relativeTo)
        {
            if (relativeVertices.Length != vertices.Length)
                System.Array.Resize(ref relativeVertices, vertices.Length);

            for (int i = 0; i < relativeVertices.Length; i++)
            {
                relativeVertices[i] = relativeTo.InverseTransformPoint(worldVertices[i]);
            }
            return relativeVertices;
        }

        /// <summary>
        /// Set VolumetricProbe of the window.
        /// </summary>
        public void SetProbe(VolumetricProbe probe)
        {
            this.probe = probe;
        }

        /// <summary>
        /// Create window from the Vector3 array.
        /// </summary>
        public void Fill(Vector3[] vertices)
        {
            this.vertices = vertices;
        }

        /// <summary>
        /// Create new window identical to the another window.
        /// </summary>
        public void Fill(VolumetricWindow window)
        {
            vertices = new Vector3[window.vertices.Length];
            window.vertices.CopyTo(vertices, 0);
        }

        /// <summary>
        /// Create new window from the mesh vertices.
        /// </summary>
        public void Fill(Mesh mesh)
        {
            vertices = new Vector3[mesh.vertexCount];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = mesh.vertices[i];
            }
        }

        /// <summary>
        /// Create new window from the mesh filter using its position.
        /// </summary>
        public void Fill(MeshFilter meshFilter)
        {
            vertices = new Vector3[meshFilter.sharedMesh.vertexCount];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = meshFilter.transform.TransformPoint(meshFilter.sharedMesh.vertices[i]);
            }
        }

        /// <summary>
        /// Create new window from an island.
        /// </summary>
        [System.Obsolete]
        public void Fill(VolumetricProbe.Island island)
        {
            Fill(island.vertices);
        }

        /// <summary>
        /// Swap vertices
        /// </summary>
        public void Swap(int a, int b)
        {
            Vector3 temp = vertices[a];
            vertices[a] = vertices[b];
            vertices[b] = temp;
        }

        /// <summary>
        /// Sets new pivot for the window and keeps all vertices positions.
        /// </summary>
        public void SetPivot(Vector3 position)
        {
            transform.position = position;
            for(int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = transform.InverseTransformPoint(worldVertices[i]);
            }
        }

    }
}