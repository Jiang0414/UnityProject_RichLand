using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataPool 
{
    public static CardData m_cardData;
    public static void f_initPool()
    {
        m_cardData = Resources.Load<CardData>("Card/CardDatas/CardData");
    }

}
