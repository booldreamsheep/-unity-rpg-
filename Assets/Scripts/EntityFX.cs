using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("Aliment colors")]
    [SerializeField] private Color[] chillcolor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Ailment particles")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFx;
    [SerializeField] private GameObject criticalHitFx;


    [Space]
    [SerializeField] private ParticleSystem dustFx;

    private void Start()
    {
        sr=GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }
    public void MakeTransprent(bool isClear)
    {
        if (isClear)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    private IEnumerable FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;

        sr.color=Color.white;
        yield return new WaitForSeconds(.2f);

        sr.color = currentColor;
        sr.material = originalMat;

    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }
    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;


        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }
    public void ShockFxFor(float _seconds)
    {
        shockFX.Play();

        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void ChillFxFor(float _seconds)
    {
        chillFX.Play();

        InvokeRepeating("ChillColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void IgniteFxFor(float _seconds)
    {
        igniteFX.Play();

        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    private void IgniteColorFx()
    {
        if(sr.color!=igniteColor[0]) 
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }
    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }
    private void ChillColorFx()
    {
        if (sr.color != chillcolor[0])
            sr.color = chillcolor[0];
        else
            sr.color = chillcolor[1];
    }


    public void CreateHitFx(Transform _target,bool _critical)
    {

        float zRotation = Random.Range(-90, 90);
        float xPosition=Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        Vector3 hitFxRotaion=new Vector3(0, 0, zRotation);


        GameObject hitPrefab = hitFx;

        if (_critical)
        {
            hitPrefab = criticalHitFx;

            float yRotation = 0;
            zRotation = Random.Range(-45,45);

            if (GetComponent<Entity>().facingDir == -1)
            {
                yRotation = 180;
            }
            hitFxRotaion=new Vector3(0, yRotation, zRotation);
        }

        GameObject newHitFx = Instantiate(hitPrefab, _target.position+new Vector3(xPosition,yPosition),Quaternion.identity);

        //if (_critical == false)
        //    newHitFx.transform.Rotate(new Vector3(0, 0, zRotation));
        //else
        //    newHitFx.transform.localScale = new Vector3(GetComponent<Entity>().facingDir, 1, 1);


        newHitFx.transform.Rotate(hitFxRotaion);

        Destroy(newHitFx, .5f);
    }

    public void PlayDustFX()
    {
        if(dustFx != null)
        {
            dustFx.Play();
        }
    }

}
