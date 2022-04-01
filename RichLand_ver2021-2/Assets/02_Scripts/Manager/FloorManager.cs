using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public List<Transform> floors = new List<Transform>();
    private bool HaveNoOwnerFloor;
    private void Awake()
    {
        GetRoads(); 
    }

    private void GetRoads()
    {
        GameObject[] objects;
        objects = GameObject.FindGameObjectsWithTag("road");
        foreach (var child in objects)
        {
            floors.Add(child.transform);
        }
        //�ϥΦW�٤����Ʀr���̾ڱƧ�List�C�NList�W�٫O�d�Ʀr�r����নint
        floors.Sort((x, y) => Int32.Parse(Regex.Replace(x.name, "[^0-9]", "")).CompareTo(Int32.Parse(Regex.Replace(y.name, "[^0-9]", ""))));
    }
    public bool CheckTerritory()
    {
        foreach (var floor in floors)
        {
            if (floor.GetComponent<Ground_Info>().owner == 0 && !floor.GetComponent<Ground_Info>().isNotRoad)
            {
                HaveNoOwnerFloor = true;
                break;
            }
            HaveNoOwnerFloor = false;
        }
        return HaveNoOwnerFloor;
    }
}
