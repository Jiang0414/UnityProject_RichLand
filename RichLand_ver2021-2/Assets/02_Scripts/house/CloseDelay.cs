using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDelay : MonoBehaviour
{
    public float delayTime;
    private GameObject fx_bomb;
    private Material material;

    public float speed;
    private float fadeValue;
    private bool none;

    private void Awake()
    {
        if(transform.childCount > 0)
        {
            fx_bomb = transform.Find("housebomb").gameObject;
        }
        material = GetComponent<MeshRenderer>().material;
    }
    private void OnEnable()
    {
        if (transform.childCount > 0 && fx_bomb == null)
        {
            fx_bomb = transform.Find("housebomb").gameObject;
        }
        else if (transform.childCount < 1 && fx_bomb != null)
        {
            fx_bomb = null;
        }
        if (fx_bomb != null)
        {
            StartCoroutine(DelayClose());
        }

        if (material != null)
        {
            none = false;
            material.SetFloat("_TransparencyLevel_", 1.2f);
            fadeValue = material.GetFloat("_TransparencyLevel_");
        }
    }
    private void FixedUpdate()
    {
        FadeIn();
    }

    IEnumerator DelayClose()
    {
        fx_bomb.SetActive(true);
        yield return new WaitForSeconds(delayTime);
        fx_bomb.SetActive(false);
    }

    public void FadeIn()
    {        
        if (!none)
        {
            fadeValue = Mathf.Lerp(fadeValue, -0.2f, speed * Time.deltaTime);
            material.SetFloat("_TransparencyLevel_", fadeValue);
            if (Mathf.Abs(fadeValue - 0.2f) <= 0.01f)
            {                
                fadeValue = -0.2f;
                none = true;
            }
        }
    }
    public void FadeOut()
    {
        if (!none)
        {
            fadeValue = Mathf.Lerp(fadeValue, 1.2f, speed * Time.deltaTime);
            material.SetFloat("_TransparencyLevel_", fadeValue);
            if (Mathf.Abs(fadeValue - 1.2f) <= 0.01f)
            {
                fadeValue = 1.2f;
                none = true;
            }
        }
    }
}
