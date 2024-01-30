using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Michsky.DreamOS
{
    public class WorldSpaceManager : MonoBehaviour
    {
        //Listeners
        [SerializeField] private DreamOSActionListener _dreamOSListener;

        // Content
        public List<AudioSource> audioSources = new List<AudioSource>();

        // Resources
        public Camera mainCamera;
        public Camera projectorCam;
        [SerializeField] private RawImage rendererImage;
        [SerializeField] private Transform enterMount;
        [SerializeField] private Canvas osCanvas;
        [SerializeField] private PressKeyEvent pressKeyEvent;

        // Settings
        public bool requiresOpening = true;
        public bool autoGetIn = false;
        [SerializeField] private bool warmComponents = false;
        [SerializeField] private bool setCursorState = true;
        [SerializeField] private bool dynamicRTSize = true;
        [SerializeField] private int rtWidth = 1920;
        [SerializeField] private int rtHeight = 1080;
        public string playerTag = "Player";
        public InputAction getInKey;
        public InputAction getOutKey;
        [Range(1, 10)] public float audioBlendSpeed = 3;
        [Range(0.1f, 4)] public float transitionTime = 1f;
        public AnimationCurve transitionCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));
        public PositionMode positionMode = PositionMode.Local;

        // Events
        public UnityEvent onTriggerEnter = new UnityEvent();
        public UnityEvent onTriggerExit = new UnityEvent();
        public UnityEvent onEnter = new UnityEvent();
        public UnityEvent onEnterEnd = new UnityEvent();
        public UnityEvent onExit = new UnityEvent();
        public UnityEvent onExitEnd = new UnityEvent();

        // Helpers
        [HideInInspector] public RenderTexture uiRT;
        [HideInInspector] public int selectedTagIndex = 0;
        [HideInInspector] public bool isInSystem = false;
        bool isInTrigger = false;
        bool takenLocalRootPos;
        CanvasGroup osCG;
        Quaternion camRotHelper;
        Vector3 targetRootPos = new Vector3(0, 0, 0);

        public enum PositionMode { Local, World }

        void Awake()
        {
            if (dynamicRTSize) { uiRT = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.RGB111110Float); }
            else { uiRT = new RenderTexture(rtWidth, rtHeight, 24, RenderTextureFormat.RGB111110Float); }

            if (projectorCam == null) 
            { 
                Debug.LogError("<b>[DreamOS]</b> Projector Camera is missing.");
                return; 
            }

            projectorCam.targetTexture = uiRT;
            projectorCam.enabled = true;

            if (rendererImage != null) { rendererImage.texture = uiRT; }
            else { Debug.LogWarning("<b>[DreamOS]</b> Renderer Image is missing. The system will work but won't be rendered in 3D."); }

            osCG = osCanvas.GetComponent<CanvasGroup>();
            osCG.interactable = false;
            osCG.blocksRaycasts = false;

            if (pressKeyEvent != null) { pressKeyEvent.enabled = false; }
            if (mainCamera == null) { mainCamera = Camera.main; }
        }

        void Start()
        {
            if (warmComponents && requiresOpening) { Invoke("WarmComponentsHelper", 0.5f); }
            else if (requiresOpening) { osCanvas.gameObject.SetActive(false); }
        }

        void OnEnable()
        {
            getInKey.Enable();
            getOutKey.Enable();
            _dreamOSListener.openUI.AddListener(TransitionInHelper);
            _dreamOSListener.closeUI.AddListener(TransitionOutHelper);
        }

        void OnDisable()
        {
            getInKey.Disable();
            getOutKey.Disable();
            _dreamOSListener.openUI.AddListener(TransitionInHelper);
            _dreamOSListener.closeUI.AddListener(TransitionOutHelper);
        }

        void Update()
        {
            // if (!isInSystem && getInKey.triggered) { TransitionInHelper(); }
            // else if (!isInTrigger && isInSystem && getOutKey.triggered) { TransitionOutHelper(); }
            if (isInSystem && getOutKey.triggered) {TransitionOutHelper();}
        }

        public void EnableCamera(bool value) { mainCamera.enabled = value; }
        public void GetIn() { TransitionInHelper(); }
        public void GetOut() { TransitionOutHelper(); }

        void TransitionInHelper()
        {
            // if (!isInTrigger || isInSystem)
            //     return;
            if (isInSystem)
                return;

            // Events
            onEnter.Invoke();
            onTriggerExit.Invoke();

            // Main CG
            osCG.interactable = true;
            osCG.blocksRaycasts = true;
            osCanvas.gameObject.SetActive(true);

            // Check for PKE
            if (pressKeyEvent != null) { pressKeyEvent.enabled = true; }

            // Camera Position and Rotation
            if (positionMode == PositionMode.World) { targetRootPos = mainCamera.transform.position; }
            else if (positionMode == PositionMode.Local && !takenLocalRootPos)
            {
                targetRootPos = mainCamera.transform.localPosition;
                takenLocalRootPos = true;
            }

            camRotHelper = mainCamera.transform.localRotation;

            // States
            isInTrigger = false;

            // Check for cursor
            if (setCursorState)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            // Camera Transition
            StopCoroutine("CameraTransitionIn");
            StopCoroutine("CameraTransitionOut");
            StartCoroutine("CameraTransitionIn");

            // Audio
            if (audioSources.Count != 0)
            {
                StopCoroutine("BlendAudioSources2D");
                StopCoroutine("BlendAudioSources3D");
                StartCoroutine("BlendAudioSources2D");
            }
        }

        void TransitionOutHelper()
        {
            // Events
            onExit.Invoke();
            onTriggerEnter.Invoke();
            // _dreamOSListener.stopInteract.Invoke(null);

            // Main CG
            osCG.interactable = false;
            osCG.blocksRaycasts = false;

            // Check for PKE
            if (pressKeyEvent != null) { pressKeyEvent.enabled = false; }

            // Rendering Stuff
            osCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            projectorCam.targetTexture = uiRT;
            projectorCam.enabled = true;

            // Check for cursor
            if (setCursorState) 
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            // Camera Transition
            StopCoroutine("CameraTransitionIn");
            StopCoroutine("CameraTransitionOut");
            StartCoroutine("CameraTransitionOut");

            // Audio
            if (audioSources.Count != 0)
            {
                StopCoroutine("BlendAudioSources2D");
                StopCoroutine("BlendAudioSources3D");
                StartCoroutine("BlendAudioSources3D");
            }
        }

        void WarmComponentsHelper()
        {
            osCanvas.gameObject.SetActive(false);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == playerTag)
            {
                isInTrigger = true;
                onTriggerEnter.Invoke();
                if (autoGetIn) { TransitionInHelper(); }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == playerTag) 
            {
                isInTrigger = false;
                onTriggerExit.Invoke();
            }
        }

        IEnumerator CameraTransitionIn()
        {
            float elapsedTime = 0;
            Vector3 startingPos = mainCamera.transform.position;
            Quaternion startingRot = mainCamera.transform.rotation;

            while (elapsedTime < transitionTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
               
                mainCamera.transform.position = Vector3.Lerp(startingPos, enterMount.position, transitionCurve.Evaluate((elapsedTime / transitionTime)));
                mainCamera.transform.rotation = Quaternion.Slerp(startingRot, enterMount.rotation, transitionCurve.Evaluate((elapsedTime / transitionTime)));
               
                yield return null;
            }

            // Set Pos and Rot
            mainCamera.transform.position = enterMount.position;
            mainCamera.transform.rotation = enterMount.rotation;

            // Process Stuff
            osCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            projectorCam.enabled = false;
            isInSystem = true;
            onEnterEnd.Invoke();
        }

        IEnumerator CameraTransitionOut()
        {
            float elapsedTime = 0;
            Vector3 startingPos = mainCamera.transform.localPosition;
            Quaternion startingRot = mainCamera.transform.localRotation;

            while (elapsedTime < transitionTime)
            {
                elapsedTime += Time.unscaledDeltaTime;

                mainCamera.transform.localPosition = Vector3.Lerp(startingPos, targetRootPos, transitionCurve.Evaluate((elapsedTime / transitionTime)));
                mainCamera.transform.localRotation = Quaternion.Slerp(startingRot, camRotHelper, transitionCurve.Evaluate((elapsedTime / transitionTime)));

                yield return null;
            }

            // Set Pos and Rot
            mainCamera.transform.localPosition = targetRootPos;
            mainCamera.transform.localRotation = camRotHelper;

            // Process Stuff
            isInSystem = false;
            isInTrigger = true;
            onExitEnd.Invoke();
        }

        IEnumerator BlendAudioSources2D()
        {
            float elapsedTime = 0;
            float startinPoint = audioSources[0].spatialBlend;

            while (audioSources[0].spatialBlend > 0.01)
            {
                foreach (AudioSource tempSource in audioSources) 
                {
                    if (tempSource == null)
                        continue;

                    tempSource.spatialBlend = Mathf.Lerp(startinPoint, 0, elapsedTime * audioBlendSpeed);
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            foreach (AudioSource tempSource in audioSources) { tempSource.spatialBlend = 0; }
        }

        IEnumerator BlendAudioSources3D()
        {
            float elapsedTime = 0;
            float startinPoint = audioSources[0].spatialBlend;

            while (audioSources[0].spatialBlend < 0.99)
            {
                foreach (AudioSource tempSource in audioSources) 
                {
                    if (tempSource == null)
                        continue;

                    tempSource.spatialBlend = Mathf.Lerp(startinPoint, 1, elapsedTime * audioBlendSpeed);
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            foreach (AudioSource tempSource in audioSources) { tempSource.spatialBlend = 1; }
        }
    }
}