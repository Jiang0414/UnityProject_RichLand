using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corner : MonoBehaviour
{
    public int startMoney, vacationMoney, UrbanMoney;
    private RoundManager round;
    private UIManager uIManager;
    private static Corner _instance;
    private Corner()
    {
        if (uIManager == null)
        {
            uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
            round = GameObject.FindGameObjectWithTag("RoadManager").GetComponent<RoundManager>();

            UrbanMoney = (int)ReadGameValue.Instance.GetValue(51);
            startMoney = (int)ReadGameValue.Instance.GetValue(52);
            vacationMoney = (int)ReadGameValue.Instance.GetValue(53);
        }
    }
    public static Corner Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Corner();
            }
            return _instance;
        }
    }

    public void CornerEvent(int pos, PlayerCtrl player)
    {
        switch (pos)
        {
            case (0):
                {
                    Corner_Start(player);
                    break;
                }
            case (8):
                {
                    Corner_Vacation(player);
                    break;
                }
            case (16):
                {
                    Corner_UrbanRenewal(player);
                    break;
                }
            case (24):
                {
                    Corner_MRT(player);
                    break;
                }
        }
    }

    #region 起始點功能
    public void Corner_Start(PlayerCtrl player)
    {
        player.SetHappy();
        player.PlayerInfo.Assets += startMoney;
        player.uiManager.isActDone = true;
    }
    #endregion

    #region 度假中心功能
    private void Corner_Vacation(PlayerCtrl player)
    {
        if (player.toMove)
        {
            uIManager.uiManager_pageCtrl.SwitchToPage("Game_Cutscene");
            uIManager.uiManager_pageCtrl.GetPage("Game_Cutscene").GetComponent<PageCtrl>().SwitchToPage("TravelPay");
            uIManager.audioSource.clip = PlayAudio.audiosList[19];
            uIManager.audioSource.Play();
            player.SetHappy();
            player.toMove = false;
            player.dice1.gameObject.SetActive(false);
            player.dice2.gameObject.SetActive(false);
            player.dice1.transform.position = player.dice1.inPos;
            player.dice2.transform.position = player.dice2.inPos;
            player.dice1.gameObject.SetActive(true);
            player.canThrow = true;
            player.uiManager.btn_RollDice.gameObject.SetActive(true);
            player.playerState = PlayerCtrl.PlayerState.ThrowTheDice;
            return;
        }
        else
        {
            if (player.PlayerInfo.Assets > player.diceNumber * vacationMoney)
            {
                player.SetHappy();
                player.PlayerInfo.Assets -= player.diceNumber * vacationMoney;
                player.uiManager.isActDone = true;
                player.passCount += 1;
                player.playerState = PlayerCtrl.PlayerState.RoadAction;
            }
            else
            {
                Bankrupt(player, player.diceNumber * vacationMoney);
                if (player.toSoldHouse) return;
                player.SetSad();
                player.toSoldHouse = true;
                uIManager.ui_BuyGround.SetActive(true);
                floorBtn selectUI = uIManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("SelectGround_Urban").GetComponentInChildren<floorBtn>();
                selectUI.player = player;
                selectUI.payMoney = player.diceNumber * vacationMoney;
                selectUI.btnState = floorBtn.BtnState.isSold;
                uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask.SetActive(false);
                uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask2.SetActive(true);
                uIManager.ui_BuyGround.GetComponent<PageCtrl>().SwitchToPage("SelectGround_Urban");
            }
        }
    }
    private void Bankrupt(PlayerCtrl player, int payMoney)
    {
        int soldMoney = 0;
            foreach (var ground in player.myGounds)
            {
                soldMoney += ground.buildPrice;
            }

        if (payMoney < soldMoney) return;
        player.otherPlayer.PlayerInfo.Assets += player.PlayerInfo.Assets;
        player.PlayerInfo.Assets = 0;
        player.playerState = PlayerCtrl.PlayerState.Lose;
        round.roundTurn = RoundManager.RoundCount.GameEnd;
    }
    #endregion

    #region 都市更新功能
    private void Corner_UrbanRenewal(PlayerCtrl player)
    {
        if (player.toUrban) return;
        if (!player.uiManager.ui_BuyGround.activeInHierarchy && CanUrban(player)) //UrbanMoney
        {
            floorBtn selectUI = player.uiManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("SelectGround_Urban").GetComponentInChildren<floorBtn>();
            player.SetHappy();
            selectUI.player = player;
            selectUI.btnState = floorBtn.BtnState.isUrban;
            player.toUrban = true;
            player.OpenGroundUI("SelectGround_Urban");
            player.uiManager.ui_BuyGround.GetComponent<PageCtrl>().Mask.SetActive(false);
            player.uiManager.ui_BuyGround.GetComponent<PageCtrl>().Mask2.SetActive(true);
            return;
        }
        else if (!player.uiManager.ui_BuyGround.activeInHierarchy && !CanUrban(player))
        {
            if (player.myGounds.Count <= 0)
            {
                uIManager.TipAndClose(player, "NoMoney", "未擁有土地", 1.2f);
                return;
            }
            else if (player.PlayerInfo.Assets < UrbanMoney)
            {
                uIManager.TipAndClose(player, "NoMoney", "存款不足", 1.2f);
                return;
            }
        }
    }
    public void Btn_Urban(PlayerCtrl player, Ground_Info ground)
    {
        uIManager.audioSource.clip = PlayAudio.audiosList[13];
        uIManager.audioSource.Play();
        player.SetHappy();
        player.toUrban = false;
        ground.isCelebration = true;
        ground.BuildedBill(ground.Info.HouseLv);
        ground.buildPrice += UrbanMoney;
        ground.SetTolls(ground.Info.HouseLv);
        ground.transform.Find("crown").gameObject.SetActive(true);
        ground.transform.Find("light").gameObject.SetActive(true);

        player.fx_PayCoin2.SetActive(true);
        player.PlayerInfo.Assets -= UrbanMoney;
        player.uiManager.ui_BuyGround.GetComponent<PageCtrl>().Mask2.SetActive(false);
        player.uiManager.ui_BuyGround.SetActive(false);
        player.uiManager.isActDone = true;
    }

    private bool CanUrban(PlayerCtrl player)
    {
        bool canU = false;
        if (player.myGounds.Count <= 0 && player.PlayerInfo.Assets < UrbanMoney)
        {
            canU = false;
        }
        else if (player.myGounds.Count > 0 && player.PlayerInfo.Assets >= UrbanMoney)
        {
            foreach (var ground in player.myGounds)
            {
                if (!ground.isCelebration)
                {
                    canU = true;
                }
            }
        }
        return canU;
    }
    #endregion

    #region 高速鐵路功能
    private void Corner_MRT(PlayerCtrl player)
    {
        if (!player.uiManager.ui_BuyGround.active && !player.toMRT)
        {
            player.uiManager.isActDone = true;
            player.toMRT = true;
            player.SetHappy();
            player.uiManager.ui_BuyGround.GetComponent<PageCtrl>().Mask2.SetActive(false);
            player.uiManager.ui_BuyGround.SetActive(false);
            return;
        }
        else if (!player.uiManager.ui_BuyGround.active && player.toMRT)
        {
            player.toMRT = false;
            floorBtn selectUI = player.uiManager.ui_BuyGround.GetComponent<PageCtrl>().Pages[3].GetComponentInChildren<floorBtn>();
            selectUI.player = player;
            selectUI.btnState = floorBtn.BtnState.isMRT;
            player.SetHappy();
            player.OpenGroundUI("SelectGround_Urban");
            player.uiManager.ui_BuyGround.GetComponent<PageCtrl>().Mask.SetActive(false);
            player.uiManager.ui_BuyGround.GetComponent<PageCtrl>().Mask2.SetActive(true);
            return;
        }
    }

    public void Btn_MRT(PlayerCtrl player, Ground_Info ground)
    {
        uIManager.audioSource.clip = PlayAudio.audiosList[18];
        uIManager.audioSource.Play();
        player.SetHappy();
        player.toMRT = false;
        player.road_Dice = ground.groundID;
        player.uiManager.ui_BuyGround.GetComponent<PageCtrl>().Mask2.SetActive(false);
        player.uiManager.ui_BuyGround.SetActive(false);
        player.playerState = PlayerCtrl.PlayerState.Move;
    }
    #endregion
}
