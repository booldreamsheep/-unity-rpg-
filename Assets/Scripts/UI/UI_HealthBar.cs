using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private CharacterStats stats;
    private RectTransform myTransform;
    private Slider slider;
    private void Start()
    {
        myTransform=GetComponent<RectTransform>();
        entity=GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        stats = GetComponentInParent<CharacterStats>();

        entity.onFlipped += FilpUI;
        stats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }


    private void UpdateHealthUI()
    {
        slider.maxValue=stats.GetMaxHealthValue();
        slider.value = stats.currentHealth;
    }

    private void FilpUI()
    {
        myTransform.Rotate(0, 180, 0);
    }
    private void OnDisable()
    {
        entity.onFlipped -= FilpUI;
        stats.onHealthChanged-= UpdateHealthUI;
    }

}
