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
        StartCoroutine(LoadWithDelay(_data)); // �ӳټ���
    }

    private void LoadCheckPoint(GameData _data)
    {
        //checkpoints = FindObjectsOfType<CheckPoint>();

        //foreach (CheckPoint checkpoint in checkpoints)
        //{
        //    checkpoint.DeactivateCheckpoint(); // ��Ҫʵ�ִ˷���
        //}

        // ���ݱ������������״̬
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (CheckPoint checkpoint in checkpoints)
            {
                //if (checkpoint.id == pair.Key)
                //{
                //    if (pair.Value)
                //        checkpoint.ActivateCheckpoint();
                //    else
                //        checkpoint.DeactivateCheckpoint(); // ��ʽ����δ����
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
        yield return new WaitForSeconds(0.5f); // �ȴ�0.1��
        
        LoadCheckPoint(_data);
        LoadClosestCheckpoint(_data);
        LoadLostCurrency(_data);
    }

    public void SaveData(ref GameData _data)
    {
        SaveLostCurrency(_data);


        if(FindClosestCheckpoint()!=null)
           _data.closestCheckpointId = FindClosestCheckpoint().id;//�ҵ�����ļ���
       
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

    private CheckPoint FindClosestCheckpoint()//�ҵ�����ļ���
    {
        float closestDistance = Mathf.Infinity;//������
        CheckPoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)//�������еļ���
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position);//������Һͼ���֮��ľ���

            if (distanceToCheckpoint < closestDistance && checkpoint.activationStatus == true)//�������С����������Ҽ��㼤��
            {
                closestDistance = distanceToCheckpoint;//�����������
                closestCheckpoint = checkpoint;//�����������
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
