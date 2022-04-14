using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AreaBonus : MonoBehaviour
{
    [HideInInspector]
    public List<Ground_Info> floors = new List<Ground_Info>();
    private AreaBonus otherArea;

    [HideInInspector]
    public bool isFrontStreat;

    public GameObject bonus, bonus2;

    private void Awake()
    {
        foreach (Transform floor in transform)
        {
            if (floor.name.Contains("floor_"))
            {
                floors.Add(floor.GetComponent<Ground_Info>());
            }
        }

        isFrontStreat = Int32.Parse(Regex.Replace(transform.name, "[^0-9]", "")) % 2 != 0 ? true : false;
        bonus = transform.Find("Bonus").gameObject;
        if (isFrontStreat)
        {
            otherArea = transform.parent.Find("Roads_" + (Int32.Parse(Regex.Replace(transform.name, "[^0-9]", "")) + 1)).GetComponent<AreaBonus>();
            bonus2 = transform.Find("Bonus2").gameObject;
        }
        else
        {
            otherArea = transform.parent.Find("Roads_" + (Int32.Parse(Regex.Replace(transform.name, "[^0-9]", "")) - 1)).GetComponent<AreaBonus>();
        }
    }

    public void SetAreaBonus(PlayerCtrl player, PlayerCtrl otherplayer)
    {
        bonus.SetActive(false);        
        if(bonus2 != null)
            bonus2.SetActive(false);

        SetArea(player, otherplayer);
        if (bonus2 != null)
        {
            if(isFrontStreat && IsStreat())
            {
                SetTollsMag(ReadGameValue.Instance.GetValue(5), floors);
                SetTollsMag(ReadGameValue.Instance.GetValue(5), otherArea.floors);
                bonus2.SetActive(true);
                bonus.SetActive(false);
                otherArea.bonus.SetActive(false);
                player.SetHappy();
                otherplayer.SetSad();
            }
            else if (isFrontStreat && !IsStreat())
            {
                SetArea(player, otherplayer);
                otherArea.SetArea(player, otherplayer);
            }
        }
        else
        {
            otherArea.SetAreaBonus(player, otherplayer);
        }
    }
    public void SetArea(PlayerCtrl player, PlayerCtrl otherplayer)
    {
        if (IsArea())
        {
            bonus.SetActive(true);
            SetTollsMag(ReadGameValue.Instance.GetValue(4), floors);
            player.SetHappy();
            otherplayer.SetSad();
        }
        else
        {
            if (bonus.activeInHierarchy)
            {
                player.SetSad();
                otherplayer.SetHappy();
            }
            bonus.SetActive(false);
            SetTollsMag(ReadGameValue.Instance.GetValue(6), floors);
        }
    }
    public void SetTollsMag(float Mag, List<Ground_Info> floors)
    {
        foreach (var floor in floors)
        {
            floor.tollsMag = Mag;
            floor.SetTolls(floor.Info.HouseLv);
            floor.SetSoldPrice();
        }
    }

    public bool IsArea()
    {
        for (int i = 0; i < floors.Count - 1; i++)
        {
            if (floors[i].Info.Owner != floors[i + 1].Info.Owner || floors[i].Info.Owner == 0 || floors[i + 1].Info.Owner == 0)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsStreat()
    {
        if (IsArea() && otherArea.IsArea())
        {
            if (floors[0].Info.Owner == otherArea.floors[0].Info.Owner && floors[0].Info.Owner != 0 && otherArea.floors[0].Info.Owner != 0)
            {
                return true;
            }
        }
        return false;
    }
}
