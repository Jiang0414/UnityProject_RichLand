using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    private RoundManager roundManager;
    private UI_ShowNumber showNumber;
    public FloorManager roads;
    public UIManager uiManager;
    public Animator animator;
    public AudioSource audioSource;
    public RandomDice dice1, dice2;
    public PlayerCtrl otherPlayer;
    [HideInInspector]
    public int playerID;
    [HideInInspector]
    public int road_Now, road_Dice;
    [HideInInspector]
    public float discount, devilTolls;
    #region ActionBool
    [HideInInspector]
    public bool canThrow;
    [HideInInspector]
    public bool toMove;
    [HideInInspector]
    public int passCount;
    [HideInInspector]
    public bool toMRT;
    [HideInInspector]
    public bool toUrban;
    [HideInInspector]
    public bool toSoldHouse;
    [HideInInspector]
    public bool isCutscenes;
    [HideInInspector]
    public bool isPay;
    [HideInInspector]
    public bool isPeace;
    [HideInInspector]
    public bool isDevil;
    [HideInInspector]
    public bool haveFirstAct;
    [HideInInspector]
    public bool isTp;
    [HideInInspector]
    public bool gettedCard;
    private bool ispass;
    private bool isWalkSound;

    #endregion
    public GameObject fx_PayCoin, fx_PayCoin2, fx_Devil, fx_Peace;
    [HideInInspector]
    public GameObject canvas_PlayerLabel;
    private Text txtLabel;
    private Image imgLabel;
    private Outline lineLabel;

    [HideInInspector]
    public float playerLabel_OldPos;

    #region 玩家資訊
    public int diceNumber;
    public float moveSpeed;
    public int turnCount;
    public Text Txt_Asset, Txt_TotalAsset;
    #endregion
    [HideInInspector]
    public List<Ground_Info> myGounds = new List<Ground_Info>();
    [HideInInspector]
    public List<CardEffect> myCards = new List<CardEffect>();
    [HideInInspector]
    public GameObject cardObject;

    public PlayerInfo PlayerInfo;
    public Ground_Info groundInfo;
    ConcreteObserver_1 observer_1;

    [HideInInspector]
    public GameObject ui_Win, ui_Lose;

    public enum PlayerState
    {
        Stay,
        ThrowTheDice,
        Move,
        RoadAction,
        Pass,
        ActEnd,
        Win,
        Lose
    }
    public PlayerState playerState;
    private void Awake()
    {
        road_Now = 0;
        turnCount = 0;
        passCount = 0;
        discount = 0;

        toMRT = false;
        isPay = false;
        toUrban = false;
        toSoldHouse = false;
        isCutscenes = false;
        playerState = PlayerState.Stay;
        animator = transform.GetChild(0).GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        showNumber = GetComponent<UI_ShowNumber>();
        dice1 = GameObject.FindGameObjectWithTag("dice" + playerID.ToString() + "_1").GetComponent<RandomDice>();
        dice2 = GameObject.FindGameObjectWithTag("dice" + playerID.ToString() + "_2").GetComponent<RandomDice>();        
        fx_PayCoin = GameObject.FindGameObjectWithTag("Fx_PayCoin").transform.Find("Fx_PayCoin" + playerID.ToString()).gameObject;
        fx_PayCoin2 = GameObject.FindGameObjectWithTag("Fx_PayCoin").transform.Find(playerID.ToString() + "P").transform.Find("fx_Pay").gameObject;
        fx_Devil = transform.Find("Card_Fx").transform.Find("10_spine_deveil").gameObject;
        fx_Peace = transform.Find("Card_Fx").transform.Find("20_peacehand").gameObject;
        canvas_PlayerLabel = transform.Find("Canvas_PlayerLabel").gameObject;
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        Txt_Asset = uiManager.transform.Find("PlayerInfo").Find("Player_" + playerID).Find("BG").Find("Info").Find("Text_Info").Find("Txt_Asset").GetComponent<Text>();
        Txt_TotalAsset = uiManager.transform.Find("PlayerInfo").Find("Player_" + playerID).Find("BG").Find("Info").Find("Text_Info").Find("TotalAsset").Find("Txt_TotalAsset").GetComponent<Text>();
        roads = GameObject.FindGameObjectWithTag("RoadManager").GetComponent<FloorManager>();
        roundManager = roads.gameObject.GetComponent<RoundManager>();
        ui_Win = uiManager.transform.Find("Settlement").transform.Find("Victory").gameObject;
        ui_Lose = uiManager.transform.Find("Settlement").transform.Find("Lose").gameObject;
        cardObject = transform.Find("Cards").gameObject;
        observer_1 = new ConcreteObserver_1("Player_" + playerID.ToString(), roundManager.round_Player);
        roundManager.round_Player.Attach(observer_1);
        dice1.gameObject.SetActive(false);
        dice2.gameObject.SetActive(false);
        playerLabel_OldPos = canvas_PlayerLabel.transform.position.y;

        txtLabel = canvas_PlayerLabel.GetComponentInChildren<Text>();
        imgLabel = canvas_PlayerLabel.GetComponentInChildren<Image>();
        lineLabel = canvas_PlayerLabel.GetComponentInChildren<Outline>();
        SetInfo();
    }
    private void Start()
    {
        otherPlayer = playerID < 2 ? roundManager.player2 : roundManager.player1;
        StartCoroutine(DelayCount());
        groundInfo = roads.floors[0].GetComponent<Ground_Info>();
    }
    private void Update()
    {
        PlayerAction();
        if (Txt_Asset != null && Txt_TotalAsset != null)
        {
            Txt_Asset.text = TextThousand.Instance.SetText(PlayerInfo.Assets);
            Txt_TotalAsset.text = TextThousand.Instance.SetText(PlayerInfo.TotalAssets);
        }
    }
    private void FixedUpdate()
    {
        SetLabel();
    }
    public void SetInfo()
    {
        //int money = playerID == 1 ? 90000 : (int)ReadGameValue.Instance.GetValue(1);
        //PlayerInfo = new PlayerInfo(playerID, money, 0);
        PlayerInfo = new PlayerInfo(playerID, (int)ReadGameValue.Instance.GetValue(1), 0);
    }
    #region 擲骰子
    public void ThrowDice()
    {
        if (!canThrow) return;
        dice1.ThrowTheDice();
        dice2.ThrowTheDice();
        uiManager.btn_RollDice.GetComponent<Animator>().SetTrigger("out");
        canThrow = false;
        if (uiManager.uiManager_pageCtrl.GetPage("Game_Cutscene").GetComponent<PageCtrl>().GetPage("TravelPay").activeInHierarchy)
        {
            uiManager.uiManager_pageCtrl.Close_Page("Game_Cutscene");
            uiManager.uiManager_pageCtrl.GetPage("Game_Cutscene").GetComponent<PageCtrl>().Close_Page("TravelPay");
        }
    }
    //移動骰子判定
    private void IsDiceStop() //移動
    {
        if (dice1.frozen && dice2.frozen)
        {
            SetHappy();
            playerState = PlayerState.Move;
            diceNumber = dice1.number + dice2.number;
            road_Dice = diceNumber + road_Now;
            showNumber.OpenDiceNumberView(diceNumber);
            dice1.throwed = false;
            dice2.throwed = false;
            dice1.frozen = false;
            dice2.frozen = false;
        }
    }
    //度假骰子判定
    private void CornerVacation()
    {
        if (dice1.frozen)
        {
            diceNumber = dice1.number;
            showNumber.OpenDiceNumberView(diceNumber);
            Corner.Instance.CornerEvent(groundInfo.groundID, GetComponent<PlayerCtrl>());
            dice1.throwed = false;
            dice2.throwed = false;
            dice1.frozen = false;
            dice2.frozen = false;
        }
    }
    #endregion

    #region 角色移動
    private void Move()
    {
        int temp = road_Dice > roads.floors.Count - 1 ? road_Dice - roads.floors.Count : 0;
        road_Dice = road_Dice > roads.floors.Count - 1 ? road_Dice - roads.floors.Count : road_Dice;
        int nowRoad = road_Now + 1 > roads.floors.Count - 1 ? -1 : road_Now;

        if (road_Now != road_Dice)
        {
            if (Vector3.Distance(transform.position, roads.floors[nowRoad + 1].localPosition) > 0.01f)
            {
                if (!isWalkSound)
                {
                    PlayAudioVolume(14);
                    isWalkSound = true;
                }
                transform.position = Vector3.MoveTowards(transform.position, roads.floors[nowRoad + 1].localPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                if (isWalkSound) isWalkSound = false;
                if (nowRoad < 0)
                {
                    turnCount += 1;
                    Corner.Instance.Corner_Start(GetComponent<PlayerCtrl>());
                }
                road_Now = road_Now + 1 > roads.floors.Count - 1 ? 0 : road_Now + 1;
            }
        }
        else
        {
            if (temp != 0)
            {
                road_Dice = temp;
                temp = 0;
                return;
            }
            uiManager.isActDone = false;
            playerState = PlayerState.RoadAction;
        }
    }
    #endregion

    #region 玩家回合狀態
    private void PlayerAction()
    {
        if (dice1.gameObject == null)
        {
            return;
        }
        switch (playerState)
        {
            case PlayerState.Stay:                
                if (playerID == observer_1.m_state)
                {
                    if (passCount > 0)
                    {
                        uiManager.isActDone = false;
                        playerState = PlayerState.Pass;
                        break;
                    }
                    if (toMRT || haveFirstAct)
                    {
                        road_Dice = road_Now;
                        uiManager.isActDone = false;
                        playerState = PlayerState.RoadAction;
                        break;
                    }
                    if (passCount < 1 && !toMRT && !haveFirstAct)
                    {
                        dice1.gameObject.SetActive(true);
                        dice2.gameObject.SetActive(true);
                        canThrow = true;
                        toMove = true;
                        uiManager.btn_RollDice.gameObject.SetActive(true);
                        playerState = PlayerState.ThrowTheDice;
                        break;
                    }
                }
                if (observer_1.m_state == 3)
                {
                    playerState = PlayerState.Win;
                }
                break;

            case PlayerState.ThrowTheDice:
                {

                    if (toMove)
                    {
                        IsDiceStop();
                    }
                    else
                    {
                        CornerVacation();
                    }
                }
                break;

            case PlayerState.Move:
                {
                    Move();
                }
                break;

            case PlayerState.RoadAction:
                {
                    if (uiManager.isActDone)
                    {
                        playerState = PlayerState.ActEnd;
                        break;
                    }
                    haveFirstAct = false;
                    if(!isTp)
                        groundInfo = roads.floors[road_Dice].GetComponent<Ground_Info>();

                    if (uiManager.ui_BuyGround.activeInHierarchy || isCutscenes || uiManager.card_View.activeInHierarchy) return;
                   
                    if (!groundInfo.isNotRoad)
                    {
                        BuyGound();
                    }
                    else if (groundInfo.isNotRoad && !uiManager.ui_BuyGround.activeInHierarchy)
                    {
                        if (groundInfo.name.Contains("corner"))
                        {
                            Corner.Instance.CornerEvent(groundInfo.groundID, GetComponent<PlayerCtrl>());
                        }
                        else if (groundInfo.name.Contains("card") && !gettedCard) //機會命運卡
                        {
                            uiManager.card_View.GetComponent<Card>().owner = this;
                            uiManager.card_View.SetActive(true);
                            gettedCard = true;
                        }
                    }
                }
                break;
            case PlayerState.Pass:
                {
                    if (ispass) return;
                    StartCoroutine(PassDelay(this, "GOtraveling", 1.2f));
                    break;
                }
            case PlayerState.ActEnd:
                {
                    isTp = false;
                    isPay = false;
                    canThrow = false;
                    gettedCard = false;
                    playerState = PlayerState.Stay;
                    dice1.gameObject.SetActive(false);
                    dice2.gameObject.SetActive(false);
                    dice1.transform.position = dice1.inPos;
                    dice2.transform.position = dice2.inPos;
                    if (otherPlayer.isHaveCard(10))
                    {
                        CardEffect card = otherPlayer.Card(10);
                        card.cardInfo.ContinuedRound -= 1;
                    }
                    if (roundManager.now_Player.playerID.Equals(playerID))
                        roundManager.roundTurn = playerID != 2 ? RoundManager.RoundCount.Switch : RoundManager.RoundCount.countRound;

                    break;
                }
            case PlayerState.Win:
                {
                    if (!ui_Win.activeInHierarchy && playerID != 2) //目前只顯示玩家1的勝利失敗
                    {
                        ui_Win.SetActive(true);
                        ui_Lose.SetActive(false);
                    }
                    SetWin();
                    Debug.Log("玩家" + playerID + "勝利");
                    break;
                }
            case PlayerState.Lose:
                {
                    if (!ui_Lose.activeInHierarchy && playerID != 2)
                    {
                        ui_Win.SetActive(false);
                        ui_Lose.SetActive(true);
                    }
                    SetLose();
                    Debug.Log("玩家" + playerID + "破產");
                    break;
                }
        }
    }
    #endregion

    #region 建造確認
    private int BuildPricce(int lv)
    {
        int[] builds = { groundInfo.Info.Build0, groundInfo.Info.Build1, groundInfo.Info.Build2, groundInfo.Info.Build3, groundInfo.Info.Build4 };
        
        return builds[lv];
    }
    #endregion

    #region 機會命運卡
    public bool isHaveCard(int cardInfo)
    {
        foreach (var card in myCards)
        {
            if (card.cardInfo.CardID.Equals(cardInfo)) return true;
        }
        return false;
    }

    public void RemoveCard(int cardID)
    {
        for(int i = 0; i < myCards.Count; i++)
        {
            if (myCards[i].cardInfo.CardID == cardID)
            {
                myCards.Remove(myCards[i]);
                break;
            }                
        }             
    }

    public CardEffect Card(int cardID)
    {
        foreach (var card in myCards)
        {
            if (card.cardInfo.CardID.Equals(cardID)) return card;
        }
        return null;
    }
    #endregion

    #region 玩家動作
    public void OpenGroundUI(string pageName) //開啟土地相關UI
    {
        if (!uiManager.ui_BuyGround.active)
        {
            uiManager.ui_BuyGround.SetActive(true);
            uiManager.ui_BuyGround.GetComponent<PageCtrl>().Mask.SetActive(false);
            uiManager.ui_BuyGround.GetComponent<PageCtrl>().Mask.SetActive(true);
            uiManager.ui_BuyGround.GetComponent<PageCtrl>().SwitchToPage(pageName);
        }
    }
    public void BuyGound()
    {
        switch (groundInfo.Info.Owner)
        {
            case (0):
                {
                    if (PlayerInfo.Assets < BuildPricce(0))
                    {
                        uiManager.TipAndClose(this, "NoMoney", "存款不足", 1.2f);
                        return;
                    }
                    uiManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("Buy_Ground").GetComponent<BuyGround>().player = this;
                    OpenGroundUI("Buy_Ground");
                    Debug.Log("道路" + groundInfo.Info.ID + "是空地");
                    break;
                }
            case (1):
                {
                    GroundAction();
                    break;
                }
            case (2):
                {
                    GroundAction();
                    break;
                }
        }

    }

    private void GroundAction()
    {
        if (groundInfo.Info.Owner != playerID && groundInfo.Info.Owner != 0) //不是我的土地
        {
            if (isPay) return;
            roundManager.cardSubject.setState(20);
            uiManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("Pay_Tolls").GetComponent<PayTolls>().player = this;
            OpenGroundUI("Pay_Tolls");  //支付過路費UI
        }
        else
        {
            if (groundInfo.Info.HouseLv < 4 && isHaveCard(18))
            {
                roundManager.cardSubject.setState(18);
            }
            else if (groundInfo.Info.HouseLv < 4 && !isHaveCard(18) && roundManager.cardSubject.GetState() != 6) //土地不是最高級
            {
                if (PlayerInfo.Assets < BuildPricce(groundInfo.Info.HouseLv + 1))
                {
                    uiManager.TipAndClose(this, "NoMoney", "存款不足", 1.2f);
                    return;
                }
                uiManager.ui_BuyGround.GetComponent<PageCtrl>().GetPage("Buy_Ground").GetComponent<BuyGround>().player = this;
                OpenGroundUI("Buy_Ground"); //購買土地UI
            }
            else if (groundInfo.Info.HouseLv > 3)
            {
                playerState = PlayerState.ActEnd;
            }
        }
        Debug.Log("道路" + groundInfo.Info.ID + "房屋等級" + groundInfo.Info.HouseLv + "屬於玩家" + groundInfo.Info.Owner);
    }

    IEnumerator PassDelay(PlayerCtrl player, string uiTip, float sec)
    {
        canThrow = false;
        ispass = true;
        uiManager.CutScenes(player, uiTip, sec);
        Text txt_Travil = uiManager.uiManager_pageCtrl.GetPage("Game_Cutscene").GetComponent<PageCtrl>().GetPage("GOtraveling").GetComponentInChildren<Text>();
        txt_Travil.text = "玩家" + playerID + " 旅遊中";
        SetSad();
        passCount = passCount - 1 < 0 ? 0 : passCount - 1;
        uiManager.btn_RollDice.gameObject.SetActive(false);
        yield return new WaitForSeconds(sec + 0.1f);
        ispass = false;
        uiManager.isActDone = true;
        uiManager.ui_BuyGround.SetActive(false);
        uiManager.ui_BuyGround.transform.Find("BG_Panel").gameObject.SetActive(true);
        playerState = PlayerState.ActEnd;
    }

    #endregion

    #region 玩家總資產計算
    public void CountTotalAsset()
    {
        PlayerInfo.TotalAssets = 0;
        foreach (var _gound in myGounds)
        {
            PlayerInfo.TotalAssets += _gound.Info.GroundValue;
        }
    }

    IEnumerator DelayCount()
    {
        yield return new WaitForSeconds(0.5f);
        CountTotalAsset();
    }

    #endregion

    #region 角色動作
    public void SetHappy()
    {
        PlayAudioVolume(15);
        animator.SetTrigger("happy");
    }

    public void SetSad()
    {
        PlayAudioVolume(16);
        animator.SetTrigger("sad");
    }

    public void SetWin()
    {
        PlayAudioVolume(15);
        animator.SetBool("Win", true);
    }

    public void SetLose()
    {
        PlayAudioVolume(16);
        animator.SetBool("Lose", true);
    }

    public void SetIdle()
    {
        animator.ResetTrigger("Win");
        animator.ResetTrigger("sad");
        animator.SetBool("Win", false);
        animator.SetBool("Lose", false);
    }

    private void PlayAudioVolume(int clip)
    {
        audioSource.clip = PlayAudio.audiosList[clip];
        audioSource.Play();
    }

    #endregion

    #region 標籤透明度
    public void SetLabel()
    {
        if (groundInfo == null) return;

        if(uiManager.btn_RollDice.gameObject.activeInHierarchy && (groundInfo.groundID == 0 || groundInfo.groundID == 1 || groundInfo.groundID == 31))
        {
            if(playerID == 1)
                txtLabel.color = new Color(0, 0, 1, 0.5f);
            else if (playerID == 2)
                txtLabel.color = new Color(1, 0, 0, 0.5f);
            imgLabel.color = new Color(1, 1, 1, 0.5f);
            lineLabel.effectColor = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            if (playerID == 1)
                txtLabel.color = new Color(0, 0, 1, 1);
            else if (playerID == 2)
                txtLabel.color = new Color(1, 0, 0, 1);
            imgLabel.color = new Color(1, 1, 1, 1);
            lineLabel.effectColor = new Color(1, 1, 1, 1);
        }
    }
    #endregion
}
