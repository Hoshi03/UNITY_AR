using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class LoadImagesFromFolder : MonoBehaviour
{
    public RawImage imageHolder;
    public GameObject imageContainer;
    private Texture2D[] textures;
    private int currentImageIndex;

    private void Start()
    {
        StartCoroutine(LoadImages());
    }

    private IEnumerator LoadImages()
    {
        yield return new WaitForSeconds(1f);
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
            yield return new WaitForSeconds(1f);
        }

        string folderPath = "/storage/emulated/0/DCIM/ARF/";
        string[] files = Directory.GetFiles(folderPath, "*.png");
        textures = new Texture2D[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            string filePath = files[i];
            byte[] bytes;
            if (File.Exists(filePath))
            {
                bytes = File.ReadAllBytes(filePath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);
                textures[i] = texture;
            }

            yield return null;
        }

        if (textures.Length > 0)
        {
            currentImageIndex = (currentImageIndex - 1 + textures.Length) % textures.Length;
            imageHolder.texture = textures[currentImageIndex];
            //imageHolder.texture = textures[0];
            //currentImageIndex = 0;
        }
    }

    public void ShowNextImage()
    {
        if (textures.Length > 0)
        {
            currentImageIndex = (currentImageIndex + 1) % textures.Length;
            imageHolder.texture = textures[currentImageIndex];
        }
    }

    public void ShowPreviousImage()
    {
        if (textures.Length > 0)
        {
            currentImageIndex = (currentImageIndex - 1 + textures.Length) % textures.Length;
            imageHolder.texture = textures[currentImageIndex];
        }
    }
}
