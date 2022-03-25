using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    #region 腳本資訊
    ConcreteObserver_1 cardObserver;
    [HideInInspector]
    public RoundManager roundManager;
    [HideInInspector]
    public UIManager uIManager;
    private Text txt_Name, txt_Content;
    #endregion

    [HideInInspector]
    public List<int> IdPool = new List<int>();
    public bool isView;

    [HideInInspector]
    public Material material;
    [HideInInspector]
    public PlayerCtrl owner;
    [HideInInspector]
    public List<CardDataData> cardDatas;

    #region 卡片資訊
    [HideInInspector]
    public int cardID;
    [HideInInspector]
    public CardEffect cardEffect;
    [HideInInspector]
    public bool isDirect;
    #endregion

    private int getRound;
    private int endRound;
    private int[] cardIDs = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }; //1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20
    private void Awake()
    {
        txt_Name = transform.Find("Face").transform.Find("Canvas_Card").transform.Find("Txt_Name").GetComponent<Text>();
        txt_Content = transform.Find("Face").transform.Find("Canvas_Card").transform.Find("Txt_Content").GetComponent<Text>();
        uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        roundManager = GameObject.FindGameObjectWithTag("RoadManager").GetComponent<RoundManager>();
        material = transform.Find("Face").transform.Find("View").GetComponent<MeshRenderer>().material;
        LoadCardDatas();
    }
    private void OnEnable()
    {
        if (!isView)
        {
            CardDataData cardInfo = cardDatas[cardID - 1];
            material.SetTexture("_BaseMap", CardManager.Instance.GetCardTextues(cardID)); //CardManager.Instance.cardSprites[cardID - 1]
            txt_Name.text = cardInfo.Name;
            txt_Content.text = cardInfo.Content;
        }
        if (isView) SetCardInfo();
    }

    public void SetCardInfo()
    {
        SetCardIdPool(cardIDs.Length, 0);
        cardID = RandomCard();
        IdPool.Clear();

        CardDataData cardInfo = cardDatas[cardID - 1];
        isDirect = cardInfo.Isdirect;
        material.SetTexture("_BaseMap", CardManager.Instance.GetCardTextues(cardID));
        txt_Name.text = cardInfo.Name;
        txt_Content.text = cardInfo.Content;
        
        CreateCard(cardInfo); //生成卡片
    }
    public void GetCard(int cardId, PlayerCtrl player)
    {
        owner = player;
        
        cardID = cardId;
        IdPool.Clear();

        CardDataData cardInfo = cardDatas[cardID - 1];
        isDirect = cardInfo.Isdirect;
        material.SetTexture("_BaseMap", CardManager.Instance.GetCardTextues(cardID));
        txt_Name.text = cardInfo.Name;
        txt_Content.text = cardInfo.Content;
        CreateCard(cardInfo); //生成卡片
    }

    private void LoadCardDatas()
    {
        CardDataPool.f_initPool();
        cardDatas = CardDataPool.m_cardData.dataList;
    }
    private void SetCardIdPool(int poolLength , int start)
    {
        if(owner.myCards.Count == poolLength)
        {
            owner.uiManager.isActDone = true;
            gameObject.SetActive(false);
            return;
        }
        for (int i = start; i < poolLength + start; i++)
        {
            IdPool.Add(cardIDs[i]);
        }
    }
    private int RandomCard()
    {
        if (IdPool.Count < 1) return 0;
        int tempID;
        tempID = Random.Range(0, IdPool.Count);
        if (owner.isHaveCard(IdPool[tempID]))
        {
            IdPool.RemoveAt(tempID);            
            return RandomCard();
        }
        return IdPool[tempID];
    }

    #region 生成卡片
    private void CreateCard(CardDataData info)
    {
        GameObject card = (GameObject)Instantiate(Resources.Load("Card/Card"), owner.cardObject.transform);
        card.name = card.name.Replace("(Clone)", "_" + cardID.ToString());
        CardEffect cardEffect = card.GetComponent<CardEffect>();
        cardEffect.uIManager = uIManager;
        cardEffect.roundManager = roundManager;
        cardEffect.cardView = uIManager.card_Effect.GetComponent<Card>();
        cardEffect.owner = owner;
        cardEffect.isAniEnd = false;
        cardEffect.isOpen = false;
        cardEffect.isEnd = false;
        if (info.Isdirect)
        {
            cardEffect.getRound = roundManager.roundCount;
            cardEffect.endRound = cardEffect.getRound + info.Continued;
        }
        cardObserver = new ConcreteObserver_1(cardID.ToString() + "_Player" + owner, roundManager.cardSubject);
        cardEffect.cardInfo = new CardInfo(cardID, info.Continued, info.Effectvalue, info.Isdirect, info.Isanit, info.Canrobbery, getRound, endRound, cardObserver);
        owner.myCards.Add(cardEffect);
        cardEffect.GetFx();
    }
    #endregion
}
