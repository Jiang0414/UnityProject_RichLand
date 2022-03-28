using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private RoundManager round;    
    private Canvas canvas;
    private Canvas canvas_Fx;
    [HideInInspector]
    public GameObject ui_BuyGround;
    [HideInInspector]
    public GameInfo gameInfo;
    [HideInInspector]
    public RectTransform btn_RollDice;
    [HideInInspector]
    public GameObject ui_Cutscene;
    [HideInInspector]
    public PageCtrl uiManager_pageCtrl;
    public AudioSource audioSource;
    public GameObject card_View, card_Effect, card_Fx;

    
    public bool isActDone;
    private void Awake()
    {
        isActDone = true;
        canvas = GetComponent<Canvas>();
        audioSource = GetComponent<AudioSource>();
        card_Fx = GameObject.FindGameObjectWithTag("card_Fx");
        canvas_Fx = GameObject.FindGameObjectWithTag("Canvas_Fx").GetComponent<Canvas>();
        uiManager_pageCtrl = GetComponent<PageCtrl>();
        round = GameObject.FindGameObjectWithTag("RoadManager").GetComponent<RoundManager>();
        ui_BuyGround = canvas.transform.Find("Ground_trade").gameObject;
        ui_Cutscene = canvas.transform.Find("Game_Cutscene").gameObject;
        gameInfo = transform.Find("GameInfo").GetComponent<GameInfo>();
        btn_RollDice = canvas_Fx.transform.Find("Btn_Roll").GetComponent<RectTransform>();
        //card_view = GameObject.FindGameObjectWithTag("Card_View");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            round.player1.playerState = PlayerCtrl.PlayerState.Win;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            round.player1.playerState = PlayerCtrl.PlayerState.Lose;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            round.player1.SetIdle();
            round.player1.ui_Win.SetActive(false);
            round.player1.ui_Lose.SetActive(false);
            round.player1.playerState = PlayerCtrl.PlayerState.Stay;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            //round.cardSubject.setState(0);
            Debug.Log(round.cardSubject.GetState() + "@");
            Debug.Log(round.round_Player.GetState() + "!!");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            round.now_Player.road_Dice = 4;
            round.now_Player.playerState = PlayerCtrl.PlayerState.Move;
        }
        /*if (Input.GetKeyDown(KeyCode.Z))
        {
            round.now_Player.road_Dice = 16;
            round.now_Player.playerState = PlayerCtrl.PlayerState.Move;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            card_View.GetComponent<Card>().GetCard(17, round.player2);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            round.now_Player.RemoveCard(7);
        }*/
        /*if (Input.GetKeyDown(KeyCode.Z))
        {
            //round.cardSubject.setState(0);
            card_View.GetComponent<Card>().GetCard(4, round.player1);
            card_View.GetComponent<Card>().GetCard(15, round.player1);
            card_View.GetComponent<Card>().GetCard(17, round.player1);
            card_View.GetComponent<Card>().GetCard(5, round.player1);
            card_View.GetComponent<Card>().GetCard(12, round.player1);
            card_View.GetComponent<Card>().GetCard(5, round.player2);
            card_View.GetComponent<Card>().GetCard(8, round.player2);
            card_View.GetComponent<Card>().GetCard(1, round.player2);
            card_View.GetComponent<Card>().GetCard(2, round.player2);
            card_View.GetComponent<Card>().GetCard(20, round.player2);
            card_View.GetComponent<Card>().GetCard(16, round.player2);
            card_View.GetComponent<Card>().GetCard(6, round.player2);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            round.now_Player.road_Dice = 8;
            round.now_Player.playerState = PlayerCtrl.PlayerState.Move;
        }*/
    }



    //過場並切換UI
    public void CutScenesSwitch(PlayerCtrl player,  string actionTip, string actionUI, float sec)
    {
        StartCoroutine(Cut_ScenesSwitch(actionTip, actionUI, sec, player));
    }
    //過場
    public void CutScenes(PlayerCtrl player, string actionTip, float sec)
    {
        StartCoroutine(Cut_Scenes(actionTip, sec, player));
    }
    public void TipAndClose(PlayerCtrl player, string actionTip, string msg, float sec)
    {
        StartCoroutine(TipScene(actionTip, msg, sec, player));
    }
    #region 過場等待
    //過場並切換UI
    private IEnumerator Cut_ScenesSwitch(string actionTip, string actionUI, float sec, PlayerCtrl player)
    {
        uiManager_pageCtrl.SwitchToPage("Game_Cutscene");
        ui_Cutscene.GetComponent<PageCtrl>().SwitchToPage(actionTip);
        player.isCutscenes = true;
        yield return new WaitForSeconds(sec);
        ui_Cutscene.GetComponent<PageCtrl>().Close_Page(actionTip);
        player.isCutscenes = false;
        uiManager_pageCtrl.SwitchToPage("Ground_trade");
        ui_BuyGround.GetComponent<PageCtrl>().SwitchToPage(actionUI);
    }
    //過場
    private IEnumerator Cut_Scenes(string actionTip, float sec, PlayerCtrl player)
    {
        uiManager_pageCtrl.Open_Page("Game_Cutscene");
        player.isCutscenes = true;
        ui_Cutscene.GetComponent<PageCtrl>().SwitchToPage(actionTip);
        yield return new WaitForSeconds(sec);
        ui_Cutscene.GetComponent<PageCtrl>().Close_Page(actionTip);
        player.isCutscenes = false;
        uiManager_pageCtrl.Close_Page("Game_Cutscene");
    }
    private IEnumerator TipScene(string actionTip, string msg, float sec, PlayerCtrl player)
    {
        uiManager_pageCtrl.Open_Page("Game_Cutscene");
        player.isCutscenes = true;
        Text txtTip = uiManager_pageCtrl.GetPage("Game_Cutscene").GetComponent<PageCtrl>().GetPage(actionTip).GetComponentInChildren<Text>();
        txtTip.text = msg;
        ui_Cutscene.GetComponent<PageCtrl>().SwitchToPage(actionTip);
        yield return new WaitForSeconds(sec);
        ui_Cutscene.GetComponent<PageCtrl>().Close_Page(actionTip);
        player.isCutscenes = false;
        uiManager_pageCtrl.Close_Page("Game_Cutscene");
        isActDone = true;
    }
    #endregion
}
