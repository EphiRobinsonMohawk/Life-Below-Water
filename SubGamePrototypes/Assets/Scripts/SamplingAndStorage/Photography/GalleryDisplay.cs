using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GalleryDisplay : MonoBehaviour
{
    public GameObject photoPrefab; // A UI Image prefab
    public Transform contentParent; // The Content object of a Scroll View

    void Start()
    {
        LoadGallery();
    }

    private void LoadGallery()
    {
        // Clear existing items in the gallery first
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // 1. Get all PNG files from the persistent path
        string path = Application.persistentDataPath;
        string[] filePaths = Directory.GetFiles(path, "*.png");

        foreach (string filePath in filePaths)
        {
            AddPhoto(filePath);
        }
    }

    public void AddPhoto(string path)
    {
        // 2. Load the file into a texture
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(fileData); // This auto-resizes the texture dimensions

        // 3. Convert Texture to Sprite
        Sprite photoSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

        // 4. Instantiate UI element
        GameObject newPhoto = Instantiate(photoPrefab, contentParent);
        newPhoto.GetComponent<Image>().sprite = photoSprite;

        // Ensure scale is correct case parent affects it
        newPhoto.transform.localScale = Vector3.one;
    }
}