using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectCreator : MonoBehaviour
{
    public GameObject[] spawnedObjects;

    private GameObject spawnedObject;
    private GameObject createdObject;
    private bool isDragging = false;
    private bool isTouchingObject = false;
    private float pressStartTime = 0f;
    private float pressDurationThreshold = 2f;

    [SerializeField] private int currentObjectIndex;

    void Start()
    {
        spawnedObject = spawnedObjects[currentObjectIndex];
        spawnedObject.transform.localScale = Vector3.one * 3f;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchPosition = touch.position;
                touchPosition.z = 10f; // Set the distance from the camera
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

                RaycastHit hit;
                if (Physics.Raycast(worldPosition, Camera.main.transform.forward, out hit))
                {
                    createdObject = hit.collider.gameObject;
                    pressStartTime = Time.time;
                    isTouchingObject = true;
                }
                else
                {
                    Quaternion rotation = Quaternion.Euler(0f, 180f, 0f); // Rotate around y-axis by 180 degrees
                    createdObject = Instantiate(spawnedObject, worldPosition, rotation);
                    pressStartTime = Time.time;
                    isTouchingObject = true;
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (isDragging)
                {
                    Vector3 touchPosition = touch.position;
                    touchPosition.z = 10f;
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                    createdObject.transform.position = worldPosition;
                }
                else if (isTouchingObject)
                {
                    float pressDuration = Time.time - pressStartTime;
                    if (pressDuration >= pressDurationThreshold)
                    {
                        Destroy(createdObject);
                        isTouchingObject = false;
                    }
                    else
                    {
                        isDragging = true;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
    }

    public void ChangeObject()
    {
        currentObjectIndex++;
        if (currentObjectIndex >= spawnedObjects.Length)
            currentObjectIndex = 0;
        spawnedObject = spawnedObjects[currentObjectIndex];

        // 셀카모드시 크기 오류나서 강제로 키움
        if (spawnedObject.gameObject.name.Contains("humanoid"))
        {
            spawnedObject.transform.localScale = Vector3.one * 3f;
        }

        Debug.Log(spawnedObject.name);
    }
}