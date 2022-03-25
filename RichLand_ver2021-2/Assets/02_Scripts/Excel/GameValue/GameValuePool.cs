using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameValuePool
{
    public static GameValue m_GameValueData;
    
    public static void f_initPool()
    {
        m_GameValueData = Resources.Load<GameValue>("GameValue/GameValue");
    }
}
