using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayer_StateMachine : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SelectPlayer selectPlayer = animator.GetComponent<SelectPlayer>();
        selectPlayer.uIManager.ui_BuyGround.SetActive(false);
        animator.gameObject.SetActive(false);
    }
}
