﻿using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

// v4.0.2 - added missing namespace to the CustomScrollRect class
namespace MeadowGames.UINodeConnect4.SampleScene.ScrollViewGraph
{
    public class CustomScrollRect : ScrollRect
    {
        int maximumTouchCount = 2;

        const PointerEventData.InputButton button = PointerEventData.InputButton.Right;

        public Vector2 MultiTouchPosition
        {
            get
            {
                Vector2 position = Vector2.zero;
                for (int i = 0; i < Input.touchCount && i < maximumTouchCount; i++)
                {
                    position += Input.touches[i].position;
                }
                position /= ((Input.touchCount <= maximumTouchCount) ? Input.touchCount : maximumTouchCount);

                return position;
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button == button)
            {
                eventData.button = PointerEventData.InputButton.Left;
                base.OnBeginDrag(eventData);
            }
            if (Input.touchCount >= maximumTouchCount)
            {
                eventData.position = MultiTouchPosition;
                base.OnBeginDrag(eventData);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button == button)
            {
                eventData.button = PointerEventData.InputButton.Left;
                base.OnEndDrag(eventData);
            }
            if (Input.touchCount >= maximumTouchCount)
            {
                eventData.position = MultiTouchPosition;
                base.OnEndDrag(eventData);
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == button)
            {
                eventData.button = PointerEventData.InputButton.Left;
                base.OnDrag(eventData);
            }
            if (Input.touchCount >= maximumTouchCount)
            {
                eventData.position = MultiTouchPosition;
                base.OnDrag(eventData);
            }
        }

    }
}