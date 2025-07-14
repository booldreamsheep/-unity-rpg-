using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>()!=null||collision.GetComponent<Enemy>()!=null)
        {
            collision.GetComponent<CharacterStats>().KillEntity();
        }
        else
            Destroy(collision.gameObject);
    }
}
