using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Line;
using ModestTree;
using UnityEngine;
using Zenject;

public class DrowLineComponent : MonoBehaviour {
    private LineRenderer _lineRenderer;

    private List<Vector2> fingerPosition = new List<Vector2>();

    private LineState _state = LineState.INITED;
    private LineController _lineController;

    private GuiHandler _guiHandler;
    private GameSettingsInstaller.LineMaterialsSettings _lineMaterialsSettings;

//    private ManufacturerController _first;
//    private ManufacturerController _second;

    [Inject]
    public void Construct(List<Vector3> points,
        List<Company> manufactures,
        GameSettingsInstaller.LineMaterialsSettings lineMaterialsSettings) {

        _lineMaterialsSettings = lineMaterialsSettings;
        _lineRenderer = GetComponent<LineRenderer>();
        _lineController = GetComponent<LineController>();
        _lineController.AddCompany(manufactures[0], manufactures[1]);
        _guiHandler = GameObject.Find("Gui").GetComponent<GuiHandler>();
        fingerPosition = new List<Vector2>();
//        _lineRenderer.positionCount = points.Count;
//        for (int index = 0; index < points.Count; index++) {
//            Vector3 point = points[index];
//            GetComponent<LineRenderer>().SetPosition(index, point);
//        }

        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, manufactures[0].gameObject.transform.position);
        _lineRenderer.SetPosition(1, manufactures[1].gameObject.transform.position);
        AddColliderToLine(_lineRenderer, _lineRenderer.GetPosition(0), _lineRenderer.GetPosition(1));
        manufactures[0].AddLine(_lineController);
        manufactures[1].AddLine(_lineController);
        
        _state = LineState.COMPLETED;
    }

    public void Init(List<Vector3> points) { }

    public void Init(Material material) {
        GetComponent<Renderer>().material = material;
        _state = LineState.INITED;
    }


    private void StartStation(StationController station) {
        Vector3 stationPosition = station.gameObject.transform.position;
        _lineController.AddStation(station);
        station.AddLine(_lineController);

        if (fingerPosition.IsEmpty()) {
            fingerPosition.Add(stationPosition);
            _lineRenderer.SetPosition(0, stationPosition);
            _lineRenderer.SetPosition(1, stationPosition);
        }

        if (Vector2.Distance(fingerPosition.First(), stationPosition) < 0.1f) {
            fingerPosition.Reverse();
            for (var i = 0; i < fingerPosition.Count; i++) {
                _lineRenderer.SetPosition(i, fingerPosition[i]);
            }
        }

        _lineRenderer.SetPosition(fingerPosition.Count - 1, stationPosition);
        // при рисовании нужна будет промежуточная точка, создадим сразу ее
        while (fingerPosition.Count + 2 > _lineRenderer.positionCount) {
            _lineRenderer.positionCount++;
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, stationPosition);
        }
    }

    public void DrowTmpPoint(Vector2 turnPos, Vector2 pointPos) {
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 2, turnPos);
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, pointPos);
    }

