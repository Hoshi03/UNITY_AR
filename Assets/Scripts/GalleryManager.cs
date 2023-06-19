using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryManager : MonoBehaviour
{
    public GameObject gallery;
    public bool isGalleryOpen = false;
    public void galleryOn()
    {
        if (isGalleryOpen == false)
        {
            isGalleryOpen = true;
            gallery.SetActive(true);
            Debug.Log("°¶·¯¸® on");
        }

        else if (isGalleryOpen == true)
        {
            isGalleryOpen = false;
            gallery.SetActive(false);
            Debug.Log("°¶·¯¸® off");
        }
    }
}
