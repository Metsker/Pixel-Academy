using System;
using System.Collections;
using System.IO;
using _Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebRequest : MonoBehaviour
{
    private const string JsonLink = "https://dl.dropboxusercontent.com/s/gfi2xu1wu2tc55d/imglink.json?dl=0";
    [SerializeField] private string link;
    
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private Slider slider;
    [SerializeField] private Image background;
    
    private string imgLink = null;

    private void Start()
    {
        StartCoroutine(DownloadBackgroundRoutine());
#if UNITY_STANDALONE_WIN
        var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Roaming\\.minecraft\\mods";
        inputField.text = path;
#elif UNITY_STANDALONE_OSX
        var pathX = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Application Support/minecraft/mods";
        inputField.text = pathX;
#endif
    }

    public void Download()
    {
        StartCoroutine(DownloadRoutine());
    }

    public void Test()
    {
        print(Directory.GetDirectoryRoot(Application.persistentDataPath));
    }
    
    private IEnumerator DownloadBackgroundRoutine()
    {
        UnityWebRequest wr1 = UnityWebRequest.Get(JsonLink);
        yield return wr1.SendWebRequest();
        if (wr1.result == UnityWebRequest.Result.Success)
        {
            var j = JsonUtility.FromJson<JsonData>(wr1.downloadHandler.text);
            imgLink = j.pic_url;
        }
        else
        {
            yield break;
        }
        
        UnityWebRequest wr2 = UnityWebRequest.Get(imgLink);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture();
        wr2.downloadHandler = texDl;
        yield return wr2.SendWebRequest();
        if (wr2.result == UnityWebRequest.Result.Success) 
        {
            Texture2D t = texDl.texture;
            Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height),
                Vector2.zero, 1f);
            background.sprite = s;
        }
        wr1.Dispose();
        wr2.Dispose();
    }
    private IEnumerator DownloadRoutine()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(link);
        
        string savePath = inputField.text;
        if (!savePath.EndsWith("\\"))
        {
            savePath += "\\";
        }
        ClearDirectory(savePath);
        
        string zipPath = savePath + "mods.zip";
        
        webRequest.downloadHandler = new DownloadHandlerFile(zipPath);
        webRequest.SendWebRequest();
        
        while (!webRequest.downloadHandler.isDone)
        {
            slider.value = webRequest.downloadProgress;
            yield return null;
        }
        
        var bytes = File.ReadAllBytes(zipPath);
        
        ZipFile.UnZip(savePath, bytes);
        
        File.SetAttributes(zipPath, FileAttributes.Normal);
        File.Delete(zipPath);

        if (webRequest.result == UnityWebRequest.Result.Success || webRequest.result == UnityWebRequest.Result.InProgress)
        {
            debugText.text = "Success";
        }
        else
        {
            debugText.text = "Failed";
        }
        webRequest.Dispose();
    }
    public static void ClearDirectory(string target_dir)
    {
        string[] files = Directory.GetFiles(target_dir);
        string[] dirs = Directory.GetDirectories(target_dir);

        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }
        foreach (string dir in dirs)
        {
            ClearDirectory(dir);
        }
    }
}
