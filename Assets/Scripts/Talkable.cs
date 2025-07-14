using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour
{
    public GameObject Button;
    public GameObject talkPanl;

    [SerializeField] private bool isEntered;
    [TextArea(1,3)]
    public string[] dialugueLines;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isEntered = true;
            
            Button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isEntered = false;
            Button.SetActive(false);
        }
    }

    private void Update()
    {
        if (isEntered && Input.GetKeyDown(KeyCode.T) && DialugueManager.instance.dialuguePanel.activeInHierarchy==false && Button.activeSelf)
        {
            
            DialugueManager.instance.ShowDialogue(dialugueLines);
        }
    }

}
