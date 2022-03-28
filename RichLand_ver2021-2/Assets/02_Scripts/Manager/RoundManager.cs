using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public ConcreteSubject round_Player = new ConcreteSubject();
    public ConcreteSubject cardSubject = new ConcreteSubject();
    [HideInInspector]
    public int roundCount;

    [HideInInspector]
    public int switchPlayer;
    [HideInInspector]
    public PlayerCtrl now_Player,other_Player;
    [HideInInspector]
    public PlayerCtrl player1, player2;

    public Material m_randomMatrtial;
    public Material[] m_sceneIn, m_sceneOut, m_block1, m_block2;
    public MeshRenderer floor1, floor2;

    public int EndRound;

    #region 更換顏色
    public List<MeshRenderer> blocks = new List<MeshRenderer>();
    public List<MeshRenderer> bases = new List<MeshRenderer>();
    Transform blocksObject, basesObject;
    public Material[] m_RandomMaterials;
    public Material[] m_RandomMaterials2;
    #endregion
    public enum RoundCount
    {
        countRound,
        Switch,
        Player1,
        Player2,
        GameEnd,
        Stay
    }

    public RoundCount roundTurn;

    private void Awake()
    {
        EndRound = (int)ReadGameValue.Instance.GetValue(54);
        roundTurn = RoundCount.countRound;
        switchPlayer = 0;
        roundCount = 0;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GetScene();
        for (int i =0; i < players.Length;i++)
        {
            if (players[i].name == "Player1")
            {
                player1 = players[i].GetComponent<PlayerCtrl>();
            }
            if (players[i].name == "Player2")
            {
                player2 = players[i].GetComponent<PlayerCtrl>();
            }
        }
    }

    private void Update()
    {
        RoundCounting();
    }


    private void RoundCounting()
    {
        switch (roundTurn)
        {
            case RoundCount.countRound: //回合計數
                {
                    round_Player.setState(0);
                    roundCount += 1;
                    roundTurn = roundCount <= EndRound ? RoundCount.Switch : RoundCount.GameEnd;
                    break;
                }
            case RoundCount.Switch: //玩家輪轉
                {
                    switchPlayer = switchPlayer + 1 > 2 ? 1 : switchPlayer + 1;
                    roundTurn = switchPlayer > 1 ? RoundCount.Player2 : RoundCount.Player1;
                    round_Player.setState(switchPlayer);
                    SetColor();
                    break;
                }
            case RoundCount.Player1:
                {
                    Debug.Log("Turn: player1");
                    now_Player = player1;
                    other_Player = player2;
                    break;
                }
            case RoundCount.Player2:
                {
                    Debug.Log("Turn: player2");
                    now_Player = player2;
                    other_Player = player1;
                    break;
                }
            case RoundCount.GameEnd:
                {
                    Debug.Log("GameEnd");
                    if (roundCount != EndRound)
                    {
                        round_Player.setState(3);
                    }
                    else //指定回合結束
                    {
                        GameSettlement();
                    }
                    break;
                }
            case RoundCount.Stay:
                {
                    Debug.Log("now_Player" + now_Player);
                    Debug.Log("other_Player" + other_Player);
                    break;
                }
        }
    }
    private void GetScene()
    {
        blocksObject = transform.Find("Blocks").transform;
        basesObject = transform.Find("Base").transform;
        foreach (Transform block in blocksObject)
        {
            if (block.name.Contains("corner"))
            {
                blocks.Add(block.GetComponent<MeshRenderer>());
            }
        }
        foreach (Transform _base in basesObject)
        {
            if (_base.name.Contains("Base"))
            {
                bases.Add(_base.GetComponent<MeshRenderer>());
            }
        }
    }
    public void SetCurrentState()
    {
        if(round_Player.GetState() == 1)
        {
            roundTurn = RoundCount.Player1;
        }
        if (round_Player.GetState() == 2)
        {
            roundTurn = RoundCount.Player2;
        }
    }
    private void SetSceneColor(Material[] m_Block, Material m_Base)
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].materials = m_Block;
            bases[i].material = m_Base;
        }
    }
    public void SetColor()
    {
        if (switchPlayer == 1)
        {
            m_RandomMaterials[0].SetColor("_EmissionColor", m_RandomMaterials[1].GetColor("_EmissionColor"));
            m_RandomMaterials2[0].SetColor("_EmissionColor", m_RandomMaterials2[1].GetColor("_EmissionColor"));
            floor1.material = m_sceneIn[0];
            floor2.material = m_sceneIn[0];
            SetSceneColor(m_block1, m_sceneOut[0]);
            return;
        }
        if (switchPlayer == 2)
        {
            m_RandomMaterials[0].SetColor("_EmissionColor", m_RandomMaterials[2].GetColor("_EmissionColor"));
            m_RandomMaterials2[0].SetColor("_EmissionColor", m_RandomMaterials2[2].GetColor("_EmissionColor"));
            floor1.material = m_sceneIn[1];
            floor2.material = m_sceneIn[1];
            SetSceneColor(m_block2, m_sceneOut[1]);
            return;
        }
    }

    private void GameSettlement()
    {
        int player1Money = player1.PlayerInfo.Assets + player1.PlayerInfo.TotalAssets;
        int player2Money = player2.PlayerInfo.Assets + player2.PlayerInfo.TotalAssets;

        if (player1Money > player2Money)
        {
            player1.playerState = PlayerCtrl.PlayerState.Win;
            player2.playerState = PlayerCtrl.PlayerState.Lose;
            return;
        }
        if (player1Money < player2Money)
        {
            player1.playerState = PlayerCtrl.PlayerState.Lose;
            player2.playerState = PlayerCtrl.PlayerState.Win;
            return;
        }
    }
}