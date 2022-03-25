using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IgnoreTransparent : MonoBehaviour
{
    private Image image;
    public float transparent;

    private void Awake()
    {
        image = GetComponent<Image>();

        image.alphaHitTestMinimumThreshold = transparent;
    }

    public void Test()
    {
        Debug.Log("55555");
    }
}
