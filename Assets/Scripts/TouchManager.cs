using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TouchManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject placeObject;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    void Start()
    {
        placeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        placeObject.transform.localScale = Vector3.one * 0.05f;
        raycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0) return;
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            if (raycastManager.Raycast(touch.position, hits, TrackableType.AllTypes))
                Instantiate(placeObject, hits[0].pose.position, hits[0].pose.rotation);

            //Vector3 touchPosition = touch.position;
            //Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            //if (raycastManager.Raycast(ray, hits, TrackableType.AllTypes))
            //    Instantiate(placeObject, hits[0].pose.position, hits[0].pose.rotation);
        }

    }
}
