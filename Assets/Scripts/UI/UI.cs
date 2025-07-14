using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour,ISaveManager
{
    [Header("End Screen")]
    [SerializeField]private  UI_FadeScreen FadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]


    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject mInimapUI;

    public UI_ItemToolTip itemToolTip;
    public UI_statToolTip statToolTip;
    public UI_CraftWindow craftWindow;
    public UI_SkillToolTip skillToolTip;

    [SerializeField]private UI_VoluneSilde[] volumeSettings;


    private void Awake()
    {
        //skillTreeUI.gameObject.SetActive(true);
        //SwitchTo(skillTreeUI);//我们需要用这个来分配事件到技术树,在分配事件给技能脚本之前

        
        FadeScreen.gameObject.SetActive(true);

    }
    void Start()
    {
        SwitchTo(inGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        itemToolTip = GetComponentInChildren<UI_ItemToolTip>(true);

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKeyTo(characterUI);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchWithKeyTo(craftUI);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchWithKeyTo(skillTreeUI);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchWithKeyTo(optionsUI);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SwitchWithKeyTo(mInimapUI);
        }
    }

    public void SwitchTo(GameObject _menu)
    {


        for (int i = 0; i < transform.childCount; i++)
        {
           bool fadeScreen=transform.GetChild(i).GetComponent<UI_FadeScreen>()!=null;


            if(fadeScreen==false)
               transform.GetChild(i).gameObject.SetActive(false);
        }
        if (_menu != null) 
        {
            AudioManager.instance.PlaySFX(7,null);
            _menu.SetActive(true);
        }

        if(GameManager.instance!=null)
        {
            if(_menu == inGameUI)
            {
                GameManager.instance.PauseGame(false);
            }
            else
            {
                GameManager.instance.PauseGame(true);
            }
        }

    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for (int i= 0; i <transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>()==null)
                return;
        }
        SwitchTo(inGameUI);
    }


    public void SwitchOnEndScreen()
    {
        
        FadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutione());
    }

    IEnumerator EndScreenCorutione()
    {
        yield return new WaitForSeconds(1f);
        endText.SetActive(true);
        yield return new WaitForSeconds(1f);
        restartButton.SetActive(true);
    }

    public void RestartGameButton()=>GameManager.instance.RestartScene();

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string,float>pair in _data.volumeSettings)
        {
            foreach(UI_VoluneSilde item in volumeSettings)
            {
                if (item.parametr == pair.Key)
                {
                    item.LoadSlider(pair.Value);
                    //break;
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach(UI_VoluneSilde item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parametr,item.slider.value);
        }
    }

  

}
