using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARController : MonoBehaviour
{
    public GameObject[] spawnedObjects;

    private GameObject spawnedObject;
    private ARRaycastManager raycastMgr;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool Touched = false;
    private GameObject SelectedObj;
    private float touchDuration = 0f;
    private const float deletionTimeThreshold = 2f;


    [SerializeField] private int currentObjectIndex;
    [SerializeField] private Camera arCamera;

    void Start()
    {
        spawnedObject = spawnedObjects[currentObjectIndex];
        spawnedObject.transform.localScale = Vector3.one * 0.5f;
        raycastMgr = GetComponent<ARRaycastManager>();
    }

    void Update()
    {

        if (Input.touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;
                if (Input.touchCount == 0) return;
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray;
                    RaycastHit hitobj;
                    ray = arCamera.ScreenPointToRay(touch.position);

                    if (Physics.Raycast(ray, out hitobj))
                    {
                        if (hitobj.collider.name.Contains("unitychan"))
                        {
                            SelectedObj = hitobj.collider.gameObject;
                            Touched = true;
                            touchDuration = Time.time;
                        }
                        else
                        {
                            if (raycastMgr.Raycast(touch.position, hits, 
                                TrackableType.PlaneWithinPolygon))
                            {
                                Quaternion rotation = hits[0].pose.rotation * 
                                Quaternion.Euler(0, 180, 0);
                                Instantiate(spawnedObject, hits[0].pose.position, rotation);
                            }
                        }
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    Touched = false;
                    touchDuration = 0f;
                }

                if (Touched && Time.time - touchDuration >= deletionTimeThreshold)
                {
                    Destroy(SelectedObj);
                    Touched = false;
                    touchDuration = 0f;
                }

                if (raycastMgr.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    if (Touched)
                    {
                        SelectedObj.transform.position = hits[0].pose.position;
                    }
                }
        }
    }


    //오브젝트 변경
    public void ChangeObject()
    {
        currentObjectIndex++;
        if (currentObjectIndex >= spawnedObjects.Length)
            currentObjectIndex = 0;
        spawnedObject = spawnedObjects[currentObjectIndex];
        Debug.Log(spawnedObject.name);
    }
}
