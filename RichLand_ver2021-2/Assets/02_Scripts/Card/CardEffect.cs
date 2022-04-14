using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CardEffect : MonoBehaviour
{
    //ConcreteObserver_1 cardObserver;
    [HideInInspector]
    public RoundManager roundManager;
    [HideInInspector]
    public UIManager uIManager;
    [HideInInspector]
    public Card cardView;
    [HideInInspector]
    public Ground_Info targetFloor;
    [HideInInspector]
    public PlayerCtrl targetPlayer;
    [HideInInspector]
    public CardEffect targetCard;
    public PlayerCtrl owner;
    public CardInfo cardInfo;
    public GameObject card_Fx;
    [HideInInspector]
    public Vector3 fx_Pos;
    private GameObject character;
    private Text txt_EndRound;
    private List<Text> txt_EndRounds = new List<Text>();
    #region RobberyCard
    private bool checkCard, isAntimissile;
    private List<CardEffect> robberyCards = new List<CardEffect>();
    private int[] defenseWeights = { 17, 15, 16 };
    private int index = 0;
    private RoundManager.RoundCount roundTemp;
    #endregion

    public bool isEnd, isOpen, isAniEnd, isFx, canLaunch, hasLaunched;

    [HideInInspector]
    public int getRound;
    [HideInInspector]
    public int endRound;
    private void Update()
    {
        Effects(cardInfo.CardID, cardInfo);
        CardEnd();
        Text_RoundCount();
    }

    public void CardEnd()
    {
        if (!cardInfo.ContinuedRound.Equals(0) && roundManager.roundCount >= endRound)
        {
            if(cardInfo.CardID != 10)
                Card_End();
            return;
        }
        if ((cardInfo.CardID == 10 && cardInfo.ContinuedRound < 1))
        {
            Card_End();
        }
        if (isEnd && isAniEnd)
        {
            Card_End();
        }
    }

    public void Effects(int cardNumber, CardInfo Info)
    {
        if (isEnd || roundManager.cardSubject.GetState() != cardNumber) return;
        switch (cardNumber)
        {
            case (1):
                {
                    Card_ToMRT();
                    break;
                }
            case (2):
                {
                    Card_ToUrban();
                    break;
                }
            case (3):
                {
                    Card_Teleport();
                    break;
                }
            case (4):
                {
                    Card_TpToVacation();
                    break;
                }
            case (5):
                {
                    Card_Buy();
                    break;
                }
            case (6):
                {
                    Card_BuyOther();
                    break;
                }
            case (7):
                {
                    Card_Earthquake();
                    break;
                }
            case (8):
                {
                    Card_HugeEarthquake();
                    break;
                }
            case (9):
                {
                    Card_Tornado(Info.EffectValue);
                    break;
                }
            case (10):
                {
                    Card_Devil(Info.EffectValue);
                    break;
                }
            case (11):
                {
                    Card_Tax(Info.EffectValue);
                    break;
                }
            case (12):
                {
                    Card_Thief(Info.EffectValue);
                    break;
                }
            case (13):
                {
                    Card_PowerOff();
                    break;
                }
            case (14):
                {
                    Card_Robbery();
                    break;
                }
            case (15):
                {
                    Card_Shield();
                    break;
                }
            case (16):
                {
                    Card_Angel();
                    break;
                }
            case (17):
                {
                    Card_Antimissile();
                    break;
                }
            case (18):
                {
                    Card_BuildUpdate((int)Info.EffectValue);
                    break;
                }
            case (19):
                {
                    Card_BuyWithHalf(Info.EffectValue);
                    break;
                }
            case (20):
                {
                    Card_Peace();
                    break;
                }
        }
    }
    #region 演出
    IEnumerator Fx_Coin(PlayerCtrl player)
    {
        player.fx_PayCoin.SetActive(true);
        yield return new WaitForSeconds(1f);
        player.fx_PayCoin.SetActive(false);
    }
    public void GetFx()
    {
        foreach (Transform fx in uIManager.card_Fx.transform)
        {
            if (Regex.Replace(fx.name, "[^0-9]", "") == cardInfo.CardID.ToString())
                card_Fx = fx.gameObject;
        }
        isFx = card_Fx != null;
    }

    public void PlayFx(bool isDone)
    {
        if (!isFx) return;
        card_Fx.transform.position = fx_Pos;
        card_Fx.SetActive(true);
        card_Fx.GetComponent<Card_Fx>().player = owner;
        card_Fx.GetComponent<Card_Fx>().isDone = isDone;
    }
    public void CloseFx()
    {
        if (!isFx) return;
        card_Fx.SetActive(false);
        card_Fx.transform.localPosition = Vector3.zero;
    }
    private void OpenCard()
    {
        if (isOpen) return;
        isOpen = true;
        cardView.cardID = cardInfo.CardID;
        cardView.cardEffect = this;
        uIManager.card_Effect.SetActive(true);
    }
    #endregion

    #region 卡片功能
    private void Card_End()
    {
        if (cardInfo.CardID != 17)
        {
            roundManager.cardSubject.setState(0);
            roundManager.SetCurrentState();
        }
        owner.RemoveCard(cardInfo.CardID);
        if (!roundManager.now_Player.isHaveCard(20) && !roundManager.other_Player.isHaveCard(20))
        {
            roundManager.now_Player.isPeace = false;
            roundManager.other_Player.isPeace = false;
        }
        if (cardInfo.CardID == 9)
        {
            AreaBonus area = targetFloor.transform.parent.GetComponent<AreaBonus>();
            foreach (var floor in area.floors)
            {
                floor.isTornado = false;
                floor.fx_Tornado.SetActive(false);
                floor.SetTolls(targetFloor.Info.HouseLv);
            }
        }
        if (cardInfo.CardID == 10)
        {
            Vector3 pos = targetPlayer.canvas_PlayerLabel.transform.position;
            targetPlayer.fx_Devil.SetActive(false);
            targetPlayer.canvas_PlayerLabel.transform.position = new Vector3(pos.x, targetPlayer.playerLabel_OldPos, pos.z);
        }
        if (cardInfo.CardID == 13 && targetFloor != null)
        {
            targetFloor.isPowerOff = false;
            targetFloor.fx_PowerOff.SetActive(false);
            targetFloor.SetTolls(targetFloor.Info.HouseLv);
        }
        if (cardInfo.CardID == 20 && !owner.isHaveCard(20) && !owner.otherPlayer.isHaveCard(20))
        {
            Vector3 pos1 = roundManager.player1.canvas_PlayerLabel.transform.position;
            Vector3 pos2 = roundManager.player2.canvas_PlayerLabel.transform.position;
            roundManager.player1.fx_Peace.SetActive(false);
            roundManager.player2.fx_Peace.SetActive(false);
            roundManager.player1.canvas_PlayerLabel.transform.position = new Vector3(pos1.x, roundManager.player1.playerLabel_OldPos, pos1.z);
            roundManager.player2.canvas_PlayerLabel.transform.position = new Vector3(pos2.x, roundManager.player2.playerLabel_OldPos, pos2.z);
        }
        Destroy(gameObject);
        Debug.Log("卡片" + cardInfo.CardID + "消失");
    }
    #region RobberyCard
    private bool HaveAllRobberyCard()
    {
        foreach (var card in robberyCards)
        {
            if (!owner.isHaveCard(card.cardInfo.CardID))
            {
                checkCard = false;
                break;
            }
            else
            {
                checkCard = true;
            }
        }
        return checkCard;
    }
    private void GetTargetCard()
    {
        for (int j = 0; j < robberyCards.Count; j++)
        {
            if (robberyCards[j].cardInfo.CardID == defenseWeights[index])
            {
                targetCard = targetPlayer.Card(robberyCards[j].cardInfo.CardID);
                break;
            }
            else
            {
                index = index + 1 >= defenseWeights .Length? 0 : index + 1;
                GetTargetCard();
            }
        }
    }
    private void RobberyCard()
    {
        if (robberyCards.Count < 1) return;
        GetTargetCard();
        if (HaveAllRobberyCard())
        {
            targetPlayer.RemoveCard(targetCard.cardInfo.CardID);
            Destroy(targetCard.gameObject);
            isEnd = true;
        }
        else
        {
            if (owner.isHaveCard(targetCard.cardInfo.CardID))
            {
                robberyCards.Remove(targetCard);
                RobberyCard();
                return;
            }
            else
            {
                index = index + 1 >= defenseWeights.Length ? 0 : index + 1;
                targetPlayer.Card(targetCard.cardInfo.CardID).owner = owner;
                targetCard.transform.parent = owner.cardObject.transform;
                targetPlayer.RemoveCard(targetCard.cardInfo.CardID);
                owner.myCards.Add(targetCard);
                isEnd = true;
            }
        }
    }
    #endregion    
    private bool IsShield(PlayerCtrl another) //盾牌和反彈
    {
        if (another.isHaveCard(17))
        {
            Card_End();
            cardView.GetCard(cardInfo.CardID, another);
            another.Card(cardInfo.CardID).isAntimissile = true;
            another.Card(17).targetCard = another.Card(cardInfo.CardID);
            roundTemp = roundManager.roundTurn;
            roundManager.roundTurn = RoundManager.RoundCount.Stay;
            roundManager.now_Player = another;
            roundManager.other_Player = another.otherPlayer;
            roundManager.cardSubject.setState(17);
            return true;
        }
        else if (another.isHaveCard(15))
        {
            Card_End();
            roundManager.cardSubject.setState(15);
            return true;
        }
        else return false;
    }
    private void Text_RoundCount()
    {
        if (txt_EndRound != null && cardInfo.CardID == 10)
        {
            txt_EndRound.text = "x" + cardInfo.ContinuedRound;
        }
        if (txt_EndRound != null && cardInfo.CardID == 13)
        {
            txt_EndRound.text = "x" + (endRound - roundManager.roundCount);
        }
        if (txt_EndRounds.Count != 0 && (cardInfo.CardID == 9 || cardInfo.CardID == 20))
        {
            foreach (var txt in txt_EndRounds)
            {
                txt.text = "x" + (endRound - roundManager.roundCount);
            }
        }
    }
    #region 額外效果(迭代器)    
    private IEnumerator EndWithTip(bool End) //關閉並提示
    {
        PageCtrl cutScene = uIManager.uiManager_pageCtrl.GetPage("Game_Cutscene").GetComponent<PageCtrl>();
        cutScene.gameObject.SetActive(true);
        cutScene.SwitchToPage("Card_Tip");
        roundManager.cardSubject.setState(0);
        yield return new WaitForSeconds(1.5f);
        isEnd = End;
        cutScene.gameObject.SetActive(false);
        owner.uiManager.isActDone = true;
    }
    private IEnumerator TeleportStart(float sec, bool firstAct, bool ActDone) //傳送前半段
    {
        roundManager.cardSubject.setState(0);
        targetPlayer.SetHappy();
        fx_Pos = new(targetPlayer.transform.position.x + 0.5f, targetPlayer.transform.position.y + 0.5f, targetPlayer.transform.position.z);
        PlayFx(false);
        yield return new WaitForSeconds(sec - 0.5f);
        targetPlayer.road_Now = targetFloor.groundID;
        targetPlayer.transform.position = targetFloor.transform.position;
        foreach (Transform ch in targetPlayer.transform)
        {
            if (ch.name.Contains("Character") && ch.gameObject.activeInHierarchy)
            {
                character = ch.gameObject;
                character.SetActive(false);
                break;
            }
        }
        yield return new WaitForSeconds(sec / 2);
        StartCoroutine(Teleport(card_Fx.GetComponent<Card_Fx>().closeTime, firstAct, ActDone));
    }
    private IEnumerator Teleport(float sec, bool firstAct, bool ActDone) //傳送效果後半段
    {
        fx_Pos = new(targetPlayer.transform.position.x + 0.5f, targetPlayer.transform.position.y + 0.5f, targetPlayer.transform.position.z);
        PlayFx(false);
        yield return new WaitForSeconds(sec - 0.5f);
        character.SetActive(true);
        targetPlayer.SetHappy();
        yield return new WaitForSeconds(sec - 0.7f);
        targetPlayer.isTp = true;
        targetPlayer.toMRT = false;
        targetPlayer.groundInfo = targetFloor;
        targetPlayer.haveFirstAct = firstAct;
        targetPlayer.playerState = PlayerCtrl.PlayerState.RoadAction;
        uIManager.isActDone = ActDone;
        isEnd = true;
    }
    private IEnumerator Antimissile(float time)
    {        
        fx_Pos = new(owner.transform.position.x + 0.1f, owner.transform.position.y + 1.5f, owner.transform.position.z);
        PlayFx(false);
        yield return new WaitForSeconds(time);
        roundManager.cardSubject.setState(targetCard.cardInfo.CardID);
        isEnd = true;
    }
    #endregion
    private void Card_ToMRT() //1
    {
        if (owner.playerState == PlayerCtrl.PlayerState.RoadAction)
        {
            OpenCard();
            if (!isAniEnd) return;
            owner.road_Dice = 24;
            owner.playerState = PlayerCtrl.PlayerState.Move;
            isEnd = true;
        }
    }
    private void Card_ToUrban() //2
    {
        if (owner.playerState == PlayerCtrl.PlayerState.RoadAction)
        {
            OpenCard();
            if (!isAniEnd) return;
            owner.road_Dice = 16;
            owner.playerState = PlayerCtrl.PlayerState.Move;
            isEnd = true;
        }
    }
    private void Card_Teleport() //3
    {
        if (roundManager.now_Player != owner && !isAntimissile) return;
        if (owner.playerState == PlayerCtrl.PlayerState.RoadAction || isAntimissile)
        {
            OpenCard();
            if (!isAniEnd) return;

            if (!uIManager.ui_BuyGround.activeInHierarchy && canLaunch)
            {
                if (targetPlayer != owner && IsShield(targetPlayer)) return;

                StartCoroutine(TeleportStart(card_Fx.GetComponent<Card_Fx>().closeTime, false, false));
            }
            else if (!canLaunch)
            {
                if (targetPlayer != null)
                {                    
                    floorBtn selectUI = uIManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("SelectGround_Urban").GetComponentInChildren<floorBtn>();
                    selectUI.btnState = floorBtn.BtnState.isCard_Teleport;
                    selectUI.cardEffect = this;
                    owner.OpenGroundUI("SelectGround_Urban");
                    uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask.SetActive(false);
                    uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask2.SetActive(true);
                }
                else
                {
                    SelectPlayer select = uIManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("SelectPlayer").GetComponent<SelectPlayer>();
                    select.card = this;
                    select.canLaunch = false;
                    owner.OpenGroundUI("SelectPlayer");
                }
            }
        }
    }
    private void Card_TpToVacation() //4
    {
        if (roundManager.now_Player != owner && !isAntimissile) return;
        if (owner.playerState == PlayerCtrl.PlayerState.RoadAction || isAntimissile)
        {
            OpenCard();
            if (!isAniEnd) return;
            targetPlayer = owner.otherPlayer;
            targetFloor = roundManager.GetComponent<FloorManager>().floors[8].GetComponent<Ground_Info>();
            if (targetPlayer != owner && IsShield(targetPlayer)) return;

            StartCoroutine(TeleportStart(card_Fx.GetComponent<Card_Fx>().closeTime, true, true));
        }
    }
    private void Card_Buy() //5
    {
        if (roundManager.now_Player != owner) return;
        if (owner.playerState == PlayerCtrl.PlayerState.RoadAction)
        {
            OpenCard();
            if (!isAniEnd) return;

            if (canLaunch)
            {
                owner.groundInfo = targetFloor;
                uIManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("Buy_Ground").GetComponent<BuyGround>().player = owner;
                owner.OpenGroundUI("Buy_Ground");
                isEnd = true;
            }
            else
            {
                if (!roundManager.GetComponent<FloorManager>().CheckTerritory())
                {
                    StartCoroutine(EndWithTip(true));
                    return;
                }
                floorBtn selectUI = uIManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("SelectGround_Urban").GetComponentInChildren<floorBtn>();
                selectUI.player = roundManager.other_Player;
                selectUI.btnState = floorBtn.BtnState.isCard_Buy;
                selectUI.cardEffect = this;
                owner.OpenGroundUI("SelectGround_Urban");
                uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask.SetActive(false);
                uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask2.SetActive(true);
            }
        }
    }
    private void Card_BuyOther() //6
    {
        if (roundManager.now_Player != owner && !isAntimissile) return;
        if ((owner.playerState == PlayerCtrl.PlayerState.RoadAction) || isAntimissile) //roundManager.other_Player.myGounds.Count > 0
        {
            OpenCard();
            if (!isAniEnd) return;

            if(roundManager.other_Player.myGounds.Count > 0)
            {
                if (canLaunch)
                {
                    if (targetPlayer != owner && IsShield(targetPlayer)) return;
                    owner.groundInfo = targetFloor;
                    uIManager.ui_BuyGround.SetActive(true);
                    uIManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("Buy_OtherGround").GetComponent<Buy_OtherGround>().player = owner;
                    uIManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("Buy_OtherGround").GetComponent<Buy_OtherGround>().isBuyOrigin = true;
                    uIManager.ui_BuyGround.GetComponent<PageCtrl>().SwitchToPage("Buy_OtherGround");
                }
                else
                {
                    if (uIManager.ui_BuyGround.GetComponent<PageCtrl>().IsOpen("Pay_Tolls"))
                    {
                        targetFloor = owner.groundInfo;
                        canLaunch = true;
                        return;
                    }
                    floorBtn selectUI = uIManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("SelectGround_Urban").GetComponentInChildren<floorBtn>();
                    selectUI.player = roundManager.other_Player;
                    targetPlayer = selectUI.player;
                    selectUI.btnState = floorBtn.BtnState.isCard_BuyOther;
                    selectUI.cardEffect = this;
                    owner.OpenGroundUI("SelectGround_Urban");
                    uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask.SetActive(false);
                    uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask2.SetActive(true);
                }
            }
            else
            {
                StartCoroutine(EndWithTip(true));
            }            
        }
    }
    private void Card_Earthquake() //7
    {
        if (roundManager.now_Player != owner && !isAntimissile) return;
        if (owner.playerState == PlayerCtrl.PlayerState.RoadAction || isAntimissile)
        {
            OpenCard();
            if (!isAniEnd) return;

            if (canLaunch)
            {
                if (targetPlayer != owner && IsShield(targetPlayer)) return;
                fx_Pos = new(targetFloor.transform.position.x, targetFloor.transform.position.y + 4f, targetFloor.transform.position.z);
                PlayFx(true);
                targetFloor.Info.HouseLv = targetFloor.Info.HouseLv == 0 ? 0 : targetFloor.Info.HouseLv - 1;
                targetFloor.houseLv = targetFloor.Info.HouseLv;
                targetFloor.SetHouseLv();
                targetFloor.SetTolls(targetFloor.houseLv);
                targetPlayer.CountTotalAsset();
                isEnd = true;
            }
            else
            {
                targetPlayer = owner.otherPlayer;
                if (targetPlayer.myGounds.Count < 1)
                {
                    StartCoroutine(EndWithTip(true));
                    return;
                }
                floorBtn selectUI = uIManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("SelectGround_Urban").GetComponentInChildren<floorBtn>();
                selectUI.player = targetPlayer;
                
                selectUI.btnState = floorBtn.BtnState.isCard_Earthquake;
                selectUI.cardEffect = this;
                owner.OpenGroundUI("SelectGround_Urban");
                uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask.SetActive(false);
                uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask2.SetActive(true);
            }
        }
    }
    private void Card_HugeEarthquake() //8
    {
        if (roundManager.now_Player != owner && !isAntimissile) return;
        if (owner.playerState == PlayerCtrl.PlayerState.RoadAction || isAntimissile)
        {
            OpenCard();
            if (!isAniEnd) return;

            if (canLaunch)
            {
                if (targetPlayer != owner && IsShield(targetPlayer)) return;
                fx_Pos = new(targetFloor.transform.position.x, targetFloor.transform.position.y + 4f, targetFloor.transform.position.z);
                PlayFx(true); 
                targetFloor.Info.HouseLv = 0;
                targetFloor.houseLv = targetFloor.Info.HouseLv;
                targetFloor.SetHouseLv();
                targetFloor.SetTolls(targetFloor.Info.HouseLv);
                targetPlayer.CountTotalAsset();
                isEnd = true;
            }
            else
            {
                targetPlayer = owner.otherPlayer;
                if (targetPlayer.myGounds.Count < 1)
                {
                    StartCoroutine(EndWithTip(true));
                    return;
                }
                floorBtn selectUI = uIManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("SelectGround_Urban").GetComponentInChildren<floorBtn>();
                selectUI.player = targetPlayer;

                selectUI.btnState = floorBtn.BtnState.isCard_HugeEarthquake;
                selectUI.cardEffect = this;
                owner.OpenGroundUI("SelectGround_Urban");
                uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask.SetActive(false);
                uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask2.SetActive(true);
            }
        }
    }
    private void Card_Tornado(float discount) //9
    {
        if (roundManager.now_Player != owner && !isAntimissile) return;
        if (owner.playerState == PlayerCtrl.PlayerState.RoadAction || isAntimissile)
        {
            if (!isAniEnd)
            {
                OpenCard();
            }
            if (canLaunch && isAniEnd)
            {
                bool haveshield = false;
                PayTolls payTolls = uIManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("Pay_Tolls").GetComponent<PayTolls>();
                if (roundManager.roundCount != endRound && !hasLaunched)
                {    
                    hasLaunched = true;
                    AreaBonus area = targetFloor.transform.parent.GetComponent<AreaBonus>();
                    foreach (var floor in area.floors)
                    {
                        if (floor.owner != 0 && targetPlayer.playerID == floor.owner && (targetPlayer.isHaveCard(15) || targetPlayer.isHaveCard(17)))
                        {
                            haveshield = true;
                        }
                        else
                        {
                            if (!floor.isPowerOff)
                            {
                                floor.isTornado = true;
                                floor.tornadoPrice = discount;
                                floor.fx_Tornado.SetActive(true);
                                txt_EndRounds.Add(floor.fx_Tornado.GetComponentInChildren<Text>());
                                floor.SetTolls(targetFloor.Info.HouseLv);
                            }
                        }
                    }
                    if (haveshield)
                    {
                        if (targetPlayer.isHaveCard(17))
                        {
                            cardView.GetCard(cardInfo.CardID, targetPlayer);
                            targetPlayer.Card(cardInfo.CardID).isAntimissile = true;
                            targetPlayer.Card(17).targetCard = targetPlayer.Card(cardInfo.CardID);
                            roundTemp = roundManager.roundTurn;
                            roundManager.roundTurn = RoundManager.RoundCount.Stay;
                            roundManager.now_Player = targetPlayer;
                            roundManager.other_Player = targetPlayer.otherPlayer;
                            roundManager.cardSubject.setState(17);
                        }
                        else
                        {
                            roundManager.cardSubject.setState(15);
                        }                            
                    }
                    else
                    {
                        roundManager.SetCurrentState();
                        uIManager.isActDone = true;
                        roundManager.cardSubject.setState(0);
                    }
                }
            }
            else if (!canLaunch && isAniEnd)
            {                
                floorBtn selectUI = uIManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("SelectGround_Urban").GetComponentInChildren<floorBtn>();
                selectUI.player = roundManager.other_Player;
                targetPlayer = selectUI.player;
                selectUI.btnState = floorBtn.BtnState.isCard_Tornado;
                selectUI.cardEffect = this;
                owner.OpenGroundUI("SelectGround_Urban");
                uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask.SetActive(false);
                uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask2.SetActive(true);
            }
        }
    }
    private void Card_Devil(float devilDiscount) //10
    {
        if (roundManager.now_Player != owner && !isAntimissile) return;
        if (owner.playerState == PlayerCtrl.PlayerState.RoadAction || isAntimissile)
        {
            if (!isAniEnd)
            {
                targetPlayer = owner.otherPlayer;
                OpenCard();
                return;
            }
            if (!hasLaunched && isAniEnd)
            {
                if (targetPlayer != owner && IsShield(targetPlayer)) return;
                Vector3 pos = targetPlayer.canvas_PlayerLabel.transform.position;
                hasLaunched = true;
                targetPlayer.isDevil = true;
                targetPlayer.devilTolls = devilDiscount;
                targetPlayer.fx_Devil.SetActive(true);
                targetPlayer.canvas_PlayerLabel.transform.position = new Vector3(pos.x, pos.y + 1.8f, pos.z);
                txt_EndRound = targetPlayer.fx_Devil.GetComponentInChildren<Text>();
                roundManager.cardSubject.setState(0);
                roundManager.SetCurrentState();
                uIManager.isActDone = true;
            }
        }            
    }
    private void Card_Tax(float taxPercent) //11
    {
        if (roundManager.now_Player != owner && !isAntimissile) return;
        if (owner.playerState == PlayerCtrl.PlayerState.RoadAction || isAntimissile)
        {
            OpenCard();
            if (!isAniEnd) return;
            targetPlayer = owner.otherPlayer;
            int tax = (int)(targetPlayer.PlayerInfo.Assets * taxPercent);

            if (targetPlayer != owner && IsShield(targetPlayer)) return;
            targetPlayer.fx_PayCoin2.SetActive(true);
            targetPlayer.PlayerInfo.Assets -= tax;
            fx_Pos = new(targetPlayer.transform.position.x + 0.5f, targetPlayer.transform.position.y + 4f, targetPlayer.transform.position.z);
            PlayFx(true);
            owner.SetHappy();
            targetPlayer.SetSad();
            isEnd = true;
        }
    }
    private void Card_Thief(float taxPercent) //12
    {
        if (roundManager.now_Player != owner && !isAntimissile) return;
        if (owner.playerState == PlayerCtrl.PlayerState.RoadAction || isAntimissile)
        {
            OpenCard();
            if (!isAniEnd || owner.fx_PayCoin.activeInHierarchy) return;
            targetPlayer = owner.otherPlayer;
            if (targetPlayer != owner && IsShield(targetPlayer)) return;
            fx_Pos = new(targetPlayer.transform.position.x + 0.3f, targetPlayer.transform.position.y + 3.7f, targetPlayer.transform.position.z);
            PlayFx(true);
            int tax = (int)(targetPlayer.PlayerInfo.Assets * taxPercent);
            targetPlayer.PlayerInfo.Assets -= tax;
            owner.PlayerInfo.Assets += tax;
            StartCoroutine(Fx_Coin(roundManager.other_Player));
            owner.SetHappy();
            targetPlayer.SetSad();
            //owner.uiManager.isActDone = true;
            isEnd = true;
        }
    }
    private void Card_PowerOff() //13
    {
        if (roundManager.now_Player != owner && !isAntimissile) return;
        if (owner.playerState == PlayerCtrl.PlayerState.RoadAction || isAntimissile)
        {
            if (!isAniEnd)
            {
                OpenCard();
            }
            if (canLaunch && isAniEnd)
            {
                PayTolls payTolls = uIManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("Pay_Tolls").GetComponent<PayTolls>();
                if (roundManager.roundCount != endRound && !hasLaunched)
                {
                    if (targetPlayer != owner && IsShield(targetPlayer)) return;
                    hasLaunched = true;
                    targetFloor.isTornado = false;
                    targetFloor.isPowerOff = true;
                    targetFloor.fx_Tornado.SetActive(false);
                    targetFloor.fx_PowerOff.SetActive(true);
                    txt_EndRound = targetFloor.fx_PowerOff.GetComponentInChildren<Text>();
                    targetFloor.SetTolls(targetFloor.Info.HouseLv);
                    roundManager.SetCurrentState();
                    uIManager.isActDone = true;
                    roundManager.cardSubject.setState(0);
                }
            }
            else if (!canLaunch && isAniEnd)
            {
                targetPlayer = owner.otherPlayer;
                if (owner.otherPlayer.myGounds.Count < 1)
                {
                    StartCoroutine(EndWithTip(true));
                    return;
                }
                floorBtn selectUI = uIManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("SelectGround_Urban").GetComponentInChildren<floorBtn>();
                selectUI.player = targetPlayer;
                selectUI.btnState = floorBtn.BtnState.isCard_PowerOff;
                selectUI.cardEffect = this;
                owner.OpenGroundUI("SelectGround_Urban");
                uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask.SetActive(false);
                uIManager.ui_BuyGround.GetComponent<PageCtrl>().Mask2.SetActive(true);
            }
        }
    }
    private void Card_Robbery() //14
    {
        if (roundManager.now_Player != owner && !isAntimissile) return;
        if (owner.playerState == PlayerCtrl.PlayerState.RoadAction || isAntimissile)
        {
            if (!isAniEnd)
            {
                targetPlayer = owner.otherPlayer;
                foreach (var card in targetPlayer.myCards)
                {
                    if (card.cardInfo.CanRobbery)
                    {
                        robberyCards.Add(card);
                    }
                }
                OpenCard();
            }
            if (isAniEnd)
            {
                if (robberyCards.Count < 1)
                {
                    StartCoroutine(EndWithTip(true));
                    return;
                }
                if (targetPlayer != owner && IsShield(targetPlayer)) return;
                RobberyCard();
                uIManager.isActDone = true;
                roundManager.cardSubject.setState(0);
            }
        }
    }
    private void Card_Shield() //15
    {
        OpenCard();
        if (!isAniEnd) return;
        fx_Pos = new(owner.transform.position.x + 0.1f, owner.transform.position.y + 1.5f, owner.transform.position.z);
        PlayFx(true);
        isEnd = true;
        roundManager.cardSubject.setState(0);
    }
    private void Card_Angel() //16
    {
        if (roundManager.now_Player != owner) return;
        if (owner.groundInfo.owner != owner.playerID && owner.playerState == PlayerCtrl.PlayerState.RoadAction && !owner.groundInfo.isNotRoad)
        {
            owner.isPeace = true;
            OpenCard();
            fx_Pos = new(owner.transform.position.x + 0.2f, owner.transform.position.y + 4f, owner.transform.position.z);
            PlayFx(false);
            isEnd = true;
            roundManager.cardSubject.setState(0);
        }
    }
    private void Card_Antimissile() //17
    {
        if (roundManager.now_Player != owner) return;
        OpenCard();
        if (!isAniEnd || hasLaunched) return;
        StartCoroutine(Antimissile(3));
        hasLaunched = true;
    }
    private void Card_BuildUpdate(int updateLv) //18
    {
        if (roundManager.now_Player != owner) return;
        if (owner.groundInfo.owner == owner.playerID && owner.groundInfo.Info.HouseLv < 4)
        {
            OpenCard();
            owner.CountTotalAsset();
            owner.groundInfo.Info.HouseLv += updateLv;
            owner.groundInfo.houseLv = owner.groundInfo.Info.HouseLv;
            owner.groundInfo.SetHouseLv();
            owner.groundInfo.SetTolls(owner.groundInfo.Info.HouseLv);
            isEnd = true;
        }
    }
    private void Card_BuyWithHalf(float discount) //19
    {
        if (roundManager.now_Player != owner) return;
        if (owner.groundInfo.owner != owner.playerID && !uIManager.uiManager_pageCtrl.IsOpen("Buy_OtherGround"))
        {
            OpenCard();
            if (!isAniEnd) return;
            owner.discount = discount;
            fx_Pos = new(owner.transform.position.x + 0.2f, owner.transform.position.y + 4f, owner.transform.position.z);
            PlayFx(false);
            roundManager.cardSubject.setState(0);
        }
    }
    private void Card_Peace() //20
    {
        roundManager.player1.isPeace = true;
        roundManager.player2.isPeace = true;
        if (roundManager.roundCount != endRound && roundManager.now_Player.groundInfo.owner != roundManager.now_Player.playerID)
        {
            if (!isAniEnd)
            {
                Vector3 pos1 = owner.canvas_PlayerLabel.transform.position;
                Vector3 pos2 = owner.otherPlayer.canvas_PlayerLabel.transform.position;
                OpenCard();
                owner.fx_Peace.SetActive(true);
                owner.otherPlayer.fx_Peace.SetActive(true);
                owner.canvas_PlayerLabel.transform.position = new Vector3(pos1.x, pos1.y + 1.2f, pos1.z);
                owner.otherPlayer.canvas_PlayerLabel.transform.position = new Vector3(pos2.x, pos2.y + 1.2f, pos2.z);
                txt_EndRounds.Add(roundManager.player1.fx_Peace.GetComponentInChildren<Text>());
                txt_EndRounds.Add(roundManager.player2.fx_Peace.GetComponentInChildren<Text>());
                roundManager.cardSubject.setState(0);
                roundManager.now_Player.uiManager.isActDone = true;
            }
        }
    }
    #endregion
}
