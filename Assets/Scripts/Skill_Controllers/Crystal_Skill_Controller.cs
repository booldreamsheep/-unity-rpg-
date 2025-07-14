using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private float crystalExitTimer;
    public void SetupCrystal(float _crystalDuration)
    {
        crystalExitTimer = _crystalDuration;
    }
    private void Update()
    {
        crystalExitTimer -= Time.deltaTime;
        if (crystalExitTimer < 0)
            SelfDestory();
    }
    public void SelfDestory() => Destroy(gameObject);
}
