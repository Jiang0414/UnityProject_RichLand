using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Fx : MonoBehaviour
{
    public float closeTime;
    public bool closeDelay;
    public PlayerCtrl player;
    public bool isDone;
    private void OnEnable()
    {
        if (closeDelay)
            StartCoroutine(CloseDelay());
    }

    IEnumerator CloseDelay()
    {
        yield return new WaitForSeconds(closeTime);
        Close();
    }
    public void Close()
    {
        player.uiManager.isActDone = isDone;
        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
    }
}
