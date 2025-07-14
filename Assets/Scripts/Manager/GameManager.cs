using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveManager
{
    public static GameManager instance;

    private Transform player;

    [SerializeField] private CheckPoint[] checkpoints;
    [SerializeField]private string closestCheckpointId;

    [Header("Lost currency")]
    [SerializeField]private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;
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
        checkpoints=FindObjectsOfType<CheckPoint>();
    }

    private void Start()
    {
      player=PlayerManager.instance.player.transform;
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame(); 
        Scene scene=SceneManager.GetActiveScene(); //Get the active scene
        SceneManager.LoadScene(scene.name); //Reload the scene
    }

    public void LoadData(GameData _data)
    {   
        StartCoroutine(LoadWithDelay(_data)); // 延迟加载
    }

    private void LoadCheckPoint(GameData _data)
    {
        //checkpoints = FindObjectsOfType<CheckPoint>();

        //foreach (CheckPoint checkpoint in checkpoints)
        //{
        //    checkpoint.DeactivateCheckpoint(); // 需要实现此方法
        //}

        // 根据保存的数据设置状态
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (CheckPoint checkpoint in checkpoints)
            {
                //if (checkpoint.id == pair.Key)
                //{
                //    if (pair.Value)
                //        checkpoint.ActivateCheckpoint();
                //    else
                //        checkpoint.DeactivateCheckpoint(); // 显式设置未激活
                //}

                if (checkpoint.id == pair.Key && pair.Value == true)
                {
                    checkpoint.ActivateCheckpoint();
                }
            }
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if (lostCurrencyAmount > 0)
        {
            GameObject newlostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newlostCurrency.GetComponent<LoatCurrencyController>().currency= lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(0.5f); // 等待0.1秒
        
        LoadCheckPoint(_data);
        LoadClosestCheckpoint(_data);
        LoadLostCurrency(_data);
    }

    public void SaveData(ref GameData _data)
    {
        SaveLostCurrency(_data);


        if(FindClosestCheckpoint()!=null)
           _data.closestCheckpointId = FindClosestCheckpoint().id;//找到最近的检查点
       
        _data.checkpoints.Clear();

        foreach (CheckPoint checkpoint in checkpoints)
        {
            //Debug.Log($"Saved Checkpoint: {checkpoint.id}, Status: {checkpoint.activationStatus}");
            _data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
        }
    }

    private void SaveLostCurrency(GameData _data)
    {
        _data.lostCurrencyX =player.position.x;
        _data.lostCurrencyY = player.position.y;
        _data.lostCurrencyAmount = lostCurrencyAmount;
    }

    private void LoadClosestCheckpoint(GameData _data)
    {
        if(_data.closestCheckpointId == null)
            return;

        closestCheckpointId = _data.closestCheckpointId;

        foreach (CheckPoint checkpoint in checkpoints)
        {
            if (closestCheckpointId == checkpoint.id)
            {
                player.position = checkpoint.transform.position;
            }
        }
    }

    private CheckPoint FindClosestCheckpoint()//找到最近的检查点
    {
        float closestDistance = Mathf.Infinity;//正无穷
        CheckPoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)//遍历所有的检查点
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position);//计算玩家和检查点之间的距离

            if (distanceToCheckpoint < closestDistance && checkpoint.activationStatus == true)//如果距离小于最近距离且检查点激活
            {
                closestDistance = distanceToCheckpoint;//更新最近距离
                closestCheckpoint = checkpoint;//更新最近检查点
            }

        }
        return closestCheckpoint;
    }


    public void PauseGame(bool _pause)
    {
        if (_pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }


}
