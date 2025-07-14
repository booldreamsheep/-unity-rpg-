using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEnter : MonoBehaviour
{
    public Transform backDoor;

    private bool isDoor;
    private Transform playertransform;

    public GameObject Button;

    void Start()
    {
        playertransform = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDoor && Input.GetKeyDown(KeyCode.E))
        {
            playertransform.position = backDoor.position;
            //playertransform.position = new Vector2(backDoor.position.x, backDoor.position.y);
            //playertransform.position = backDoor.position;
            //playertransform.position = new Vector2(backDoor.position.x, backDoor.position.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isDoor = true;

            Button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isDoor = false;
            Button.SetActive(false);
        }
    }

}
