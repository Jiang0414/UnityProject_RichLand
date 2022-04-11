using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextThousand : MonoBehaviour
{
    private static TextThousand _instance;
    string txtFormat = "";
    string txtValue = "";
    public static TextThousand Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TextThousand();
            }
            return _instance;
        }
    }
    public string SetText(double number)
    {
        if(number >= 10000)
        {
            number = number > 0 ? Math.Round(number / 10000, 4) : number;
            txtFormat = number > 0 ? "0.#####.¸U" : "0";
            txtValue = number.ToString(txtFormat);
        }
        else if (number < 10000)
        {
            txtFormat = "###,###,0";
            txtValue = number.ToString(txtFormat);
        }
        return txtValue;
    }
}