//    public void SetType(LineType type) {
//        _lineRenderer.material = type.GetMaterial(_lineMaterialsSettings);
//    }

    private void OnGUI() {
        if (Event.current.type == EventType.MouseDown) {
            Debug.Log("==========================");
            if (ClickOnLine()) {
                _guiHandler.ShowPanelFor(_lineController);                
            }
        }


        switch (_state) {
            case LineState.INITED:
                if (Input.GetMouseButtonDown(0)) {
                    StationController station = ClickOnStation();
                    if (station != null) {
                        Debug.Log("Line create started");
                        StartStation(station);
                        _state = LineState.IN_PROGRESS;
                    }
                    else {
                        Destroy(gameObject);
                    }
                }


                break;
            case LineState.IN_PROGRESS:
                Vector2 tmpPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (Vector2.Distance(tmpPosition, fingerPosition[fingerPosition.Count - 1]) > 0.5f) {
                    Vector2 turnPoint = getPoint(fingerPosition[fingerPosition.Count - 1], tmpPosition);
                    DrowTmpPoint(turnPoint, tmpPosition);
                }

                if (Input.GetMouseButtonDown(0)) {
                    StationController station = ClickOnStation();
                    if (station != null) {
                        Debug.Log("Line create completed");
                        DrowNexStation(station);
                        _state = LineState.COMPLETED;
                        fingerPosition.Add(_lineRenderer.GetPosition(_lineRenderer.positionCount - 2));
                        fingerPosition.Add(_lineRenderer.GetPosition(_lineRenderer.positionCount - 1));
                        for (int i = 1; i < _lineRenderer.positionCount; i++) {
                            AddColliderToLine(_lineRenderer, _lineRenderer.GetPosition(i - 1),
                                _lineRenderer.GetPosition(i));
                        }
                    }
                    else {
                        Debug.Log("Destroy Line");
                        Destroy(gameObject);
                    }
                }

                break;
            case LineState.COMPLETED:
                return;
            default:
                return;
        }
    }
    
    
    private bool ClickOnLine() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] raycastHits = Physics.RaycastAll(ray);
        foreach (RaycastHit raycastHit in raycastHits) {
            Debug.Log(raycastHit.transform.gameObject);

            LineController lineController = raycastHit.transform.GetComponentInParent<LineController>();
            if (lineController == null) {
                return false;
            }
            if (lineController.gameObject == gameObject) {
                return true;
            }
        }
        return false;
    }

    private void DrowNexStation(StationController station) {
        _lineController.AddStation(station);
        Vector3 stationPosition = station.gameObject.transform.position;
        Vector2 turnPoint = getPoint(fingerPosition[fingerPosition.Count - 1], stationPosition);
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 2, turnPoint);
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, stationPosition);
    }

    private static StationController ClickOnStation() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] raycastHits = Physics.RaycastAll(ray);
        foreach (RaycastHit raycastHit in raycastHits) {
            StationController stationController = raycastHit.transform.GetComponent<StationController>();
            if (stationController != null) {
                return stationController;
            }
        }

        return null;
    }

    public Vector2 getPoint(Vector2 f, Vector2 s) {
        Debug.Log("1 " + f + " , 2 " + s);
        float k;
        if (f.x > s.x && f.y > s.y || f.x < s.x && f.y < s.y) {
            k = 1f;
        }
        else {
            k = -1f;
        }

        //y-y0=k(x-x0)
        //kx-kx0-y+y0=0 
        //kx-y+y0-kx0=0 
        // y - 12 = x - 12
        //x-y-0 1,-1,0 12 12
        float fa = k;
        float fb = -1;
        float fc = -k * f.x + f.y;
        Debug.Log(fa + "*x + " + fb + "*y + " + fc);


        float sa;
        float sb;
        float sc;
        if (Math.Abs(f.x - s.x) > Math.Abs(f.y - s.y)) {
            //горизонтально //y=3
            sa = 0;
            sb = -1;
            sc = s.y;
        }
        else {
            // x=3
            //
            sa = 1;
            sb = 0;
            sc = -s.x;
        }

        Debug.Log(sa + "*x + " + sb + "*y + " + sc);

        float x = -(fc * sb - sc * fb) / (fa * sb - sa * fb);
        float y = -(fa * sc - sa * fc) / (fa * sb - sa * fb);


        return new Vector2(x, y);
    }


    private void AddColliderToLine(LineRenderer line, Vector3 startPoint, Vector3 endPoint) {
        //create the collider for the line
        BoxCollider lineCollider = new GameObject("LineCollider").AddComponent<BoxCollider>();
        //set the collider as a child of your line
        lineCollider.transform.parent = line.transform;
        // get width of collider from line 
        float lineWidth = line.endWidth;
        // get the length of the line using the Distance method
        float lineLength = Vector3.Distance(startPoint, endPoint) * 0.8f;
        // size of collider is set where X is length of line, Y is width of line
        //z will be how far the collider reaches to the sky
        lineCollider.size = new Vector3(lineLength, lineWidth, 1f);
        // get the midPoint
        Vector3 midPoint = (startPoint + endPoint) / 2;
        // move the created collider to the midPoint
        lineCollider.transform.position = midPoint;


        //heres the beef of the function, Mathf.Atan2 wants the slope, be careful however because it wants it in a weird form
        //it will divide for you so just plug in your (y2-y1),(x2,x1)
        float angle = Mathf.Atan2((endPoint.y - startPoint.y), (endPoint.x - startPoint.x));

        // angle now holds our answer but it's in radians, we want degrees
        // Mathf.Rad2Deg is just a constant equal to 57.2958 that we multiply by to change radians to degrees
        angle *= Mathf.Rad2Deg;

        //were interested in the inverse so multiply by -1
//        angle *= -1; 
        // now apply the rotation to the collider's transform, carful where you put the angle variable
        // in 3d space you don't wan't to rotate on your y axis
        lineCollider.transform.Rotate(0, 0, angle);
    }

    public class Factory : PlaceholderFactory<List<Vector3>, List<Company>, DrowLineComponent> { }
}