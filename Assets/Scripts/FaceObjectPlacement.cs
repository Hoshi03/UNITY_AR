using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FaceObjectPlacement : MonoBehaviour
{
    public ARFaceManager faceManager; // AR Face Manager ������Ʈ ����
    public GameObject objectPrefab; // ������ ������Ʈ ������

    private GameObject spawnedObject; // ������ ������Ʈ
    private Touch selectedTouch; // ���õ� ��ġ
    private Vector3 touchOffset; // ��ġ�� ��ġ�� ������Ʈ�� ������

    private void OnEnable()
    {
        // �� �ν� �̺�Ʈ ������ ���� �̺�Ʈ �ڵ鷯 ���
        faceManager.facesChanged += OnFacesChanged;
    }

    private void OnDisable()
    {
        // �̺�Ʈ �ڵ鷯 ��� ����
        faceManager.facesChanged -= OnFacesChanged;
    }

    private void OnFacesChanged(ARFacesChangedEventArgs eventArgs)
    {
        foreach (var face in eventArgs.added)
        {
            // ���� �νĵǸ� ������Ʈ�� �����ϰ� ��ġ
            Vector3 spawnPosition = face.transform.position + face.transform.right * 0.5f; // �� ���� ������Ʈ ����
            Quaternion spawnRotation = face.transform.rotation;
            spawnedObject = Instantiate(objectPrefab, spawnPosition, spawnRotation);
        }
    }

    private void Update()
    {
        // ��ġ �Է� ó��
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // ������Ʈ�� ��ġ�� ��� ����
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
                    // ������ ������Ʈ�� �巡���Ͽ� �̵�
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
                    // ������ ��ġ�� ���� ��� ���� ����
                    if (selectedTouch.fingerId == touch.fingerId)
                    {
                        selectedTouch = new Touch();
                    }
                    break;
            }
        }
    }
}
