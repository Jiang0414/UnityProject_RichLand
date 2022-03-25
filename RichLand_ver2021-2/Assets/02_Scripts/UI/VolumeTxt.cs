using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeTxt : MonoBehaviour
{
    private Text txt_Volume;
    public Slider slider_Volume;
    private int value , totle;
    private void Start()
    {
        txt_Volume = GetComponent<Text>();
        totle = ((int)(slider_Volume.maxValue - slider_Volume.minValue));
    }

    private void FixedUpdate()
    {
        TxtVolume();
    }

    public void TxtVolume()
    {
        value = (int)(((slider_Volume.value + totle) / totle) * 100);
        txt_Volume.text = value.ToString() + "%";
    }
}
