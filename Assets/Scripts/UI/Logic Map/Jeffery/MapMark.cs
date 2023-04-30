using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMark : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected bool dragging, clickChecking;
    public enum MarkType { MapPoint, Danger, Caution, Info, Cow, MapLine}
    public MarkType type;

    protected bool isMouseEnter;

    public float clickCheckTime = .1f;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }
    protected virtual void OnMouseOver()
    {
        /*if (MouseDrag.GetMouseDragging(1))
            transform.position = PublicMethods.GetWorldPosFromMousePos();*/
        /*if (MapController.mapController.isOn)
        {
            if (!dragging && Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Drag(0));
            }
            else if (!dragging && Input.GetMouseButtonDown(1))
            {
                Remove();
            }
        }*/
        /*if (!dragging && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Drag(0));
        }
        else if (!dragging && Input.GetMouseButtonDown(1))
        {
            Remove();
        }*/

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ClickCheck());
        }
    }

    protected virtual void OnMouseEnter()
    {
        isMouseEnter = true;
        MapLineController.mouseEnterPoint++;
        sr.color = Color.yellow;
    }

    protected virtual void OnMouseExit()
    {
        isMouseEnter = false;
        MapLineController.mouseEnterPoint--;
        sr.color = Color.white;
    }
    public virtual void Remove()
    {
        if (isMouseEnter)
            MapLineController.mouseEnterPoint--;
        Destroy(gameObject);
    }
    public virtual IEnumerator Drag(int mouseButton)
    {
        dragging = true;
        while (Input.GetMouseButton(mouseButton))
        {
            Vector3 pos = PublicMethods.GetWorldPosFromMousePos();
            pos.z = -5;
            transform.position = pos;

            yield return null;
        }
        dragging = false;
    }
    protected virtual IEnumerator ClickCheck()
    {
        clickChecking = true;
        Debug.Log("click");
        Vector2 mousePos = Input.mousePosition;
        for (float i = clickCheckTime; i >= 0; i -= Time.deltaTime)
        {
            if (Vector2.Distance(mousePos, Input.mousePosition) > .3f)
            {
                break;
            }
            yield return null;
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("click remove");
                Remove();
                yield break;
            }

        }
        //Debug.Log("remove");
        if (!dragging && Input.GetMouseButton(0))
        {
            StartCoroutine(Drag(0));
        }
        clickChecking = false;
    }
}
