using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetCard : MonoBehaviour
{
    private List<CardDataData> cardDatas;
    public Text txt_Name, txt_Content;
    public Image img_View;
    public int cardID;
    private void Start()
    {
        CardDataPool.f_initPool();
        cardDatas = CardDataPool.m_cardData.dataList;

        txt_Name = transform.Find("txt_Name").GetComponent<Text>();
        txt_Content = transform.Find("txt_Content").GetComponent<Text>();
        img_View = transform.Find("Img_card").GetComponent<Image>();
        Set();
    }
    private void Set()
    {
        CardDataData cardInfo = cardDatas[cardID - 1];
        txt_Name.text = cardInfo.Name;
        txt_Content.text = cardInfo.Content;
        img_View.sprite = Resources.Load<Sprite>("Card/Cards_Textues/" + cardID);
    }
}
