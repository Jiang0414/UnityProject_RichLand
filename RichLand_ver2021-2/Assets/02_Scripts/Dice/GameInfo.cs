using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInfo : MonoBehaviour
{
    public Text infoTxt, infoTxt2;
    public RoundManager roundManager;

    private void Update()
    {
        infoTxt.text = "��" + roundManager.roundCount + "�^�X";
        infoTxt2.text = "���쪱�a_" + roundManager.switchPlayer;
    }
}
