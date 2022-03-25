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
        infoTxt.text = "第" + roundManager.roundCount + "回合";
        infoTxt2.text = "輪到玩家_" + roundManager.switchPlayer;
    }
}
