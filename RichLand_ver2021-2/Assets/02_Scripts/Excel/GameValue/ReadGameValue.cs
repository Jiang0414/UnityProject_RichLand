using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadGameValue : MonoBehaviour
{
    public static ReadGameValue _instance = null;
    public static List<GameValueData> gameValueDatas;
    public float gameValue;
    public static ReadGameValue Instance 
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ReadGameValue();
            }
            if (gameValueDatas == null)
            {
                GameValuePool.f_initPool();
                gameValueDatas = GameValuePool.m_GameValueData.dataList;
            }
            return _instance;
        }
    }
    public float GetValue(int ContentID)
    {
        foreach (var value in gameValueDatas)
        {
            if (value.ID == ContentID)
            {
                gameValue = value.Contentvalue;
                break;
            }
        }
        return gameValue;
    }
}
