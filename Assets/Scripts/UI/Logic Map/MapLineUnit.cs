using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLineUnit : MonoBehaviour
{
    LineRenderer lineRenderer;
    List<MapLinePoint> pointList;
    EdgeCollider2D collider;

    List<Vector2> colliderPoints;
    // Start is called before the first frame update
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        collider = GetComponent<EdgeCollider2D>();

        colliderPoints = new List<Vector2>();
        colliderPoints.Add(Vector2.zero);
        colliderPoints.Add(Vector2.zero);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (pointList != null)
        {
            if (pointList[0])
            {
                transform.position = pointList[0].transform.position + Vector3.forward;
            }
            lineRenderer.positionCount = pointList.Count;
            lineRenderer.SetPositions(GetLineVectors());
            if (pointList.Count>1)
            {
                Vector2 difference = pointList[1].transform.position - pointList[0].transform.position;
                colliderPoints[1] = difference;
                collider.SetPoints(colliderPoints);
            }
            
        }
        
    }

    public Vector3[] GetLineVectors()
    {
        Vector3[] posList = new Vector3[pointList.Count];
        for (int i = 0; i < pointList.Count; i++)
        {
            posList[i] = pointList[i].transform.position;
        }
        return posList;
    }
    public void InsertPoint(MapLinePoint point, MapLinePoint relativePoint, bool isBehind)
    {
        int index = pointList.IndexOf(relativePoint);
        if (isBehind)
        {
            index++;
        }
        pointList.Insert(index, point);
    }
    public void SetUp(MapLinePoint point)
    {
        pointList = new List<MapLinePoint>();
        AddPoint(point);
        //point.lineUnit = this;
    }
    public void AddPoint(MapLinePoint point)
    {
        pointList.Add(point);
        point.AddLineUnit(this);
    }
    public void OnDestroy()
    {
        
    }
    public void DestroySelf()
    {
        foreach (MapLinePoint point in pointList)
        {
            point.RemoveLineUnit(this);
        }
        Destroy(gameObject);
    }
    public void Disconnect()
    {
        pointList[0].RemoveConnectedPoints(pointList[1]);
        pointList[1].RemoveConnectedPoints(pointList[0]);
    }
    public void CreateNewPoint()
    {
        Disconnect();
        DestroySelf();
        MapLineController.main.CreatePointFromLine(pointList[0],pointList[1]);
    }


    public void RemovePoint(MapLinePoint point)
    {
        pointList.Remove(point); 
    }
}
