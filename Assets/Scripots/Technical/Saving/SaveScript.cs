using UnityEngine;
using System.IO;

[System.Serializable]
public class Data
{
    public float playerX;
    public float playerY;
    public float playerZ;

    public int playerScore;
}


public class SaveScript : MonoBehaviour
{
    public static SaveScript Instance;

    public Data data = new Data();

    private string filePath;

    void Awake()
    {
        // Singleton guard (optional but common)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // optional

        filePath = Path.Combine(Application.persistentDataPath, "data.json");
    }

    void Start()
    {
        Load();   // attempt auto-load on start
    }

    public void Save()
    {
        if (data == null)
        {
            Debug.LogError("Missing Data");
            return;
        }

        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(filePath, json);

        Debug.Log("saved");
    }

    public void Load()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Save file not found. Creating new data.");
            data = new Data();
            Save(); // write default file so future loads succeed
            return;
        }

        string json = File.ReadAllText(filePath);
        data = JsonUtility.FromJson<Data>(json);


    }


}
