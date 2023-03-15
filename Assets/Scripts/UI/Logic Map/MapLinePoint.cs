using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapLinePoint : MapMark
{
    private MapLineUnit[] lineUnits;
    public MapLinePoint[] connectedPoints = new MapLinePoint[2];
    //public MapLinePoint last, next;
    public float stickyRange = .5f;
    /* SpriteRenderer sr;
     private bool dragging;*/
    // Start is called before the first frame update
    private void Awake()
    {
        SetUp();
    }
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    public void SetUp()
    {
        if (lineUnits == null)
        {
            lineUnits = new MapLineUnit[2];
        }
        if (connectedPoints == null)
        {
            connectedPoints = new MapLinePoint[2];
        }
    }
    protected override void OnMouseOver()
    {
        /*if (MouseDrag.GetMouseDragging(1))
            transform.position = PublicMethods.GetWorldPosFromMousePos();*/


        if (!dragging && Input.GetMouseButtonDown(2) && CheckEmpty()>0)
        {
            MapLineController.main.CreatePointFromPoint(this);
        }

        base.OnMouseOver();
    }

    /*protected override void OnMouseEnter()
    {
        MapLineController.mouseEnterPoint++;
        sr.color = Color.yellow;
    }

    protected override void OnMouseExit()
    {
        MapLineController.mouseEnterPoint--;
        sr.color = Color.white;
    }*/
    /*public void InserRelativePoint(MapLinePoint point, bool isBehind)
    {
        lineUnit.InsertPoint(point, this, isBehind);
    }*/
    /*public override IEnumerator Drag(int mouseButton)
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
    }*/
    public override void Remove()
    {
        foreach (MapLineUnit line in lineUnits)
        {
            if (line != null)
            {
                line.DestroySelf();
            }
        }
        Debug.Log("remove");
        if (!connectedPoints.Contains(null))
        {
            /*Destroy(lineUnits[0].gameObject);
            Destroy(lineUnits[1].gameObject);*/
            connectedPoints[0].RemoveConnectedPoints(this);
            connectedPoints[1].RemoveConnectedPoints(this);
            MapLineController.main.ConnectTwoPoints(connectedPoints[0], connectedPoints[1]);
        }
        else
        {
            if (CheckEmpty() < 2)
            {
                MapLinePoint point = connectedPoints.First(m => m != null);
                point.RemoveConnectedPoints(this);
            }
            //point.RemoveLineUnit(lineUnits.First(m => m != null));
            //Destroy(lineUnits.First(l => l != null).gameObject);
        }
        base.Remove();
    }
    public void ReplaceConnectedPoints(MapLinePoint from, MapLinePoint to)
    {
        for (int i = 0; i < connectedPoints.Length; i++)
        {
            if (connectedPoints[i] == from)
            {
                connectedPoints[i] = to;
            }
        }
    }
    public void RemoveConnectedPoints(MapLinePoint point)
    {
        for (int i = 0; i < connectedPoints.Length; i++)
        {
            if (connectedPoints[i] == point)
            {
                connectedPoints[i] = null;
            }
        }
    }
    public void AddConnectedPoint(MapLinePoint point)
    {
        for (int i = 0; i < connectedPoints.Length; i++)
        {
            if (connectedPoints[i] == null)
            {
                connectedPoints[i] = point;
                return;
            }
        }
    }

    public void RemoveLineUnit(MapLineUnit unit)
    {
        for (int i = 0; i < lineUnits.Length; i++)
        {
            if (lineUnits[i] == unit)
            {
                lineUnits[i] = null;
            }
        }
    }
    public void AddLineUnit(MapLineUnit unit)
    {
        for (int i = 0; i < lineUnits.Length; i++)
        {
            if (lineUnits[i] == null)
            {
                lineUnits[i] = unit;
                return;
            }
        }
    }
    public int CheckEmpty()
    {
        int num = 0;
        for (int i = 0; i < connectedPoints.Length; i++)
        {
            if (connectedPoints[i] == null)
            {
                num++;
            }
        }
        return num;
    }
    public bool CheckConnected(MapLinePoint point)
    {
        return connectedPoints.Contains(point);
    }
    public override IEnumerator Drag(int mouseButton)
    {
        dragging = true;
        Collider2D[] inPoints;
        Collider2D inPoint;
        while (Input.GetMouseButton(mouseButton))
        {
            Vector3 pos = PublicMethods.GetWorldPosFromMousePos();
            pos.z = -5;
            inPoints = Physics2D.OverlapPointAll(transform.position, LayerMask.GetMask("Mark"));
            inPoint = null;
            foreach (Collider2D point in inPoints)
            {
                if (point.GetComponent<MapLinePoint>() && point.transform != transform)
                {
                    inPoint = point;
                }
            }
            //Debug.Log(inPoint);
            if (inPoint != null && Vector2.Distance(inPoint.transform.position, pos) < stickyRange)
            {
                transform.position = inPoint.transform.position;
            }
            else
            {
                transform.position = pos;
            }

            yield return null;
        }
        inPoints = Physics2D.OverlapPointAll(transform.position, LayerMask.GetMask("Mark"));
        inPoint = null;
        foreach (Collider2D point in inPoints)
        {
            if (point.GetComponent<MapLinePoint>() && point.transform != transform)
            {
                inPoint = point;
            }
        }
        //Debug.Log(inPoint);
        if (inPoint != null && Vector2.Distance(inPoint.transform.position, transform.position) < .05f)
        {
            RemoveOnDragging(inPoint.GetComponent<MapLinePoint>());
        }


        dragging = false;
    }
    private void RemoveOnDragging(MapLinePoint other)
    {
        if (CheckEmpty() > 0 && other.CheckEmpty() > 0)
        {
            /*MapLinePoint last= connectedPoints.First(m => m != null);
            if(last!=null)*/
            if (!CheckConnected(other))
                MapLineController.main.ConnectTwoPoints(this, other);
            Remove();
            //MapLineController.main.ConnectTwoPoints(last,other);
        }
        else if (other.CheckConnected(this))
        {
            Remove();
        }

    }
}
