using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class floorBtn : MonoBehaviour
{
    private UIManager uIManager;
    private FloorManager floorManager;
    private RoundManager round;
    private Text msg;
    private GameObject btn_Sold;
    private int soldMoney;
    [HideInInspector]
    public CardEffect cardEffect;
    [HideInInspector]
    public int payMoney;
    public Sprite img_Sold, img_NoSold;
    public PlayerCtrl player;
    public List<Ground_Info> grounds = new List<Ground_Info>();
    private Transform arrow;

    public enum BtnState
    {
        Idle,
        isUrban, isMRT, isSold, 
        isCard_Earthquake, isCard_HugeEarthquake,
        isCard_Buy, isCard_BuyOther, isCard_Teleport,
        isCard_Tornado,isCard_PowerOff
    }
    public BtnState btnState;
    private void Awake()
    {
        uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        floorManager = GameObject.FindGameObjectWithTag("RoadManager").GetComponent<FloorManager>();
        round = floorManager.GetComponent<RoundManager>();
        msg = transform.parent.Find("Txt_Msg").GetComponent<Text>();
        btn_Sold = transform.parent.Find("btn_Sold").gameObject;
    }

    private void OnEnable()
    {
        SetUI();
    }
    /// <summary>
    /// 設定每個"地板按鈕"的位置
    /// </summary>
    /// <param name="target">對應的地板</param>
    /// <param name="ui_GroundBtn">地板按鈕</param>
    /// <param name="ground_Info">地板的資訊</param>
    private void SetUIPos(GameObject target, RectTransform ui_GroundBtn, Ground_Info ground_Info, bool isfloor)
    {
        if (ui_GroundBtn == null) return;
        ui_GroundBtn.gameObject.SetActive(true);
        ui_GroundBtn.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);
        if(isfloor)
            ground_Info.transform.Find("Fx_FloorSelect").gameObject.SetActive(true);
        else if (!isfloor && ground_Info.transform.parent.name.Contains("Roads"))
            ground_Info.transform.parent.transform.Find("Fx_RoadSelect").gameObject.SetActive(true);
        Reset_arrow(ui_GroundBtn.transform.Find("Select"));
    }

    private void CloseFX()
    {
        foreach (var fx in floorManager.floors)
        {
            fx.transform.Find("Fx_FloorSelect").gameObject.SetActive(false);
            if(fx.transform.parent.name.Contains("Roads"))
                fx.transform.parent.transform.Find("Fx_RoadSelect").gameObject.SetActive(false);
        }
    }
    #region 初始化UI
    public void SetUI()
    {
        switch (btnState)
        {
            case BtnState.isUrban:
                {
                    Set_Urban();
                    break;
                }
            case BtnState.isMRT:
                {
                    Set_MRT();
                    break;
                }
            case BtnState.isSold:
                {
                    Set_SoldHouse();
                    break;
                }
            case BtnState.isCard_Earthquake:
                {
                    Set_Earthquake();
                    break;
                }
            case BtnState.isCard_HugeEarthquake:
                {
                    Set_Earthquake();
                    break;
                }
            case BtnState.isCard_Buy:
                {
                    Set_Buy();
                    break;
                }
            case BtnState.isCard_BuyOther:
                {
                    Set_BuyOther();
                    break;
                }
            case BtnState.isCard_Teleport:
                {
                    Set_Teleport();
                    break;
                }
            case BtnState.isCard_Tornado:
                {
                    Set_Tornado();
                    break;
                }
            case BtnState.isCard_PowerOff:
                {
                    Set_PowerOff();
                    break;
                }
        }
    }
    public void Set_Urban() //都市更新的UI初始化
    {
        msg.text = "請選擇都市更新地點";
        for (int i = 0; i < floorManager.floors.Count; i++)
        {
            Ground_Info _groundInfo = floorManager.floors[i].GetComponent<Ground_Info>();

            if (!_groundInfo.isNotRoad && _groundInfo.Info.Owner == player.playerID)
            {
                RectTransform btn_floor = transform.GetChild(i).GetComponent<RectTransform>();
                SetUIPos(floorManager.floors[i].gameObject, btn_floor, _groundInfo, true);

                if (_groundInfo.isCelebration)
                {
                    btn_floor.gameObject.SetActive(false);
                    _groundInfo.transform.Find("Fx_FloorSelect").gameObject.SetActive(false);
                }
                else
                {
                    btn_floor.gameObject.SetActive(true);
                }
            }
            else if (_groundInfo.isNotRoad || _groundInfo.Info.Owner != player.playerID)
            {
                GameObject btn_floor = transform.GetChild(i).gameObject;
                btn_floor.SetActive(false);
            }
        }
    }

    public void Set_MRT() //高速鐵路的UI初始化
    {
        msg.text = "請選擇目的地";
        for (int i = 0; i < floorManager.floors.Count; i++)
        {
            Ground_Info _groundInfo = floorManager.floors[i].GetComponent<Ground_Info>();
            if (!_groundInfo.name.Contains("card") && _groundInfo.groundID != 24)
            {
                RectTransform btn_floor = transform.GetChild(i).GetComponent<RectTransform>();
                SetUIPos(floorManager.floors[i].gameObject, btn_floor, _groundInfo, true);

                btn_floor.gameObject.SetActive(true);
            }
            else if (_groundInfo.name.Contains("card") || _groundInfo.groundID == 24)
            {
                GameObject btn_floor = transform.GetChild(i).gameObject;
                btn_floor.SetActive(false);
            }
        }
    }

    public void Set_SoldHouse() //破產的UI初始化
    {
        grounds.Clear();
        btn_Sold.SetActive(true);
        payMoney = player.groundInfo.isNotRoad ? payMoney : player.groundInfo.Info.Tolls;
        CanSold();
        for (int i = 0; i < floorManager.floors.Count; i++)
        {
            Ground_Info _groundInfo = floorManager.floors[i].GetComponent<Ground_Info>();

            if (!_groundInfo.isNotRoad && _groundInfo.Info.Owner == player.playerID)
            {
                RectTransform btn_floor = transform.GetChild(i).GetComponent<RectTransform>();
                SetUIPos(floorManager.floors[i].gameObject, btn_floor, _groundInfo, true);

                btn_floor.gameObject.SetActive(true);
            }
            else if (_groundInfo.isNotRoad || _groundInfo.Info.Owner != player.playerID)
            {
                GameObject btn_floor = transform.GetChild(i).gameObject;
                btn_floor.SetActive(false);
            }
        }
    }

    public void Set_Earthquake() //地震卡片UI初始化
    {
        msg.text = "請選擇地震區域";
        for (int i = 0; i < floorManager.floors.Count; i++)
        {
            Ground_Info _groundInfo = floorManager.floors[i].GetComponent<Ground_Info>();

            if (!_groundInfo.isNotRoad && _groundInfo.Info.Owner != 0
                && _groundInfo.Info.Owner != cardEffect.owner.playerID)
            {
                RectTransform btn_floor = transform.GetChild(i).GetComponent<RectTransform>();
                SetUIPos(floorManager.floors[i].gameObject, btn_floor, _groundInfo, true);

                btn_floor.gameObject.SetActive(true);
            }
            else if (_groundInfo.isNotRoad || _groundInfo.Info.Owner == cardEffect.owner.playerID || _groundInfo.Info.Owner == 0)
            {
                GameObject btn_floor = transform.GetChild(i).gameObject;
                btn_floor.SetActive(false);
            }
        }
    }
    public void Set_Buy() //空地購買卡片UI初始化
    {
        msg.text = "請選擇購買地";
        for (int i = 0; i < floorManager.floors.Count; i++)
        {
            Ground_Info _groundInfo = floorManager.floors[i].GetComponent<Ground_Info>();

            if (!_groundInfo.isNotRoad && _groundInfo.Info.Owner == 0)
            {
                RectTransform btn_floor = transform.GetChild(i).GetComponent<RectTransform>();
                SetUIPos(floorManager.floors[i].gameObject, btn_floor, _groundInfo, true);

                btn_floor.gameObject.SetActive(true);
            }
            else if (_groundInfo.isNotRoad || _groundInfo.Info.Owner != 0)
            {
                GameObject btn_floor = transform.GetChild(i).gameObject;
                btn_floor.SetActive(false);
            }
        }
    }
    public void Set_BuyOther() //他人土地購買卡片UI初始化
    {
        msg.text = "請選擇購買地";
        for (int i = 0; i < floorManager.floors.Count; i++)
        {
            Ground_Info _groundInfo = floorManager.floors[i].GetComponent<Ground_Info>();

            if (!_groundInfo.isNotRoad && _groundInfo.Info.Owner != 0
                && _groundInfo.Info.Owner != cardEffect.owner.playerID)
            {
                RectTransform btn_floor = transform.GetChild(i).GetComponent<RectTransform>();
                SetUIPos(floorManager.floors[i].gameObject, btn_floor, _groundInfo, true);

                btn_floor.gameObject.SetActive(true);
            }
            else if (_groundInfo.isNotRoad || _groundInfo.Info.Owner == cardEffect.owner.playerID || _groundInfo.Info.Owner == 0)
            {
                GameObject btn_floor = transform.GetChild(i).gameObject;
                btn_floor.SetActive(false);
            }
        }
    }
    public void Set_Teleport() //傳送卡片UI初始化
    {
        msg.text = "請選擇傳送地點";
        for (int i = 0; i < floorManager.floors.Count; i++)
        {
            Ground_Info _groundInfo = floorManager.floors[i].GetComponent<Ground_Info>();

            if (!_groundInfo.name.Contains("card"))
            {
                RectTransform btn_floor = transform.GetChild(i).GetComponent<RectTransform>();
                SetUIPos(floorManager.floors[i].gameObject, btn_floor, _groundInfo, true);

                btn_floor.gameObject.SetActive(true);
            }
            else if (_groundInfo.name.Contains("card"))
            {
                GameObject btn_floor = transform.GetChild(i).gameObject;
                btn_floor.SetActive(false);
            }
        }
    }
    public void Set_Tornado() //龍捲風卡片UI初始化
    {
        msg.text = "請選擇龍捲風區域";
        for (int i = 0; i < floorManager.floors.Count; i++)
        {
            Ground_Info _groundInfo = floorManager.floors[i].GetComponent<Ground_Info>();
            if (!_groundInfo.isNotRoad && !_groundInfo.isTornado)
            {
                RectTransform btn_floor = transform.GetChild(i).GetComponent<RectTransform>();
                SetUIPos(floorManager.floors[i].gameObject, btn_floor, _groundInfo, false);

                btn_floor.gameObject.SetActive(true);
            }
            else
            {
                GameObject btn_floor = transform.GetChild(i).gameObject;
                btn_floor.SetActive(false);
            }
                
        }
    }
    public void Set_PowerOff() //土地停電卡片UI初始化
    {
        msg.text = "請選擇停電土地";
        for (int i = 0; i < floorManager.floors.Count; i++)
        {
            Ground_Info _groundInfo = floorManager.floors[i].GetComponent<Ground_Info>();

            if (!_groundInfo.isNotRoad && _groundInfo.Info.Owner != 0
                && _groundInfo.Info.Owner != cardEffect.owner.playerID && !_groundInfo.isPowerOff)
            {
                RectTransform btn_floor = transform.GetChild(i).GetComponent<RectTransform>();
                SetUIPos(floorManager.floors[i].gameObject, btn_floor, _groundInfo, true);

                btn_floor.gameObject.SetActive(true);
            }
            else if (_groundInfo.isNotRoad || _groundInfo.Info.Owner == cardEffect.owner.playerID 
                || _groundInfo.Info.Owner == 0 || _groundInfo.isPowerOff)
            {
                GameObject btn_floor = transform.GetChild(i).gameObject;
                btn_floor.SetActive(false);
            }

        }
    }
    #endregion

    public void Btn_SelectFloor(GameObject btn_Floor)
    {
        switch (btnState)
        {
            case BtnState.isUrban:
                {
                    if(player.PlayerInfo.Assets >= ReadGameValue.Instance.GetValue(51))
                    {
                        foreach (var floor in floorManager.floors)
                        {
                            if (Regex.Replace(floor.name, "[^0-9]", "") == Regex.Replace(btn_Floor.name, "[^0-9]", ""))
                            {
                                btnState = BtnState.Idle;
                                Corner.Instance.Btn_Urban(player, floor.GetComponent<Ground_Info>());                                
                                CloseFX();
                                break;
                            }
                        }
                    }
                    
                    break;
                }
            case BtnState.isMRT:
                {
                    foreach (var floor in floorManager.floors)
                    {
                        if (Regex.Replace(floor.name, "[^0-9]", "") == Regex.Replace(btn_Floor.name, "[^0-9]", ""))
                        {
                            btnState = BtnState.Idle;
                            Corner.Instance.Btn_MRT(player, floor.GetComponent<Ground_Info>());
                            CloseFX();
                            break;
                        }
                    }
                    break;
                }
            case BtnState.isSold:
                {
                    foreach (var floor in floorManager.floors)
                    {
                        if (Regex.Replace(floor.name, "[^0-9]", "") == Regex.Replace(btn_Floor.name, "[^0-9]", ""))
                        {
                            Ground_Info _Info = floor.GetComponent<Ground_Info>();
                            if (grounds.Count > 0)
                            {
                                if (grounds.Contains(_Info))
                                {
                                    grounds.Remove(_Info);
                                }
                                else if (!grounds.Contains(_Info))
                                {
                                    grounds.Add(_Info);
                                }
                            }
                            else
                            {
                                grounds.Add(_Info);
                            }
                            Set_arrow(_Info);
                            CanSold();
                            break;
                        }
                    }
                    break;
                }
            case BtnState.isCard_Earthquake:
                {
                    foreach (var floor in floorManager.floors)
                    {
                        if (Regex.Replace(floor.name, "[^0-9]", "") == Regex.Replace(btn_Floor.name, "[^0-9]", ""))
                        {
                            cardEffect.targetFloor = floor.GetComponent<Ground_Info>();
                            Card_FloorBtn(7);
                            CloseFX();
                            break;
                        }
                    }
                    break;
                }
            case BtnState.isCard_HugeEarthquake:
                {
                    foreach (var floor in floorManager.floors)
                    {
                        if (Regex.Replace(floor.name, "[^0-9]", "") == Regex.Replace(btn_Floor.name, "[^0-9]", ""))
                        {
                            cardEffect.targetFloor = floor.GetComponent<Ground_Info>();
                            Card_FloorBtn(8);
                            CloseFX();
                            break;
                        }
                    }
                    break;
                }
            case BtnState.isCard_Buy:
                {
                    foreach (var floor in floorManager.floors)
                    {
                        if (Regex.Replace(floor.name, "[^0-9]", "") == Regex.Replace(btn_Floor.name, "[^0-9]", ""))
                        {
                            cardEffect.targetFloor = floor.GetComponent<Ground_Info>();
                            Card_FloorBtn(5);
                            CloseFX();
                            break;
                        }
                    }
                    break;
                }
            case BtnState.isCard_BuyOther:
                {
                    foreach (var floor in floorManager.floors)
                    {
                        if (Regex.Replace(floor.name, "[^0-9]", "") == Regex.Replace(btn_Floor.name, "[^0-9]", ""))
                        {
                            cardEffect.targetFloor = floor.GetComponent<Ground_Info>();
                            Card_FloorBtn(6);
                            CloseFX();
                            break;
                        }
                    }
                    break;
                }
            case BtnState.isCard_Teleport:
                {
                    foreach (var floor in floorManager.floors)
                    {
                        if (Regex.Replace(floor.name, "[^0-9]", "") == Regex.Replace(btn_Floor.name, "[^0-9]", ""))
                        {
                            cardEffect.targetFloor = floor.GetComponent<Ground_Info>();
                            Card_FloorBtn(3);
                            CloseFX();
                            break;
                        }
                    }
                    break;
                }
            case BtnState.isCard_Tornado:
                {
                    foreach (var floor in floorManager.floors)
                    {
                        if (Regex.Replace(floor.name, "[^0-9]", "") == Regex.Replace(btn_Floor.name, "[^0-9]", ""))
                        {
                            cardEffect.targetFloor = floor.GetComponent<Ground_Info>();
                            Card_FloorBtn(9);
                            CloseFX();
                            break;
                        }
                    }
                    break;
                }
            case BtnState.isCard_PowerOff:
                {
                    foreach (var floor in floorManager.floors)
                    {
                        if (Regex.Replace(floor.name, "[^0-9]", "") == Regex.Replace(btn_Floor.name, "[^0-9]", ""))
                        {
                            cardEffect.targetFloor = floor.GetComponent<Ground_Info>();
                            Card_FloorBtn(13);
                            CloseFX();
                            break;
                        }
                    }
                    break;
                }
        }
    }
    public void Reset_arrow(Transform Arrow)
    {
        foreach (Transform arrow in Arrow)
        {
            arrow.gameObject.SetActive(false);
        }
    }
    public void Set_arrow(Ground_Info ground)
    {
        foreach (Transform a in transform)
        {
            if (Regex.Replace(a.name, "[^0-9]", "") == Regex.Replace(ground.gameObject.name, "[^0-9]", ""))
            {
                arrow = a.transform.Find("Select");
            }
        }

        if (ground.owner == 1)
        {
            if (!arrow.transform.Find("P1").gameObject.activeInHierarchy)
            {
                arrow.transform.Find("P1").gameObject.SetActive(true);
            }
            else
            {
                arrow.transform.Find("P1").gameObject.SetActive(false);
            }
        }
        else if (ground.owner == 2)
        {
            if (!arrow.transform.Find("P2").gameObject.activeInHierarchy)
            {
                arrow.transform.Find("P2").gameObject.SetActive(true);
            }
            else
            {
                arrow.transform.Find("P2").gameObject.SetActive(false);
            }
        }
    }

    private void Card_FloorBtn(int cardId)
    {
        btnState = BtnState.Idle;
        PageCtrl uiBuyGround = uIManager.ui_BuyGround.GetComponent<PageCtrl>();
        cardEffect.canLaunch = true;
        round.cardSubject.setState(cardId);
        uiBuyGround.Mask2.SetActive(false);
        uiBuyGround.Close_Page("SelectGround_Urban");
        uIManager.ui_BuyGround.SetActive(false);
        CloseFX();
    }

    #region 賣房
    public void Btn_SoldHouse()
    {
        
        if (!CanSold()) return;
        foreach (var ground in grounds)
        {
            ground.Info.Owner = 0;
            player.myGounds.Remove(ground);
            ground.Info.HouseLv = 0;
            ground.owner = 0;
            ground.houseLv = 0;
            ground.SetHouseLv();
            ground.SetTolls(ground.Info.HouseLv);
            ground.SetSoldPrice();
            ground.areaBonus.SetAreaBonus(player, player.otherPlayer);
        }
        player.PlayerInfo.Assets += soldMoney;
        player.toSoldHouse = false;
        uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask.SetActive(true);
        uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask2.SetActive(false);
        player.CountTotalAsset();
        btn_Sold.SetActive(false);
        CloseFX();
        if (!player.groundInfo.isNotRoad)
        {
            player.isPay = false;
            uIManager.ui_BuyGround.GetComponent<PageCtrl>().SwitchToPage("Pay_Tolls");
        }
        else
        {
            player.playerState = PlayerCtrl.PlayerState.RoadAction;
            uIManager.ui_BuyGround.SetActive(false);
            player.fx_PayCoin2.SetActive(true);
            player.PlayerInfo.Assets -= payMoney;
            uIManager.isActDone = true;
        }
    }

    private bool CanSold()
    {
        soldMoney = 0;
        foreach (var ground in grounds)
        {
            soldMoney += ground.Info.GroundValue;
        }
        //soldMoney = (int)(soldMoney * ReadGameValue.Instance.GetValue(7));
        double shortage = (payMoney - player.PlayerInfo.Assets) - soldMoney <= 0 ? 0 : (payMoney - player.PlayerInfo.Assets) - soldMoney;
        
        if (shortage <= 0)
        {
            btn_Sold.GetComponent<Image>().sprite = img_Sold;
            msg.text = "請選擇半價拋售房產" + "\n" + "拋售價錢: " + TextThousand.Instance.SetText(soldMoney) +
                "，不足: " + TextThousand.Instance.SetText(shortage);
            return true;
        }
        else
        {
            btn_Sold.GetComponent<Image>().sprite = img_NoSold;
            msg.text = "請選擇半價拋售房產" + "\n" + "拋售價錢: " + TextThousand.Instance.SetText(soldMoney) +
                "，不足: " + TextThousand.Instance.SetText(shortage);
            return false;
        }
    }
    #endregion
}
