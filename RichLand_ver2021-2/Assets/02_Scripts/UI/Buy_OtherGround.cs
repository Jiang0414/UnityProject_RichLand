using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buy_OtherGround : MonoBehaviour
{
    private UIManager uIManager;
    private RoundManager round;
    private Ground_Info ground_Info;

    private Text txt_Name, txt_TotalPrice, txt_Discount, txt_FinalPrice;
    private int groundValue, finalPrice;
    public bool isBuyOrigin;
    public int discount;
    public PlayerCtrl player;

    private void OnEnable()
    {
        if (player == null) return;
        if (round != null && !player.groundInfo.isNotRoad)
        {
            SetUI();
        }
    }

    private void Awake()
    {
        uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        round = GameObject.FindGameObjectWithTag("RoadManager").GetComponent<RoundManager>();
        txt_Name = transform.Find("Txt_Name").Find("Txt_Name").GetComponent<Text>();
        txt_TotalPrice = transform.Find("Txt_Info").Find("Txt_BuyPrice").Find("Txt_BuyPrice").GetComponent<Text>();
        txt_Discount = transform.Find("Txt_Info").Find("Txt_Discount").Find("Txt_Discount").GetComponent<Text>();
        txt_FinalPrice = transform.Find("Txt_Info").Find("Btns").Find("Btn_Buy").Find("Txt_FinalPrice").GetComponent<Text>();
        
        SetUI();
    }
    public void Btn_CloseUI(int audioclip) //UI按鈕_關閉
    {
        isBuyOrigin = false;
        uIManager.audioSource.clip = PlayAudio.audiosList[audioclip];
        uIManager.audioSource.Play();
        ground_Info.SetTolls(ground_Info.Info.HouseLv);
        uIManager.ui_BuyGround.SetActive(false);
        ground_Info.areaBonus.SetAreaBonus(player, player.otherPlayer);
        ground_Info = null;
        player.discount = 0;
        if (player.isHaveCard(19))
        {
            player.Card(19).isOpen = false;
            player.Card(19).card_Fx.GetComponent<Card_Fx>().Close();
        }
        if (player.isHaveCard(6))
        {
            player.Card(6).isEnd = true;
        }
        round.cardSubject.setState(0);
        uIManager.isActDone = true;
    }
    public void Btn_BuyGound()
    {
        player.PlayerInfo.Assets -= finalPrice;
        player.PlayerInfo.TotalAssets += groundValue;
        player.otherPlayer.PlayerInfo.Assets += groundValue;
        ground_Info.owner = player.PlayerInfo.ID;
        ground_Info.Info.Owner = player.PlayerInfo.ID;
        player.myGounds.Add(ground_Info);
        player.otherPlayer.myGounds.Remove(ground_Info);
        ground_Info.SetHouseLv();
        player.SetHappy();
        player.otherPlayer.SetSad();
        player.CountTotalAsset();
        player.otherPlayer.CountTotalAsset();
        player.fx_PayCoin.SetActive(true);
        player.discount = 0;
        if (player.isHaveCard(19))
            player.Card(19).isEnd = true;
        Btn_CloseUI(3);
    }

    public void SetUI()
    {
        ground_Info = player.groundInfo;
        if (!isBuyOrigin)
        {
            groundValue = (int)(ground_Info.buildPrice * ground_Info.soldPrice);
        }
        else
        {
            groundValue = ground_Info.buildPrice;
        }
        discount = (int)(groundValue * player.discount);
        txt_Name.text = "是否收購土地" + ground_Info.Info.ID + "?";
        txt_TotalPrice.text = TextThousand.Instance.SetText(groundValue);
        txt_Discount.text = TextThousand.Instance.SetText(discount);
        finalPrice = groundValue - discount;
        txt_FinalPrice.text = TextThousand.Instance.SetText(finalPrice);
    }
}
