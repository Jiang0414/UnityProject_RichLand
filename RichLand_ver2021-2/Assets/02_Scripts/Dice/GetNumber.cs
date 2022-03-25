using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNumber : MonoBehaviour
{
    public RandomDice dice;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "floor")
        {
            dice.number = Int32.Parse(transform.name);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "floor")
        {
            dice.number = 0;
        }
    }
}
