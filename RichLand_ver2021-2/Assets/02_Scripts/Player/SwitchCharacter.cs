using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCharacter : MonoBehaviour
{
    private PlayerCtrl player;
    public List<Animator> animators = new List<Animator>();
    private Image img_Head;
    public List<Sprite> heads;

    private void Awake()
    {
        player = GetComponent<PlayerCtrl>();
        img_Head = GameObject.FindGameObjectWithTag("img_Head" + player.playerID).GetComponent<Image>();
        foreach (Transform ani in transform)
        {
            if(ani.name.Contains("Character"))
                animators.Add(ani.GetComponent<Animator>());
        }
    }
    private void Start()
    {
        setCharacter();
    }

    private void Update()
    {
        if (player.playerID == 1)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Change(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Change(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Change(2);
            }
        }
        if (player.playerID == 2)
        {
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Change(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Change(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                Change(2);
            }
        }
    }

    public void setCharacter()
    {
        if (player.playerID != 1 || UserInfo.characterName == null) return;
        for(int i = 0; i < animators.Count; i++)
        {
            if (animators[i].gameObject.name == UserInfo.characterName)
            {
                Change(i);
                break;
            }
        }
    }

    private void Change(int i)
    {
        for (int j = 0; j < animators.Count; j++)
        {
            animators[j].gameObject.SetActive(j == i);
        }
        player.animator = animators[i];
        img_Head.sprite = heads[i];
    }
}
