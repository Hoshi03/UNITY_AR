using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

public class CameraSwitcher : MonoBehaviour
{

    public void SwitchCamera()
    {
        if (SceneManager.GetActiveScene().name == "BackCam")
            SceneManager.LoadScene("FrontCam");
        if (SceneManager.GetActiveScene().name == "FrontCam")
            SceneManager.LoadScene("BackCam");
    }
}
