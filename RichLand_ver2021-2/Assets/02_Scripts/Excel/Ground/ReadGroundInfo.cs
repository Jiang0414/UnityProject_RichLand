using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadGroundInfo : MonoBehaviour
{
    public static ReadGroundInfo _instance = null;
    public static List<LandData> groundDatas;
    public LandData groundData;
    public static ReadGroundInfo Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ReadGroundInfo();
            }
            if (groundDatas == null)
            {
                GroundPool.f_initPool();
                groundDatas = GroundPool.m_LandValueData.dataList;
            }
            return _instance;
        }
    }
    public LandData GetData(int groundID)
    {
        foreach (var value in groundDatas)
        {
            if (value.ID == groundID)
            {
                groundData = value;
                break;
            }
        }
        return groundData;
    }
}
