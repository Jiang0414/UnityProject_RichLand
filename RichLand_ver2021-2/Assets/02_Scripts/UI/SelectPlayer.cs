using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayer : MonoBehaviour
{
    private Animator animator;
    [HideInInspector]
    public UIManager uIManager;
    [HideInInspector]
    public RoundManager roundManager;
    [HideInInspector]
    public CardEffect card;
    public Image img_player1, img_player2, head1, head2;
    [HideInInspector]
    public bool canLaunch;

    private void OnEnable()
    {
        img_player1.sprite = head1.sprite;
        img_player2.sprite = head2.sprite;
    }

    private void Start()
    {
        uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        roundManager = GameObject.FindGameObjectWithTag("RoadManager").GetComponent<RoundManager>();
        animator = GetComponent<Animator>();
    }

    public void Btn_Playe1()
    {
        card.canLaunch = canLaunch;
        card.targetPlayer = roundManager.player1;
        animator.SetBool("Player1", true);
        animator.SetBool("Player2", false);
    }
    public void Btn_Playe2()
    {
        card.canLaunch = canLaunch;
        card.targetPlayer = roundManager.player2;
        animator.SetBool("Player1", false);
        animator.SetBool("Player2", true);
    }
}
