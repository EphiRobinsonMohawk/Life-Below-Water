using System.IO;
using UnityEngine;

public class Photography : MonoBehaviour
{
    public Camera photoCamera;
    public int photoWidth = 1920;
    public int photoHeight = 1080;

    public void TakePhoto()
    {
        // 1. Create a temporary Render Texture
        RenderTexture rt = new RenderTexture(photoWidth, photoHeight, 24);
        photoCamera.targetTexture = rt;

        // 2. Manually tell the camera to render its view to that texture
        Texture2D screenshot = new Texture2D(photoWidth, photoHeight, TextureFormat.RGB24, false);
        photoCamera.Render();

        // 3. Set the active Render Texture to our texture and read the pixels
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, photoWidth, photoHeight), 0, 0);
        screenshot.Apply();

        // 4. Clean up (Very important to avoid memory leaks!)
        photoCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // 5. Save to disk
        SaveImage(screenshot);
    }

    private void SaveImage(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        string filename = $"Photo_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
        string path = Path.Combine(Application.persistentDataPath, filename);

        File.WriteAllBytes(path, bytes);
        Debug.Log($"Photo saved to: {path}");

        // Destroy the texture to free up GPU memory
        Destroy(texture);
    }
}
