using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public bool activationStatus;
    private SpriteRenderer spriteRenderer;


    [Header("ÊÓ¾õÐ§¹û")]
    public Sprite activeSprite;
    public Sprite inactiveSprite;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        activationStatus = false;
        anim = GetComponent<Animator>();

        UpdateVisual();
    }

    [ContextMenu("Generate checkpoint ID")]
    private void GenerteId()
    {
        id = System.Guid.NewGuid().ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>()!=null)
        {
            ActivateCheckpoint();

        }
    }

    public void ActivateCheckpoint()
    {
        if(activationStatus==false)
            AudioManager.instance.PlaySFX(5, transform);

        activationStatus = true;
        anim.SetBool("active", true);

        UpdateVisual();
    }

    public void DeactivateCheckpoint()
    {
        activationStatus = false;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        spriteRenderer.sprite = activationStatus ? activeSprite : inactiveSprite;
    }
}

