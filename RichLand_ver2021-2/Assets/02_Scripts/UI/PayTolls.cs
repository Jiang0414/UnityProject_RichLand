using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayTolls : MonoBehaviour
{
    private UIManager uIManager;
    private RoundManager round;
    private Ground_Info ground_Info;
    private GameObject BG_Mask;
    public bool isPeace, isAngel;
    public PlayerCtrl player;

    private void OnEnable() //UI��l��
    {
        if (round != null && player != null)
        {
            if (player.isPay) return;
            round.cardSubject.setState(20);
            BG_Mask.SetActive(false);
            ground_Info = player.groundInfo;
            player.isPay = true;
            StartCoroutine(OpenAct());
        }
    }
    private void Awake()
    {
        uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        round = GameObject.FindGameObjectWithTag("RoadManager").GetComponent<RoundManager>();
        BG_Mask = uIManager.ui_BuyGround.transform.Find("BG_Panel").gameObject;
        ground_Info = player.groundInfo;
    }

    private void SkipPay()
    {
        isAngel = false;
        BG_Mask.SetActive(true);
        gameObject.SetActive(false);
        player.isPeace = false;
        player = null;
        ground_Info = null;
        uIManager.ui_BuyGround.SetActive(false);
        gameObject.SetActive(false);
        round.cardSubject.setState(0);
        uIManager.isActDone = true;
    }
    IEnumerator OpenAct()
    {
        yield return new WaitForSeconds(0.1f);
        isPeace = player.isPeace;
        StartCoroutine(DelayOpen());
    }
    IEnumerator DelayOpen()
    {
        if (!isPeace)
            round.cardSubject.setState(16);
        else if (isPeace || ground_Info.isPowerOff)
            SkipPay();
        yield return new WaitForSeconds(0.15f);
        isAngel = player.isPeace;
        if (player.otherPlayer.passCount < 1) //�ݭn��I�L���O
        {
            Pay_Tolls();
        }
        else if (player.otherPlayer.passCount > 0) //�L�ݤ�I�L���O
        {
            StartCoroutine(closeDelay(player, "traveling", 1.2f, false));
        }
    }
    public void Pay_Tolls()  //��I�L���O�P�w
    {        
        int _tolls = ground_Info.Info.Tolls;
        _tolls = player.isDevil ? (int)Math.Ceiling(_tolls * player.devilTolls) : _tolls;
        _tolls = _tolls < 0 ? 0 : _tolls;
        if (isPeace)
        {
            player.isPay = true;
            uIManager.isActDone = true;
            return;
        }
        if (player.PlayerInfo.Assets >= _tolls)  //��I�L���O
        {
            if (isAngel)
            {
                if (player.isHaveCard(16))
                    player.Card(16).isEnd = true;
                uIManager.CutScenes(player, "Card_Angel", 1f);
            }
            if (!isPeace && !isAngel && !ground_Info.isPowerOff && player.PlayerInfo.Assets >= _tolls)
            {
                player.PlayerInfo.Assets -= _tolls;
                player.otherPlayer.PlayerInfo.Assets += _tolls;
                //����ʧ@
                player.otherPlayer.SetHappy();
                player.SetSad();
            }
            if (ground_Info.Info.HouseLv < 4 && player.PlayerInfo.Assets >= ground_Info.buildPrice * ReadGameValue.Instance.GetValue(3))  //��I�ÿ�ܬO�_�ʶR
            {
                round.cardSubject.setState(19);
                if (!player.isHaveCard(19) && player.isHaveCard(6))
                {
                    StartCoroutine(BuyOriginPrice(player, 1.8f));
                }
                else if (!player.isHaveCard(6))
                {
                    uIManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("Buy_OtherGround").GetComponent<Buy_OtherGround>().player = player;
                    StartCoroutine(SwitchDelay(player, "paying", "Buy_OtherGround", 1.5f));
                }
            }
            else //��I�õL�k�ʶR
            {
                StartCoroutine(closeDelay(player, "paying", 1.5f, true));
            }
            return;
        }
        else if (player.PlayerInfo.Assets < _tolls && !isPeace && !isAngel) //�L�k��I�L���O
        {
            Debug.Log("�g�a" + ground_Info.groundID + "���L���O:" + _tolls + "�{��:"+ player.PlayerInfo.Assets);
            if (player.myGounds.Count > 0 && SoldCount() > _tolls) // �ܽ�в�
            {
                player.SetSad();
                player.otherPlayer.SetHappy();
                if (!player.toSoldHouse)
                {
                    StartCoroutine(SoldDelay(_tolls));
                    return;
                }
            }
            else// �}��
            {
                Bankrupt();
                round.roundTurn = RoundManager.RoundCount.GameEnd;
                player.isPay = true;
                uIManager.isActDone = true;
            }
        }
    }
    IEnumerator BuyOriginPrice(PlayerCtrl player, float sec)
    {
        player.fx_PayCoin.SetActive(true);
        yield return new WaitForSeconds(sec);
        player.fx_PayCoin.SetActive(false);
        round.cardSubject.setState(6);
    }
    IEnumerator SwitchDelay(PlayerCtrl player, string uiTip, string uiName, float sec)
    {
        if (!isPeace && !isAngel && !ground_Info.isPowerOff)
        {
            BG_Mask.SetActive(false);
            player.fx_PayCoin.SetActive(true);
        }
        uIManager.CutScenesSwitch(player, uiTip, uiName, sec);
        yield return new WaitForSeconds(sec + 0.1f);
        player.fx_PayCoin.SetActive(false);
        uIManager.ui_BuyGround.SetActive(false);
        ground_Info = null;
        player.isCutscenes = false;
        uIManager.isActDone = true;
        BG_Mask.SetActive(true);
        gameObject.SetActive(false);
    }


    IEnumerator closeDelay(PlayerCtrl player, string uiTip, float sec, bool ispay)
    {
        BG_Mask.SetActive(false);
        uIManager.CutScenes(player, uiTip, sec);
        if (ispay)
            player.fx_PayCoin.SetActive(true);
        yield return new WaitForSeconds(sec + 0.5f);
        if (ispay)
            player.fx_PayCoin.SetActive(false);
        BG_Mask.SetActive(true);
        uIManager.ui_BuyGround.SetActive(false);
        player.isCutscenes = false;
        uIManager.isActDone = true;
        ground_Info = null;
        gameObject.SetActive(false);
    }

    IEnumerator SoldDelay(int tolls)
    {
        yield return new WaitForSeconds(0.2f);
        player.toSoldHouse = true;
        floorBtn selectUI = uIManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("SelectGround_Urban").GetComponentInChildren<floorBtn>();
        selectUI.player = player;
        selectUI.btnState = floorBtn.BtnState.isSold;
        selectUI.payMoney = tolls;

        uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask.SetActive(false);
        uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask2.SetActive(true);
        uIManager.ui_BuyGround.GetComponent<PageCtrl>().SwitchToPage("SelectGround_Urban");
    }

    private int SoldCount()
    {
        int totalMoney = player.PlayerInfo.Assets;
        foreach (var ground in player.myGounds)
        {
            totalMoney += ground.Info.GroundValue;
        }
        Debug.Log("�{��+�Ҧ��в�:" + totalMoney);
        return totalMoney;
    }

    private void Bankrupt()
    {
        /*player.PlayerInfo.Assets = SoldCount();
        foreach (var ground in player.myGounds)
        {
            ground.Info.Owner = 0;
            ground.Info.HouseLv = 0;
            ground.SetHouseLv();
            ground.SetTolls(ground.Info.HouseLv);
            ground.SetSoldPrice();
        }
        player.otherPlayer.PlayerInfo.Assets += player.PlayerInfo.Assets;
        player.PlayerInfo.Assets = 0;*/
        player.playerState = PlayerCtrl.PlayerState.Lose;
    }
}
