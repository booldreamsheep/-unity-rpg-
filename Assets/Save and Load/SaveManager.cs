using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public GameData gameData;
    [SerializeField]private string fileName;
    [SerializeField] private bool encryptData;

    private List<ISaveManager> saveManagers;

    private FileDataHandler dataHandler;
    


    [ContextMenu("Delete Save file")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
         dataHandler.Delete();
    }

    private void Awake()
    {
        if (instance != null)
        {
           Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }


   private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);

        saveManagers = FindAllSaveManagers();

        LoadGame();
    }
    public void NewGame()
    {
        gameData = new GameData();
    }
    public void LoadGame()
    {

        gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("NO SAVED DATA FOUND");
            NewGame();
        }
       foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
        //load game data from data handler
        
        //gameData = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString("GameData"));
        //Debug.Log(gameData.currency);
    }

    public void SaveGame()
    {

        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
      
        //PlayerPrefs.SetString("GameData", JsonUtility.ToJson(gameData));
        //PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
       IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }

    public bool HasSaveData()
    {
        if(dataHandler.Load() != null)
        {
            return true;
        }
        return false;
    }

}
