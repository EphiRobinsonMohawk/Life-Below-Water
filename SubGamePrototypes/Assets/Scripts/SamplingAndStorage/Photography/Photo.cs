using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Photo : MonoBehaviour
{
    private string filePath;
    private GalleryDisplay galleryDisplay;
    private Sprite photoSprite;

    public void Setup(string path, GalleryDisplay manager, Sprite sprite)
    {
        filePath = path;
        galleryDisplay = manager;
        photoSprite = sprite;
    }

    public void DeletePhoto()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        
        // Remove this photo UI element from the gallery
        Destroy(gameObject);
    }

    public void ViewFullscreen()
    {
        if (galleryDisplay != null)
        {
            galleryDisplay.ShowFullscreen(photoSprite);
        }
    }
}
