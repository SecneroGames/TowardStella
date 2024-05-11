using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class SaveManager : MonoBehaviour
{
    [SerializeField] PlayerDataSO PlayerData;
    private string saveFileName = "ts_playerData.json";
    private string saveDirectory; // Updated saveDirectory variable
    public static SaveManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this);

        // Set the save path based on whether in editor or in build
#if UNITY_EDITOR
        saveDirectory = Path.Combine(Application.dataPath, "SaveFile"); // For editor
#else
        saveDirectory = Path.Combine(Application.persistentDataPath, "SaveFile"); // For build
#endif

        // Create the directory if it doesn't exist
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
    }

    private void Start()
    {
        NewGame();
    }

  
    public void SaveData(object data, string fileName)
    {
        string json = JsonUtility.ToJson(data);
        var savePath = Path.Combine(saveDirectory, fileName);
        // Write JSON string to file
        File.WriteAllText(savePath, json);

        Debug.Log("Data saved to: " + savePath);
    }


    public T LoadData<T>(string fileName)
    {
        var savePath = Path.Combine(saveDirectory, fileName);

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            T data = JsonUtility.FromJson<T>(json);

            Debug.Log("Data loaded from: " + savePath);

            return data;
        }
        else
        {
            Debug.LogWarning("No save data found at: " + savePath);
            return default(T);
        }
    }

    bool CheckFile(string fileName)
    {
        var savePath = Path.Combine(saveDirectory, $"{fileName}");
        Debug.Log($"File Found? {File.Exists(savePath)}");
        return (File.Exists(savePath));
    }


    public bool CheckFile()
    {
        var savePath = Path.Combine(saveDirectory, $"{saveFileName}");
        Debug.Log($"File Found? {File.Exists(savePath)}");
        return (File.Exists(savePath));
    }


    public bool IsChapterUnlocked(int chapter)
    {
        return  chapter <= PlayerData.ChapterCompleted;
    }

    //call this to unlock next chapter
    public void UnlockNextChapter(int chapterToUnlock)
    {
        PlayerData.ChapterCompleted = chapterToUnlock;
    }

    public void NewGame()
    {

        PlayerData.ResetToDefaultValues();
        Debug.Log($"Starting New Game");
    }

    public void SaveGame()
    {
        var saveData = new SaveFile(PlayerData);
        SaveData(saveData, saveFileName);
    }

    public void LoadGame()
    {
        if (!CheckFile(saveFileName)) return;

        var saveData = LoadData<SaveFile>(saveFileName);
        PlayerData.CopyFromSaveFile(saveData);


    }
}
