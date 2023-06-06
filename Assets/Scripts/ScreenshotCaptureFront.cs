using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.XR.ARFoundation;

public class ScreenshotCaptureFront : MonoBehaviour
{
    // 스크린샷 찍기
    public void CaptureScreenshot()
    {
        StartCoroutine(TakeScreenshot());
    }

    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForSeconds(0.5f);
        // 스크린샷 이미지 생성
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshotTexture.Apply();


        // 스크린샷 파일명
        string screenshotFileName = GetUniqueFileName("screenshot", "png");

        // 저장 폴더 경로
        string saveFolderPath = Path.Combine("/storage/emulated/0/DCIM/", "ARF");

        // 폴더가 없는 경우 생성
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }

        // 스크린샷 저장 경로
        string screenshotPath = Path.Combine(saveFolderPath, screenshotFileName);

        // 스크린샷 저장
        File.WriteAllBytes(screenshotPath, screenshotTexture.EncodeToPNG());

        // 완료 메시지 출력
        Debug.Log("스크린샷이 저장되었습니다. 경로: " + screenshotPath);


        // 안드로이드에서 미디어 스캔 실행
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass environment = new AndroidJavaClass("android.os.Environment");
            string externalStoragePath = environment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory").Call<string>("getAbsolutePath");
            string[] paths = { screenshotPath };
            AndroidJavaClass mediaScannerConnection = new AndroidJavaClass("android.media.MediaScannerConnection");
            mediaScannerConnection.CallStatic("scanFile", new object[] { AndroidJNIHelper.ConvertToJNIArray(paths), null, null });
        }
    }

    // 고유한 파일 이름 생성
    private string GetUniqueFileName(string prefix, string extension)
    {
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = string.Format("{0}_{1}.{2}", prefix, timestamp, extension);
        return fileName;
    }
}
