using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fx_DelayCLose : MonoBehaviour
{
    public float delayTime;
    private void OnEnable()
    {
        StartCoroutine(DelayClose());
    }
    IEnumerator DelayClose()
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
    }
}
