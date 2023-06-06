using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FaceObjectPlacement : MonoBehaviour
{
    public ARFaceManager faceManager; // AR Face Manager 컴포넌트 참조
    public GameObject objectPrefab; // 생성할 오브젝트 프리팹

    private GameObject spawnedObject; // 생성된 오브젝트
    private Touch selectedTouch; // 선택된 터치
    private Vector3 touchOffset; // 터치한 위치와 오브젝트의 오프셋

    private void OnEnable()
    {
        // 얼굴 인식 이벤트 수신을 위해 이벤트 핸들러 등록
        faceManager.facesChanged += OnFacesChanged;
    }

    private void OnDisable()
    {
        // 이벤트 핸들러 등록 해제
        faceManager.facesChanged -= OnFacesChanged;
    }

    private void OnFacesChanged(ARFacesChangedEventArgs eventArgs)
    {
        foreach (var face in eventArgs.added)
        {
            // 얼굴이 인식되면 오브젝트를 생성하고 배치
            Vector3 spawnPosition = face.transform.position + face.transform.right * 0.5f; // 얼굴 옆에 오브젝트 생성
            Quaternion spawnRotation = face.transform.rotation;
            spawnedObject = Instantiate(objectPrefab, spawnPosition, spawnRotation);
        }
    }

    private void Update()
    {
        // 터치 입력 처리
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // 오브젝트를 터치한 경우 선택
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject == spawnedObject)
                        {
                            selectedTouch = touch;
                            touchOffset = spawnedObject.transform.position - hit.point;
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    // 선택한 오브젝트를 드래그하여 이동
                    if (selectedTouch.fingerId == touch.fingerId)
                    {
                        Vector3 touchPosition = touch.position;
                        touchPosition.z = Camera.main.nearClipPlane;
                        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(touchPosition) + touchOffset;
                        spawnedObject.transform.position = targetPosition;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    // 선택한 터치가 끝난 경우 선택 해제
                    if (selectedTouch.fingerId == touch.fingerId)
                    {
                        selectedTouch = new Touch();
                    }
                    break;
            }
        }
    }
}
