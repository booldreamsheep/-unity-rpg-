using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField]UI_FadeScreen fadeScreen;

    private void Start()
    {
        if(SaveManager.instance.HasSaveData()==false)
            continueButton.SetActive(false);
    }
    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1f));
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
       
        StartCoroutine(LoadSceneWithFadeEffect(1f));
    }

    public void ExitGame()
    {
        //Application.Quit();
        Debug.Log("Game Exited");
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay)

    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneName);

    }

}
