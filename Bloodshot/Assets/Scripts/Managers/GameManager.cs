using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    private Transform _player;

    [SerializeField] private CheckPoint[] _checkPoints;
    [SerializeField] private string _closestCheckpointLoaded;

    public static GameManager _instance;

    [Header("Lost currency")]
    [SerializeField] private GameObject _lostCurrencyPrefab;
    [SerializeField] private float _lostCurrencyX;
    [SerializeField] private float _lostCurrencyY;

    public int LostCurrencyAmount;


    private void Awake()
    {
        if (_instance != null)
            Destroy(_instance.gameObject);
        else
            _instance = this;
    }

    private void Start()
    {
        _checkPoints = FindObjectsOfType<CheckPoint>();

        _player = PlayerManager.Instance.Player.transform;
    }

    public void RestartScene()
    {
        SaveManager.Instance.SaveGame();

        Scene scene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData data) => StartCoroutine(LoadWithDelay(data));

    private void LoadCheckPoints(GameData data)
    {
        foreach (KeyValuePair<string, bool> pair in data.Checkpoints)
        {
            foreach (CheckPoint checkPoint in _checkPoints)
            {
                if (checkPoint.Id == pair.Key && pair.Value == true)
                    checkPoint.ActivateCheckpoint();
            }
        }
    }

    private void LoadLostCurrency(GameData data)
    {
        LostCurrencyAmount = data.LostCurrencyAmount;
        _lostCurrencyX = data.LostCurrencyX;
        _lostCurrencyY = data.LostCurrencyY;

        if (LostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(_lostCurrencyPrefab, new Vector3(_lostCurrencyX, _lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().Currency = LostCurrencyAmount;
        }

        LostCurrencyAmount = 0;
    }

    private IEnumerator LoadWithDelay(GameData data)
    {
        yield return new WaitForSeconds(0.1f);

        LoadCheckPoints(data);
        LoadClosestCheckpoint(data);
        LoadLostCurrency(data);
    }

    public void SaveData(ref GameData data)
    {
        data.LostCurrencyAmount = LostCurrencyAmount;

        data.LostCurrencyY = _player.position.y;

        data.LostCurrencyX = _player.position.x;

        if (FindClosestCheckpoint() != null)
            data.ClosestCheckpointId = FindClosestCheckpoint().Id;

        data.Checkpoints.Clear();

        foreach (CheckPoint checkpoint in _checkPoints)
        {
            data.Checkpoints.Add(checkpoint.Id, checkpoint.ActivationtStatus);
        }
    }
    private void LoadClosestCheckpoint(GameData data)
    {
        if (data.ClosestCheckpointId == null)
            return;

        _closestCheckpointLoaded = data.ClosestCheckpointId;

        foreach (CheckPoint checkpoint in _checkPoints)
        {
            if (_closestCheckpointLoaded == checkpoint.Id)
                _player.position = checkpoint.transform.position;
        }
    }

    private CheckPoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        CheckPoint closestCheckpoint = null;

        foreach (var cheackpoint in _checkPoints)
        {
            float distanceToCheckPoint = Vector2.Distance(_player.position, cheackpoint.transform.position);

            if (distanceToCheckPoint < closestDistance && cheackpoint.ActivationtStatus == true)
            {
                closestDistance = distanceToCheckPoint;
                closestCheckpoint = cheackpoint;
            }
        }

        return closestCheckpoint;
    }
}
