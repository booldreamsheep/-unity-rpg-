using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialugueManager: MonoBehaviour
{
    public static DialugueManager instance;

    public GameObject dialuguePanel;
    public TextMeshProUGUI dialugueText,nameText;

    [TextArea(1,3)]
    public string[] dialugueLines;
    [SerializeField]private int currentLine;

    Player player;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
            
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();

        dialugueText.text = dialugueLines[currentLine];
    }

    private void Update()
    {
        if (dialuguePanel.activeInHierarchy)
        {
            if (Input.GetMouseButtonUp(0))
            {
                currentLine++;
                if (currentLine < dialugueLines.Length)
                {
                    CheckName();
                   
                    dialugueText.text = dialugueLines[currentLine];
                }
                else
                {
                    dialuguePanel.SetActive(false);
                    if (player != null)
                    {
                        player.SetDialogueState(false);
                    }

                }
                    
            }
        }
       
    }

    public void ShowDialogue(string[] newlines)
    {
        dialugueLines = newlines;
        currentLine = 0;

        CheckName();

        dialugueText.text = dialugueLines[currentLine];
        dialuguePanel.SetActive(true);
       
        if(player!= null)
        {
            player.SetDialogueState(true);
            player.ZeroVelocity();
        }


    }

    private void CheckName()
    {
        if (dialugueLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialugueLines[currentLine].Replace("n-", "");
            currentLine++;
        }
    }

}
