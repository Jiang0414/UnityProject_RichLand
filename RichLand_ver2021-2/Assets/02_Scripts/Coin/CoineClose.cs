using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoineClose : MonoBehaviour
{
    public float time;
    private void OnEnable()
    {
        StartCoroutine(CloseDelay());
    }
    IEnumerator CloseDelay()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
