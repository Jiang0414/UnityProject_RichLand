using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPool : MonoBehaviour
{
    public static Land m_LandValueData;

    public static void f_initPool()
    {
        m_LandValueData = Resources.Load<Land>("GroundInfo/Land");
    }
}
