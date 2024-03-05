using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NorthLab
{
    /// <summary>
    /// Main class for the EasyVolumetric.
    /// </summary>
    [ExecuteInEditMode]
    public class VolumetricProbe : MonoBehaviour
    {

        /// <summary>
        /// This struct contains positions of the vertices of the island.
        /// </summary>
        [System.Serializable, System.Obsolete]
        public struct Island
        {
            /// <summary>
            /// Position of the points.
            /// </summary>
            public Vector3[] vertices;

            /// <summary>
            /// Create island from the Vector3 array.
            /// </summary>
            public Island(Vector3[] vertices)
            {
                this.vertices = vertices;
            }

            /// <summary>
            /// Create new island identical to the another island.
            /// </summary>
            public Island(Island island)
            {
                vertices = new Vector3[island.vertices.Length];
                for (int i = 0; i < island.vertices.Length; i++)
                {
                    vertices[i] = island.vertices[i];
                }
            }

            /// <summary>
            /// Create new island from the mesh vertices.
            /// </summary>
            public Island(Mesh mesh)
            {
                List<Vector3> vcs = new List<Vector3>();
                for (int i = 0; i < mesh.vertexCount; i++)
                {
                    if (!vcs.Contains(mesh.vertices[i]))
                        vcs.Add(mesh.vertices[i]);
                }
                vertices = vcs.ToArray();
            }

            /// <summary>
            /// Create new island from the mesh filter using its position.
            /// </summary>
            public Island(MeshFilter meshFilter)
            {
                List<Vector3> vcs = new List<Vector3>();
                for (int i = 0; i < meshFilter.sharedMesh.vertexCount; i++)
                {
                    if (!vcs.Contains(meshFilter.sharedMesh.vertices[i]))
                        vcs.Add(meshFilter.sharedMesh.vertices[i]);
                }

                for (int i = 0; i < vcs.Count; i++)
                {
                    vcs[i] = meshFilter.transform.TransformPoint(vcs[i]);
                }
                vertices = vcs.ToArray();
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
            /// Scale all vertices by scale value.
            /// </summary>
            public void Scale(float scale)
            {
                for (int i = 0; i < vertices.Length; i++)
                    vertices[i] *= scale;
            }
        }

        [SerializeField, System.Obsolete]
        private Island[] islands = new Island[0];
        /// <summary>
        /// Islands of this probe. OBSOLETE! Use <see cref="VolumetricWindow"/>.
        /// </summary>
        [System.Obsolete]
        public Island[] Islands
        {
            get
            {
                return islands;
            }
            set
            {
                if (islands == value)
                    return;

                islands = value;
                UpdateMesh(false);
            }
        }

        [SerializeField]
        private VolumetricWindow[] windows = new VolumetricWindow[0];
        /// <summary>
        /// Windows of this probe. New version of the <see cref="Island"/>.
        /// </summary>
        public VolumetricWindow[] Windows
        {
            get
            {
                return windows;
            }
            set
            {
                if (windows == value)
                    return;

                windows = value;
                UpdateMesh(false);
            }
        }
            
        [SerializeField, Header("Options")]
        private Light lightSource = null;
        /// <summary>
        /// Light source used for the calculations.
        /// </summary>
        public Light LightSource
        {
            get
            {
                return lightSource;
            }
            set
            {
                if (lightSource == value)
                    return;

                lightSource = value;
                UpdateMesh(false);
            }
        }

        [SerializeField, Tooltip("You can use transform instead of the LightSource. Remember LightSource has more priority!")]
        private Transform overrideTransform = null;
        /// <summary>
        /// You can use transform instead of the LightSource. Remember LightSource has more priority!
        /// </summary>
        public Transform OverrideTransform
        {
            get
            {
                return overrideTransform;
            }
            set
            {
                if (overrideTransform == value)
                    return;

                overrideTransform = value;
                UpdateMesh(false);
            }
        }

        [SerializeField]
        private float overrideIntensity = 1;
        [SerializeField]
        private Color overrideColor = Color.white;

        /// <summary>
        /// Final intensity based off the LightSource or Point.
        /// </summary>
        public float Intensity => (lightSource ? lightSource.intensity : (overrideTransform ? overrideIntensity : 0)) * lightIntensityMultiplier;
        /// <summary>
        /// Final color based off the LightSource or Point.
        /// </summary>
        public Color Color => lightSource ? lightSource.color : (overrideTransform ? overrideColor : Color.white);
        /// <summary>
        /// Final color based off the LightSource or Point.
        /// </summary>
        public LightType LightType => lightSource ? lightSource.type : LightType.Point;
        /// <summary>
        /// Final transform based off the LightSource or Point.
        /// </summary>
        public Transform FinalTransform => lightSource ? lightSource.transform : overrideTransform;

        [SerializeField, Tooltip("Volumetric probe will be calculated only once and on validation(only outside of the Play mode)")]
        private bool isStatic = false;
        /// <summary>
        /// Volumetric probe will be calculated only once and on validation.
        /// </summary>
        public bool IsStatic
        {
            get
            {
                return isStatic;
            }
            set
            {
                if (isStatic == value)
                    return;

                isStatic = value;
                UpdateMesh(false);
            }
        }

        [SerializeField, Tooltip("Will force to update volumetric mesh every frame.")]
        private bool forceUpdate = false;
        /// <summary>
        /// Will force to update volumetric mesh every frame.
        /// </summary>
        public bool ForceUpdate
        {
            get
            {
                return forceUpdate;
            }
            set
            {
                forceUpdate = value;
            }
        }

        [SerializeField, Header("Physics"), Tooltip("Use Raycast to calculate volume")]
        private bool usePhysics = false;
        /// <summary>
        /// Use Raycast to calculate volume.
        /// </summary>
        public bool UsePhysics
        {
            get
            {
                return usePhysics;
            }
            set
            {
                if (usePhysics == value)
                    return;

                usePhysics = value;
                UpdateMesh(false);
            }
        }

        [SerializeField]
        private float rayDist = 3;
        /// <summary>
        /// Distance of the raycasts.
        /// </summary>
        public float RayDist
        {
            get
            {
                return rayDist;
            }
            set
            {
                if (rayDist == value)
                    return;

                rayDist = value;
                UpdateMesh(false);
            }
        }

        [SerializeField]
        private LayerMask layerMask = new LayerMask();
        /// <summary>
        /// LayerMask of the raycasts.
        /// </summary>
        public LayerMask LayerMask
        {
            get
            {
                return layerMask;
            }
            set
            {
                if (layerMask == value)
                    return;

                layerMask = value;
                UpdateMesh(false);
            }
        }

        [SerializeField, Header("Appearence")]
        private Material material = null;
        public Material Material
        {
            get
            {
                return material;
            }
            set
            {
                if (material == value)
                    return;

                material = value;
                UpdateMesh(true);
            }
        }
        [SerializeField, Space]
        private bool useLightColor = true;
        /// <summary>
        /// Use light color while calculating volumetric.
        /// </summary>
        public bool UseLightColor
        {
            get
            {
                return useLightColor;
            }
            set
            {
                if (useLightColor == value)
                    return;

                useLightColor = value;
                UpdateMesh(true);
            }
        }

        [SerializeField]
        private bool useLightIntensity = false;
        /// <summary>
        /// Use light intensity while calculating volumetric.
        /// </summary>
        public bool UseLightIntensity
        {
            get
            {
                return useLightIntensity;
            }
            set
            {
                if (useLightIntensity == value)
                    return;

                useLightIntensity = value;
                UpdateMesh(true);
            }
        }

        [SerializeField]
        private float lightIntensityMultiplier = 1;
        public float LightIntensityMultiplier
        {
            get
            {
                return lightIntensityMultiplier;
            }
            set
            {
                if (lightIntensityMultiplier == value)
                    return;

                lightIntensityMultiplier = value;
                UpdateMesh(true);
            }
        }

        [SerializeField, Tooltip("Clamp the light intensity when calculating the ray color.")]
        private float lightIntensityClamp = 1;
        /// <summary>
        /// Clamp the light intensity when calculating the ray color.
        /// </summary>
        public float LightIntensityClamp
        {
            get
            {
                return lightIntensityClamp;
            }
            set
            {
                if (lightIntensityClamp == value)
                    return;

                lightIntensityClamp = value;
                UpdateMesh(true);
            }
        }

        [SerializeField, Space]
        private Color startColor = Color.white;
        /// <summary>
        /// Start color of the volumetric.
        /// </summary>
        public Color StartColor
        {
            get
            {
                return startColor;
            }
            set
            {
                if (startColor == value)
                    return;

                startColor = value;
                UpdateMesh(true);
            }
        }

        [SerializeField]
        private Color endColor = new Color(1, 1, 1, 0);
        /// <summary>
        /// End color of the volumetric.
        /// </summary>
        public Color EndColor
        {
            get
            {
                return endColor;
            }
            set
            {
                if (endColor == value)
                    return;

                endColor = value;
                UpdateMesh(true);
            }
        }

        [SerializeField, Space]
        private ParticleSystem particles = null;

        private Mesh mesh;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private Color oldLightColor;
        private float oldLightIntensity;
        private int calculationFrame = -1;

        //temp arrays
        private List<Vector3> verts = new List<Vector3>();
        private List<int> tris = new List<int>();
        private List<Color> vertColor = new List<Color>();
        private Vector3[] worldVertices = new Vector3[0];
        private Vector3[] relativeVertices = new Vector3[0];

        private void Awake()
        {
            if (Application.isPlaying)
            {
                //Calculate mesh
                UpdateMesh(false);

                //if in the play mode and particles is assigned then throw meshrenderer to it
                if (particles)
                {
                    //if particles object is prefab we should instantiate it first
                    if (!particles.gameObject.scene.IsValid())
                    {
                        particles = Instantiate(particles, transform.position, Quaternion.identity);
                        particles.transform.SetParent(transform);
                    }

                    ParticleSystem.ShapeModule pShape = particles.shape;
                    pShape.shapeType = ParticleSystemShapeType.MeshRenderer;
                    pShape.meshRenderer = meshRenderer;
                    particles.Clear();
                    particles.Simulate(particles.main.duration);
                    particles.Play();
                }
            }
        }

        //recalculate mesh when component values is changed
        private void OnValidate()
        {
            if (lightIntensityClamp < 0)
                lightIntensityClamp = 0;

            if (Application.isPlaying && isStatic)
                return;

            UpdateMesh(false);
        }

        private void Update()
        {
            //exit if the lightsource or point is not assigned
            if (!lightSource && !overrideTransform)
                return;

            //dont check for changes if isStatic
            if (isStatic)
                return;

            //if forceUpdate is true then update mesh and visuals without checking
            if (forceUpdate)
            {
                UpdateMesh(false);
            }
            else
            {
                if (FinalTransform.hasChanged || transform.hasChanged)
                {
                    FinalTransform.hasChanged = transform.hasChanged = false;
                    UpdateMesh(false);
                }
                else if (Color != oldLightColor || Intensity != oldLightIntensity)
                {
                    UpdateMesh(true);
                }
            }
        }

        private void CreateMeshComponents()
        {
            //Creating new MeshFilter and MeshRenderer to the object
            mesh = new Mesh();

            meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            meshFilter.mesh = mesh;

            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }
        }

        private void CalculateTriangles()
        {
            //clear lists for verices, tris and mesh
            verts.Clear();
            tris.Clear();
            mesh.Clear();

            //main calculation algorithm
            int addIndex = 0;
            for (int s = 0; s < windows.Length; s++)
            {
                if (windows[s] == null)
                    continue;

                if (windows[s].Probe != this)
                    windows[s].SetProbe(this);

                worldVertices = windows[s].WorldVertices();
                relativeVertices = windows[s].RelativeVertices(transform);
                verts.AddRange(relativeVertices);

                if (windows[s].VertexCount < 2)
                    continue;

                if (s > 0 && windows[s - 1] != null)
                    addIndex += windows[s - 1].VertexCount * 2;

                for (int i = 0; i < windows[s].VertexCount; i++)
                {
                    if (i == 0)
                    {
                        tris.Add(i + addIndex);
                        tris.Add(windows[s].VertexCount + 1 + addIndex);
                        tris.Add(windows[s].VertexCount + addIndex);

                        tris.Add(i + addIndex);
                        tris.Add(i + 1 + addIndex);
                        tris.Add(windows[s].VertexCount + 1 + addIndex);
                    }
                    else if (i == windows[s].VertexCount - 1)
                    {
                        tris.Add(i + addIndex);
                        tris.Add(windows[s].VertexCount + addIndex);
                        tris.Add(windows[s].VertexCount + i + addIndex);

                        tris.Add(i + addIndex);
                        tris.Add(0 + addIndex);
                        tris.Add(windows[s].VertexCount + addIndex);
                    }
                    else
                    {
                        tris.Add(i + addIndex);
                        tris.Add(windows[s].VertexCount + i + 1 + addIndex);
                        tris.Add(windows[s].VertexCount + i + addIndex);

                        tris.Add(i + addIndex);
                        tris.Add(i + 1 + addIndex);
                        tris.Add(windows[s].VertexCount + i + 1 + addIndex);
                    }

                    RaycastHit hit;
                    Vector3 dir;
                    switch (LightType)
                    {
                        case LightType.Directional:
                            if (usePhysics && Physics.Raycast(new Ray(worldVertices[i], lightSource.transform.forward), out hit, rayDist, layerMask))
                            {
                                verts.Add(transform.InverseTransformPoint(hit.point));
                            }
                            else
                            {
                                verts.Add(relativeVertices[i] + lightSource.transform.forward * rayDist);
                            }
                            
                            break;

                        case LightType.Spot:
                            float offset = lightSource.spotAngle * lightSource.range / 90f;
                            Vector3 origin = worldVertices[i];
                            Vector3 pointDir = (origin - lightSource.transform.position).normalized;
                            Vector3 target = lightSource.transform.position + (lightSource.transform.forward) * lightSource.range + pointDir * offset;
                            dir = (target - origin).normalized;
                            if (usePhysics && Physics.Raycast(new Ray(worldVertices[i], dir), out hit, rayDist, layerMask))
                            {
                                verts.Add(transform.InverseTransformPoint(hit.point));
                            }
                            else
                            {
                                verts.Add(transform.InverseTransformPoint(worldVertices[i] + dir * rayDist));
                            }
                            break;

                        default:
                            dir = (worldVertices[i] - FinalTransform.position).normalized;
                            if (usePhysics && Physics.Raycast(new Ray(worldVertices[i], dir), out hit, rayDist, layerMask))
                            {
                                verts.Add(transform.InverseTransformPoint(hit.point));
                            }
                            else
                            {
                                verts.Add(transform.InverseTransformPoint(worldVertices[i] + dir * rayDist));
                            }
                            break;
                    }
                }
            }

            //update mesh
            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
        }

        private void UpdateVisuals()
        {
            if (verts.Count == 0)
                return;

            vertColor.Clear();

            Color sCol = useLightColor ? Color * startColor : startColor;
            if (useLightIntensity)
                sCol *= Mathf.Clamp(Intensity, 0, lightIntensityClamp);

            Color eCol = useLightColor ? Color * endColor : endColor;
            if (useLightIntensity)
                eCol *= Mathf.Clamp(Intensity, 0, lightIntensityClamp);
            eCol.a = 0;

            for (int s = 0; s < windows.Length; s++)
            {
                if (windows[s] == null)
                    continue;

                for (int i = 0; i < windows[s].VertexCount; i++)
                {
                    vertColor.Add(sCol);
                }

                for (int i = 0; i < windows[s].VertexCount; i++)
                {
                    vertColor.Add(eCol);
                }
            }

            //update mesh
            mesh.SetColors(vertColor);
        }

        /// <summary>
        /// Update volumetric probe mesh.
        /// </summary>
        public void UpdateMesh(bool onlyVisuals)
        {
            if (calculationFrame == Time.frameCount)
                return;

            calculationFrame = Time.frameCount;

            //stop if lightsource is not assigned
            if (!lightSource && !overrideTransform)
            {
                Debug.LogError("Light Source is not assigned!");
                return;
            }

            //create necessary components if they are not present in the object
            if (meshFilter == null || meshRenderer == null)
                CreateMeshComponents();

            //check if mesh for some reason is null
            if (mesh == null)
            {
                meshFilter.mesh = mesh = new Mesh();
            }

            //assign material
            meshRenderer.material = material;

            //update light color and intensity
            oldLightColor = Color;
            oldLightIntensity = Intensity;

            if (!onlyVisuals)
            {
                CalculateTriangles();
            }

            UpdateVisuals();

            mesh.UploadMeshData(false);
        }

        /// <summary>
        /// Add empty window.
        /// </summary>
        /// <returns>New created window.</returns>
        public VolumetricWindow AddWindow()
        {
            GameObject go = new GameObject("VolumetricWindow_" + (windows.Length + 1));
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            VolumetricWindow window = go.AddComponent<VolumetricWindow>();
            window.SetProbe(this);

            System.Array.Resize(ref windows, windows.Length + 1);
            windows[windows.Length - 1] = window;

            return window;
        }

        /// <summary>
        /// Add window and fill it with the vertices array.
        /// </summary>
        /// <returns>New created window.</returns>
        public VolumetricWindow AddWindow(Vector3[] vertices)
        {
            VolumetricWindow window = AddWindow();
            window.Fill(vertices);
            return window;
        }

        /// <summary>
        /// Duplicate window.
        /// </summary>
        /// <returns>New created window.</returns>
        public VolumetricWindow AddWindow(VolumetricWindow original)
        {
            VolumetricWindow window = AddWindow();
            window.transform.position = original.transform.position + Vector3.right;
            window.Fill(original);
            return window;
        }

        /// <summary>
        /// Add new window from the mesh.
        /// </summary>
        /// <returns>New created window.</returns>
        public VolumetricWindow AddWindow(Mesh mesh)
        {
            VolumetricWindow window = AddWindow();
            window.Fill(mesh);
            return window;
        }

        /// <summary>
        /// Add new window from the MeshFilter.
        /// </summary>
        /// <returns>New created window.</returns>
        public VolumetricWindow AddWindow(MeshFilter meshFilter)
        {
            VolumetricWindow window = AddWindow();
            window.Fill(meshFilter);
            return window;
        }

        /// <summary>
        /// Convert old island to the window.
        /// </summary>
        /// <returns>New created window.</returns>
        [System.Obsolete]
        public VolumetricWindow AddWindow(Island island)
        {
            VolumetricWindow window = AddWindow();
            window.Fill(island);
            return window;
        }

        /// <summary>
        /// Converts obsolete islands to the windows.
        /// </summary>
        [System.Obsolete]
        public void ConvertIslandsToWindows()
        {
            if (islands.Length <= 0)
                return;

            for (int i = 0; i < islands.Length; i++)
            {
                AddWindow(islands[i]);
            }

            UpdateMesh(false);
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Effects/Volumetric Probe")]
        public static void CreateVolumetricProbe()
        {
            SceneView view = SceneView.lastActiveSceneView;
            GameObject go = new GameObject("VolumetricProbe");
            go.transform.position = view.camera.transform.position + view.camera.transform.forward * 2;
            go.AddComponent<VolumetricProbe>();
            Selection.activeGameObject = go;
        }
#endif

    }
}