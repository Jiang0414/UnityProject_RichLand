using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyGround : MonoBehaviour
{
    private UIManager uIManager;
    private RoundManager round;
    private Ground_Info ground_Info;
    private Text txt_BuildPrice, txt_Discount, txt_FinalPrice;
    private GameObject ui_TxtInfo;
    private GameObject uiTip_Build0, uiTip_Build1, uiTip_Build2, uiTip_Build3;
    private GameObject ui_CheckBuild0, ui_CheckBuild1, ui_CheckBuild2, ui_CheckBuild3;
    private GameObject ui_isDone0, ui_isDone1, ui_isDone2, ui_isDone3;
    private Text Txt_Msg;
    private GameObject ui_Buildbtns, ui_BuildMax, ui_Blockade;
    private bool CanBuild;
    private int buildPrice, discount, finalPrice, tolls, houseLv; //_buildedBill
    public bool isBuild_1, isBuild_2, isBuild_3;
    public PlayerCtrl player;
    public Text txt_Build0, txt_Build1, txt_Build2, txt_Build3, txt_Build4;

    private void OnEnable() //UI初始化
    {
        if (player == null) return;
        if (round != null && !player.groundInfo.isNotRoad)
        {
            ground_Info = player.groundInfo;
            houseLv = ground_Info.Info.HouseLv;
            SetTextPrice();
            SetCheckBuild();
            BuildPriceCount();
        }
    }
    private void Awake()
    {
        ui_TxtInfo = transform.Find("Txt_Info").gameObject;
        uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        round = GameObject.FindGameObjectWithTag("RoadManager").GetComponent<RoundManager>();
        Txt_Msg = transform.Find("Error_MSG").GetComponent<Text>();
        txt_BuildPrice = ui_TxtInfo.transform.Find("Txt_Build").Find("Txt_Build").GetComponent<Text>();
        txt_Discount = ui_TxtInfo.transform.Find("Txt_Discount").Find("Txt_Discount").GetComponent<Text>();
        txt_FinalPrice = ui_TxtInfo.transform.Find("Buy_Btn").transform.Find("Txt_FinalPrice").GetComponent<Text>();
        uiTip_Build0 = transform.Find("Builds").transform.Find("Build0").gameObject;
        uiTip_Build1 = transform.Find("Builds").transform.Find("Build1").gameObject;
        uiTip_Build2 = transform.Find("Builds").transform.Find("Build2").gameObject;
        uiTip_Build3 = transform.Find("Builds").transform.Find("Build3").gameObject;
        ui_CheckBuild0 = uiTip_Build0.transform.Find("Check_block").transform.Find("Build_0").gameObject;
        ui_CheckBuild1 = uiTip_Build1.transform.Find("Check_block").transform.Find("Build_1").gameObject;
        ui_CheckBuild2 = uiTip_Build2.transform.Find("Check_block").transform.Find("Build_2").gameObject;
        ui_CheckBuild3 = uiTip_Build3.transform.Find("Check_block").transform.Find("Build_3").gameObject;
        ui_isDone0 = uiTip_Build0.transform.Find("isDone").gameObject;
        ui_isDone1 = uiTip_Build1.transform.Find("isDone").gameObject;
        ui_isDone2 = uiTip_Build2.transform.Find("isDone").gameObject;
        ui_isDone3 = uiTip_Build3.transform.Find("isDone").gameObject;
        ui_Blockade = uiTip_Build3.transform.Find("Blockade").gameObject;
        ui_Buildbtns = transform.Find("Builds").gameObject;
        ui_BuildMax = transform.Find("BuildsMax").gameObject;
        //UI初始化
        ground_Info = player.groundInfo;
        houseLv = ground_Info.Info.HouseLv;
        discount = 15;
        SetCheckBuild();
        BuildPriceCount();
    }

    private void Update()
    {
        if (finalPrice > player.PlayerInfo.Assets)
        {
            Txt_Msg.text = "現金不足以支付建設費用";
        }
        else
        {
            Txt_Msg.text = "";
        }
    }
    public void SetTextPrice()
    {
        txt_Build0.text = TextThousand.Instance.SetText(ground_Info.Info.Build0);
        txt_Build1.text = TextThousand.Instance.SetText(ground_Info.Info.Build1);
        txt_Build2.text = TextThousand.Instance.SetText(ground_Info.Info.Build2);
        txt_Build3.text = TextThousand.Instance.SetText(ground_Info.Info.Build3);
        txt_Build4.text = TextThousand.Instance.SetText(ground_Info.Info.Build4);
    }
    public void SetCheckBuild()//設定建造選項初始值
    {
        if (player.groundInfo.Info.Owner == 0)
        {
            houseLv = 0;
            ui_Buildbtns.SetActive(true);
            ui_BuildMax.SetActive(false);
            ui_CheckBuild0.SetActive(true);
            ui_isDone0.SetActive(false);
            ui_isDone1.SetActive(false);
            ui_isDone2.SetActive(false);
            ui_isDone3.SetActive(false);
            uiTip_Build1.GetComponent<Image>().raycastTarget = true;
            uiTip_Build2.GetComponent<Image>().raycastTarget = true;
            uiTip_Build3.GetComponent<Image>().raycastTarget = true;
            SetBuild(1, ui_isDone1, ui_Blockade, ref isBuild_1, ui_CheckBuild1, uiTip_Build1);
            SetBuild(2, ui_isDone2, ui_Blockade, ref isBuild_2, ui_CheckBuild2, uiTip_Build2);
            CanBuild3();
            BuildPriceCount();
        }

        if (ground_Info.Info.Owner == player.playerID)
        {
            ground_Info.BuildedBill(ground_Info.Info.HouseLv);
            switch (ground_Info.Info.HouseLv)
            {
                case (0):
                    {
                        houseLv = 2;
                        ui_Buildbtns.SetActive(true);
                        ui_BuildMax.SetActive(false);
                        ui_CheckBuild0.SetActive(true);
                        ui_isDone0.SetActive(true);
                        uiTip_Build1.GetComponent<Image>().raycastTarget = false;
                        uiTip_Build2.GetComponent<Image>().raycastTarget = true;
                        uiTip_Build3.GetComponent<Image>().raycastTarget = true;
                        SetBuild(1, ui_isDone1, ui_Blockade, ref isBuild_1, ui_CheckBuild1, uiTip_Build1);
                        SetBuild(2, ui_isDone2, ui_Blockade, ref isBuild_2, ui_CheckBuild2, uiTip_Build2);
                        CanBuild3();
                        BuildPriceCount();
                        break;
                    }
                case (1):
                    {
                        houseLv = 2;
                        ui_Buildbtns.SetActive(true);
                        ui_BuildMax.SetActive(false);
                        ui_CheckBuild0.SetActive(false);
                        ui_isDone0.gameObject.SetActive(true);
                        ui_isDone1.gameObject.SetActive(true);
                        isBuild_1 = true;
                        ui_isDone1.GetComponentInChildren<Text>().text = "已擁有";
                        uiTip_Build1.GetComponent<Image>().raycastTarget = false;
                        uiTip_Build2.GetComponent<Image>().raycastTarget = true;
                        uiTip_Build3.GetComponent<Image>().raycastTarget = true;
                        SetBuild(2, ui_isDone2, ui_Blockade, ref isBuild_2, ui_CheckBuild2, uiTip_Build2);
                        CanBuild3();
                        BuildPriceCount();
                        break;
                    }
                case (2):
                    {
                        houseLv = 3;
                        ui_Buildbtns.SetActive(true);
                        ui_BuildMax.SetActive(false);
                        ui_CheckBuild0.SetActive(false);
                        isBuild_1 = true;
                        isBuild_2 = true;
                        ui_isDone0.gameObject.SetActive(true);
                        ui_isDone1.gameObject.SetActive(true);
                        ui_isDone1.GetComponentInChildren<Text>().text = "已擁有";
                        ui_isDone2.gameObject.SetActive(true);
                        ui_isDone2.GetComponentInChildren<Text>().text = "已擁有";
                        uiTip_Build1.GetComponent<Image>().raycastTarget = false;
                        uiTip_Build2.GetComponent<Image>().raycastTarget = false;
                        CanBuild3();
                        BuildPriceCount();
                        break;
                    }
                case (3):
                    {
                        houseLv = 4;
                        isBuild_1 = true;
                        isBuild_2 = true;
                        isBuild_3 = true;
                        ui_Buildbtns.SetActive(false);
                        ui_BuildMax.SetActive(true);
                        BuildPriceCount();
                        break;
                    }
            }
        }
    }
    private void CanBuild3() //確認是否可以建造地標
    {
        if (player.turnCount < 1)
        {
            uiTip_Build3.GetComponent<Image>().raycastTarget = false;
            ui_Blockade.SetActive(true);
            isBuild_3 = true;
            SwitchBuild(ref isBuild_3, ui_CheckBuild3);
        }
        else if (player.turnCount > 0)
        {
            uiTip_Build3.GetComponent<Image>().raycastTarget = true;
            ui_Blockade.SetActive(false);
            isBuild_3 = false;
            SwitchBuild(ref isBuild_3, ui_CheckBuild3);
            SetBuild(3, ui_isDone3, ui_Blockade, ref isBuild_3, ui_CheckBuild3, uiTip_Build3);
        }
    }

    private void SetBuild(int lv, GameObject isdone, GameObject block, ref bool isbuild, GameObject checkBuild, GameObject buildTip)
    {
        if (lv != 3)
        {
            if (player.PlayerInfo.Assets >= BuildPricce(lv))
            {
                buildTip.GetComponent<Image>().raycastTarget = true;
                isdone.SetActive(false);
                isbuild = false;
            }
            else
            {
                buildTip.GetComponent<Image>().raycastTarget = false;
                isdone.SetActive(true);
                isdone.GetComponentInChildren<Text>().text = "金額不足";
                isbuild = false;
            }
            SwitchBuild(ref isbuild, checkBuild);
        }
        else
        {
            if (player.PlayerInfo.Assets >= BuildPricce(lv) && !block.activeInHierarchy)
            {
                buildTip.GetComponent<Image>().raycastTarget = true;
                isdone.SetActive(false);
                isbuild = false;
            }
            else
            {
                buildTip.GetComponent<Image>().raycastTarget = false;
                isdone.SetActive(true);
                isdone.GetComponentInChildren<Text>().text = "金額不足";
                isbuild = false;
            }
            SwitchBuild(ref isbuild, checkBuild);
        }
    }
    //建造選項開關    
    private void SwitchBuild(ref bool isBuild, GameObject _toogle)
    {
        if (isBuild)
        {
            isBuild = false;
            _toogle.SetActive(isBuild);
            return;
        }
        if (!isBuild)
        {
            isBuild = true;
            _toogle.SetActive(isBuild);
            return;
        }
    }
    //建設成本計算
    private int BuildPricce(int lv)
    {
        int price = 0;
        int[] builds = { ground_Info.Info.Build0, ground_Info.Info.Build1, ground_Info.Info.Build2, ground_Info.Info.Build3, ground_Info.Info.Build4 };
        for (int i = 0; i <= lv; i++)
        {
            price += builds[i];
        }
        return price;
    }
    private void BuildCount(ref int BasePrice, int builid1, int build2, int build3, int build4)
    {
        BasePrice += builid1 + build2 + build3 + build4;
        txt_BuildPrice.text = TextThousand.Instance.SetText(BasePrice);
    }
    private void Discount()//建造優惠計算
    {
        discount = (int)(buildPrice * player.discount);
        txt_Discount.text = TextThousand.Instance.SetText(discount);
    }
    private void FinalPrice()//總金額計算
    {
        finalPrice = buildPrice - discount < 0 ? 0 : buildPrice - discount;
        txt_FinalPrice.text = TextThousand.Instance.SetText(finalPrice);
    }
    private void TollsCount()//過路費計算
    {
        ground_Info.BuildedBill(houseLv);
    }
    private void AllPrice()//總價顯示
    {
        Discount();
        FinalPrice();
        TollsCount();
        ground_Info.SetSoldPrice(); //土地收購價計算
    }
    public void BuildPriceCount()//建設明細顯示計算
    {
        buildPrice = 0;
        switch (ground_Info.Info.HouseLv)
        {
            case (0):
                {
                    if (ground_Info.Info.Owner == 0)
                    {
                        buildPrice = ground_Info.Info.Build0;
                    }

                    if (!isBuild_1 && !isBuild_2 && !isBuild_3)
                    {
                        BuildCount(ref buildPrice, 0, 0, 0, 0);
                        houseLv = ground_Info.Info.HouseLv;
                        CanBuild = false;
                    }
                    else if (isBuild_1 && !isBuild_2 && !isBuild_3)
                    {
                        BuildCount(ref buildPrice, ground_Info.Info.Build1, 0, 0, 0);
                        houseLv = 1;
                        CanBuild = true;
                    }
                    else if (isBuild_1 && isBuild_2 && !isBuild_3)
                    {
                        BuildCount(ref buildPrice, ground_Info.Info.Build1, ground_Info.Info.Build2, 0, 0);
                        houseLv = 2;
                        CanBuild = true;

                    }
                    else if (isBuild_1 && isBuild_2 && isBuild_3)
                    {
                        BuildCount(ref buildPrice, ground_Info.Info.Build1, ground_Info.Info.Build2, ground_Info.Info.Build3, 0);
                        houseLv = 3;
                        CanBuild = true;
                    }
                    break;
                }
            case (1):
                {
                    if (!isBuild_2 && !isBuild_3)
                    {
                        BuildCount(ref buildPrice, 0, 0, 0, 0);
                        houseLv = ground_Info.Info.HouseLv;
                        CanBuild = false;
                    }
                    else if (isBuild_2 && !isBuild_3)
                    {
                        BuildCount(ref buildPrice, 0, ground_Info.Info.Build2, 0, 0);
                        houseLv = 2;
                        CanBuild = true;
                    }
                    else if (isBuild_2 && isBuild_3)
                    {
                        BuildCount(ref buildPrice, 0, ground_Info.Info.Build2, ground_Info.Info.Build3, 0);
                        houseLv = 3;
                        CanBuild = true;
                    }
                    break;
                }
            case (2):
                {
                    if (!isBuild_3)
                    {
                        BuildCount(ref buildPrice, 0, 0, 0, 0);
                        houseLv = ground_Info.Info.HouseLv;
                        CanBuild = false;
                    }
                    else if (isBuild_3)
                    {
                        BuildCount(ref buildPrice, 0, 0, ground_Info.Info.Build3, 0);
                        houseLv = 3;
                        CanBuild = true;
                    }
                    break;
                }
            case (3):
                {
                    BuildCount(ref buildPrice, 0, 0, 0, ground_Info.Info.Build4);
                    houseLv = 4;
                    CanBuild = true;
                    break;
                }
        }
        AllPrice();
    }
    public void Btn_Build(GameObject toogle)//UI按鈕_建造選項開關
    {
        switch (toogle.name.Replace("Build_", ""))
        {
            case ("1"):
                {
                    SwitchBuild(ref isBuild_1, toogle);
                    if (!isBuild_1)
                    {
                        isBuild_2 = true;
                        isBuild_3 = true;
                        SwitchBuild(ref isBuild_2, ui_CheckBuild2);
                        SwitchBuild(ref isBuild_3, ui_CheckBuild3);
                    }
                    break;
                }
            case ("2"):
                {
                    if (!isBuild_1) return;
                    SwitchBuild(ref isBuild_2, toogle);
                    if (!isBuild_2)
                    {
                        isBuild_3 = true;
                        SwitchBuild(ref isBuild_3, ui_CheckBuild3);
                    }
                    break;
                }
            case ("3"):
                {
                    if (!isBuild_2) return;
                    SwitchBuild(ref isBuild_3, ui_CheckBuild3);
                    break;
                }
        }
        BuildPriceCount();
    }

    public void Btn_BuyGound()//UI按鈕_購買
    {
        if (finalPrice > player.PlayerInfo.Assets) return;
        AllPrice();
        ground_Info.owner = player.playerID;
        ground_Info.Info.Owner = player.playerID;
        player.PlayerInfo.Assets -= finalPrice;
        player.myGounds.Add(ground_Info);
        player.CountTotalAsset();
        ground_Info.Info.HouseLv = houseLv;
        ground_Info.SetHouseLv();
        player.SetHappy();
        Btn_CloseUI(3);
    }
    public void Btn_CloseUI(int audioclip) //UI按鈕_關閉
    {
        uIManager.audioSource.clip = PlayAudio.audiosList[audioclip];
        uIManager.audioSource.Play();
        ground_Info.SetTolls(ground_Info.Info.HouseLv);
        uIManager.ui_BuyGround.SetActive(false);
        player.discount = 0;
        if (!ground_Info.isNotRoad)
            ground_Info.areaBonus.SetAreaBonus(player, player.otherPlayer);
        if (player.isHaveCard(6))
        {
            round.cardSubject.setState(6);
        }
        else
        {
            round.cardSubject.setState(0);
            uIManager.ui_BuyGround.SetActive(false);
            uIManager.isActDone = true;
        }
        player = null;
        ground_Info = null;
    }
}
