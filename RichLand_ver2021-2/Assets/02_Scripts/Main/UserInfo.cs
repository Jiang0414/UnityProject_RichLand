using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{

    public static string characterName;

    private static UserInfo _instance;

    public static UserInfo Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UserInfo();
            }
            return _instance;
        }
    }

}
