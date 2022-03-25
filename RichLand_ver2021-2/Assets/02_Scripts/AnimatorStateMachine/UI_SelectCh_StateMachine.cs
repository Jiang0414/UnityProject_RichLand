using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SelectCh_StateMachine : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SetActive(false);
    }
}
