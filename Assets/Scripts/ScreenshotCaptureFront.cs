using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.XR.ARFoundation;

public class ScreenshotCaptureFront : MonoBehaviour
{
    // ��ũ���� ���
    public void CaptureScreenshot()
    {
        StartCoroutine(TakeScreenshot());
    }

    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForSeconds(0.5f);
        // ��ũ���� �̹��� ����
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshotTexture.Apply();


        // ��ũ���� ���ϸ�
        string screenshotFileName = GetUniqueFileName("screenshot", "png");

        // ���� ���� ���
        string saveFolderPath = Path.Combine("/storage/emulated/0/DCIM/", "ARF");

        // ������ ���� ��� ����
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }

        // ��ũ���� ���� ���
        string screenshotPath = Path.Combine(saveFolderPath, screenshotFileName);

        // ��ũ���� ����
        File.WriteAllBytes(screenshotPath, screenshotTexture.EncodeToPNG());

        // �Ϸ� �޽��� ���
        Debug.Log("��ũ������ ����Ǿ����ϴ�. ���: " + screenshotPath);


        // �ȵ���̵忡�� �̵�� ��ĵ ����
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass environment = new AndroidJavaClass("android.os.Environment");
            string externalStoragePath = environment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory").Call<string>("getAbsolutePath");
            string[] paths = { screenshotPath };
            AndroidJavaClass mediaScannerConnection = new AndroidJavaClass("android.media.MediaScannerConnection");
            mediaScannerConnection.CallStatic("scanFile", new object[] { AndroidJNIHelper.ConvertToJNIArray(paths), null, null });
        }
    }

    // ������ ���� �̸� ����
    private string GetUniqueFileName(string prefix, string extension)
    {
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = string.Format("{0}_{1}.{2}", prefix, timestamp, extension);
        return fileName;
    }
}
