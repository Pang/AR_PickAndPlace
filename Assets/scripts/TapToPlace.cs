using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

enum Shapes : int
{
    Cube = 0,
    Sphere = 1,
}

[RequireComponent(typeof(ARRaycastManager))]
public class TapToPlace : MonoBehaviour
{
    private Shapes selectedShape;
    public List<GameObject> objsToInst;
    public GameObject spawnedObject;

    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPos;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public GameObject testText;

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    bool GetTouchPos(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            Debug.Log(Input.GetTouch(0));
            return true;
        }
        touchPosition = default;
        return false;
    }
    void Update()
    {
        if (!GetTouchPos(out Vector2 touchPos))
            return;
        if(IsPointerOverUIObject())
            return;

        if (_arRaycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon)) 
        {
            PlaceMoveObj(hits[0].pose);
        }
        if (touchPos != null) 
        {
            testText.GetComponent<Text>().text = "X: " + touchPos.x + " | Y: " + touchPos.y;
        }
    }

    private void PlaceMoveObj(Pose hitPose) 
    {
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(objsToInst[(int)selectedShape], hitPose.position, hitPose.rotation);
        }
        else
        {
            spawnedObject.transform.position = hitPose.position;
        }
    }

    private bool IsPointerOverUIObject() 
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void ChangeObjToCube() {
        testText.GetComponent<Text>().text = "Cube Pressed";
        Destroy(spawnedObject);
        selectedShape = Shapes.Cube;
        Debug.Log(objsToInst[(int)selectedShape]);
    }

    public void ChangeObjToSphere() {
            testText.GetComponent<Text>().text = "Sphere Pressed";
        Destroy(spawnedObject);
        selectedShape = Shapes.Sphere;
        Debug.Log(objsToInst[(int)selectedShape]);
    }
}
