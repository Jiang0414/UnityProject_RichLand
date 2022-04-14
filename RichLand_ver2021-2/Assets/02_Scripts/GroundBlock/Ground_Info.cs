using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Ground_Info : MonoBehaviour
{
    private RoundManager round;
    [HideInInspector]
    public AreaBonus areaBonus;
    [HideInInspector]
    public int groundID;
    public GameObject fx_Tornado, fx_PowerOff;
    public SetCrownColor crownColor;
    private int area;
    private List<GameObject> houses = new List<GameObject>();
    private Text txt_Price;
    private LandData groundData;
    public GroundInfo Info;
    private AudioSource audioSource;
    public bool isNotRoad, isCelebration, isTornado, isPowerOff;
    public int owner, houseLv;
    public int buildPrice, tolls;
    public float tollsMag, soldPrice, tornadoPrice;

    private void Awake()
    {
        tollsMag = ReadGameValue.Instance.GetValue(2);
        soldPrice = ReadGameValue.Instance.GetValue(3);
        round = GameObject.FindGameObjectWithTag("RoadManager").GetComponent<RoundManager>();
        audioSource = round.GetComponent<AudioSource>();
    }
    private void Start()
    {
        groundID = Int32.Parse(Regex.Replace(gameObject.name, "[^0-9]", ""));
        if (isNotRoad) return;
        SetInfo();
    }

    private void SetInfo()
    {
        groundData = ReadGroundInfo.Instance.GetData(groundID);
        fx_Tornado = transform.Find("9_wind").gameObject;
        fx_PowerOff = transform.Find("13_notthunder").gameObject;
        areaBonus = GetComponentInParent<AreaBonus>();
        txt_Price = transform.Find("Canvas").transform.Find("txt_Price").GetComponent<Text>();
        area = Int32.Parse(transform.parent.gameObject.name.Replace("Roads_", ""));
        GetHouse();
        Info = new GroundInfo(groundID, "888,888", area, owner, houseLv, 0, 0, groundData.Build0
            , groundData.Build1, groundData.Build2, groundData.Build3, groundData.Build4);
        SetHouseLv();
        SetTolls(Info.HouseLv);
        SetSoldPrice();
        if (Info.Owner == 1)
        {
            round.player1.myGounds.Add(transform.GetComponent<Ground_Info>());
            StartCoroutine(DelaySetArea(round.player1, round.player2));
        }
        if (Info.Owner == 2)
        {
            round.player2.myGounds.Add(transform.GetComponent<Ground_Info>());
            StartCoroutine(DelaySetArea(round.player2, round.player1));
        }

    }

    private void GetHouse()
    {
        for (int h = 0; h < 5; h++)
        {
            GameObject house = transform.Find("house_" + h).gameObject;
            houses.Add(house);
        }
    }
    public void SetHouseLv()
    {
        if (Info.HouseLv > 0)
        {
            houses[0].SetActive(false);
            audioSource.Play();
            if (Info.HouseLv < 4)
            {
                for (int i = 1; i <= 4; i++)
                {
                    houses[i].SetActive(false);
                    houses[i].SetActive(Info.HouseLv >= i);
                    if (Info.HouseLv > i) houses[i].GetComponent<SetHouseColor>().SetColor();
                    if (isCelebration) crownColor.SetColor();
                }
            }
            if (Info.HouseLv == 4)
            {
                for (int i = 0; i < Info.HouseLv; i++)
                {
                    houses[i].SetActive(false);
                }
                houses[houses.Count - 1].SetActive(true);
                houses[houses.Count - 1].GetComponent<SetHouseColor>().SetColor();
                if (isCelebration) crownColor.SetColor();
            }
        }
        if (Info.HouseLv < 1)
        {
            for (int i = 1; i < houses.Count; i++)
            {
                if (houses[i].activeInHierarchy)
                {
                    houses[i].SetActive(false);
                    houses[i].GetComponent<CloseDelay>().FadeOut();
                }
                if(Info.Owner != 0)
                {
                    if (isCelebration) crownColor.SetColor();
                    houses[0].SetActive(true);
                    houses[0].GetComponent<SetHouseColor>().SetColor();
                }
                else
                {
                    if (isCelebration) closeCelebration();
                    houses[0].SetActive(false);
                }             
            }
        }
        if (Info.Owner != 0)
            txt_Price.text = TextThousand.Instance.SetText(Info.Tolls);
        else
            txt_Price.text = "";
    }
    private void closeCelebration()
    {
        isCelebration = false;
        transform.Find("crown").gameObject.SetActive(false);
        transform.Find("light").gameObject.SetActive(false);
    }
    public void SetTolls(int _Lv)
    {
        BuildedBill(_Lv);
        if (Info.Owner != 0)
        {
            Info.Tolls = isCelebration ? (int)Math.Floor((buildPrice + groundData.Urban) * tollsMag) : (int)(tolls * tollsMag);            
            Info.Tolls = isTornado ? (int)(Info.Tolls * tornadoPrice) : Info.Tolls;
            Info.Tolls = isPowerOff ? 0 : Info.Tolls;
            tolls = Info.Tolls;
            txt_Price.text = TextThousand.Instance.SetText(Info.Tolls);
        }
        else
        {
            txt_Price.text = "";
        }
    }
    public void SetSoldPrice()
    {
        Info.GroundValue = (int)Math.Ceiling(buildPrice * ReadGameValue.Instance.GetValue(7));
    }

    public void BuildedBill(int Lv)
    {
        buildPrice = 0;
        tolls = 0;
        for (int i = 0; i <= Lv; i++)
        {
            if (i == 0)
            {
                buildPrice += Info.Build0;
            }
            else if (i == 1)
            {
                buildPrice += Info.Build1;
            }
            else if (i == 2)
            {
                buildPrice += Info.Build2;
            }
            else if (i == 3)
            {
                buildPrice += Info.Build3;
            }
            else if (i == 4)
            {
                buildPrice += Info.Build4;
            }
        }
        tolls = (int)(buildPrice * ReadGameValue.Instance.GetValue(6));
        SetSoldPrice();
    }
    IEnumerator DelaySetArea(PlayerCtrl player, PlayerCtrl otherPlayer)
    {
        yield return new WaitForSeconds(0.2f);
        if (Info.Owner != 0)
            areaBonus.SetAreaBonus(player, otherPlayer);
    }
}
