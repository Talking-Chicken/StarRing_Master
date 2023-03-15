using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLineController : MonoBehaviour
{
    public GameObject linePointPrefab, lineUnitPrefab;

    public static int mouseEnterPoint;
    public static MapLineController main;
    private void Awake()
    {
        if (main==null)
        {
            main = this;
        }
        mouseEnterPoint = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreatePoint(Vector3 pos)
    {
        if (mouseEnterPoint > 0)
        {
            return;
        }
        pos.z = -5;
        MapLinePoint fromPoint=Instantiate(linePointPrefab, pos, Quaternion.identity, transform).GetComponent<MapLinePoint>();
        MapLinePoint point = Instantiate(linePointPrefab, pos, Quaternion.identity, transform).GetComponent<MapLinePoint>();
        fromPoint.SetUp();
        point.SetUp();
        /*fromPoint.last = point;
        point.next = fromPoint;*/
        ConnectTwoPoints(fromPoint, point);


        /*MapLineUnit lineUnit = Instantiate(lineUnitPrefab, transform).GetComponent<MapLineUnit>();
        lineUnit.SetUp(point);
        lineUnit.AddPoint(fromPoint);*/
        //point.InserRelativePoint(fromPoint, true);
        point.StartCoroutine(point.Drag(2));
    }
    public MapLinePoint CreatePointFromPoint(MapLinePoint fromPoint)
    {
        if(fromPoint.CheckEmpty()<=0)
        {
            return null;
        }
        Vector3 pos = PublicMethods.GetWorldPosFromMousePos();
        pos.z = -5;
        MapLinePoint point= Instantiate(linePointPrefab, pos, Quaternion.identity, transform).GetComponent<MapLinePoint>();
        point.SetUp();
        ConnectTwoPoints(point,fromPoint);
        point.StartCoroutine(point.Drag(2));
        return point;
    }
    public MapLinePoint CreatePointFromLine(MapLinePoint fromPoint, MapLinePoint toPoint)
    {
        if (fromPoint.CheckEmpty() <= 0 || toPoint.CheckEmpty() <= 0)
        {
            return null;
        }
        // MapController.mapController.creatingLine = true;

        Vector3 pos = PublicMethods.GetWorldPosFromMousePos();
        pos.z = -5;
        MapLinePoint point = Instantiate(linePointPrefab, pos, Quaternion.identity, transform).GetComponent<MapLinePoint>();
        point.SetUp();
        ConnectTwoPoints(point, fromPoint);
        ConnectTwoPoints(point, toPoint);
        point.StartCoroutine(point.Drag(2));
        return point;
    }

    public void ConnectTwoPoints(MapLinePoint point1, MapLinePoint point2)
    {
        if (point1.CheckConnected(point2))
        {
            return;
        }
        MapLineUnit lineUnit = Instantiate(lineUnitPrefab, transform).GetComponent<MapLineUnit>();
        lineUnit.SetUp(point1);
        lineUnit.AddPoint(point2);
        point1.AddConnectedPoint(point2);
        point2.AddConnectedPoint(point1);
    }
}

