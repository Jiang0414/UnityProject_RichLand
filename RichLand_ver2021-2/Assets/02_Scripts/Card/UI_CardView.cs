using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CardView : MonoBehaviour
{
    public Transform content;
    public PlayerCtrl player;

    private void OnEnable()
    {
        foreach (var card in player.myCards)
        {
            SetCard(card.cardInfo.CardID);
        }
    }
    private void SetCard(int ID) //UI_Card
    {
        GameObject card = Instantiate(Resources.Load<GameObject>("Card/UI_Card/Img_CardBack"), content.transform);
        card.GetComponent<UI_SetCard>().cardID = ID;
    }

    public void Btn_Close()
    {
        foreach (Transform card in content)
        {
            Destroy(card.gameObject);
        }
        gameObject.SetActive(false);
    }
}
