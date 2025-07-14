using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoatCurrencyController: MonoBehaviour
{
    public int currency;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            PlayerManager.instance.currency += currency;//ºÒ»°¡ÈªÍ
            Destroy(gameObject);
        }
    }

}
