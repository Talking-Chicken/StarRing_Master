using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.PostProcessing;

// this is only a simple example code to show the zoomming is possible with the correct hierarchy, please, use a more reliable code in your project 
namespace MeadowGames.UINodeConnect4.SampleScene.ScrollViewGraph
{
    public class Zoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        bool _mouseIsOver;
        public RectTransform contentRectTransform;
        public GraphManager graphManager;

        [MinMax(0.05f, 10)] public Vector2 zoomScaleRange = new(0, 2);
        public float scrollSpeed = .2f;

        void Start()
        {
            graphManager = GetComponentInParent<GraphManager>();
        }

        void Update()
        {
            if (_mouseIsOver)
            {
                if (Input.mouseScrollDelta.y != 0)
                {
                    float newScale = contentRectTransform.localScale.x + Input.mouseScrollDelta.y * scrollSpeed;
                    // contentRectTransform.localScale += Vector3.one * Input.mouseScrollDelta.y * 0.2f;

                    //clamp the scale by zoomScaleRange
                    newScale = Mathf.Clamp(newScale, zoomScaleRange.x, zoomScaleRange.y);
                    contentRectTransform.localScale = newScale * Vector3.one;

                    graphManager.connectionDetectionDistance = 15 * contentRectTransform.localScale.x;
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _mouseIsOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _mouseIsOver = false;
        }
    }
}