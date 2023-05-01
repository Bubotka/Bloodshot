using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [SerializeField] private string _fileName;
    [SerializeField] private bool _encryptData;

    private GameData _gameData;
    private List<ISaveManager> _saveManagers;
    private FileDataHandler _dataHandler;

    [ContextMenu("Delete save file")]
    public void DeleteSavedData()
    {
        _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _encryptData);
        _dataHandler.Delete();
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this; 
    }

    private void Start()
    {
        _dataHandler = new FileDataHandler(Application.persistentDataPath,_fileName, _encryptData);
        _saveManagers = FindAllSaveManager();

        LoadGame();   
    }

    public void NewGame() 
    {
        _gameData = new GameData();

    }

    public void LoadGame()
    {
        _gameData = _dataHandler.Load();

        if (this._gameData == null)
            NewGame();

        foreach(ISaveManager saveManager in _saveManagers)
        {
            saveManager.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        foreach(ISaveManager saveManager in _saveManagers)
        {
            saveManager.SaveData(ref _gameData);
        }

        _dataHandler.Save(_gameData);

    }

    private void OnApplicationQuit() 
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManager()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }

    public bool  HasSavedData()
    {
        if (_dataHandler.Load() != null)
        {
            return true;
        }

        return false;
    }
}
