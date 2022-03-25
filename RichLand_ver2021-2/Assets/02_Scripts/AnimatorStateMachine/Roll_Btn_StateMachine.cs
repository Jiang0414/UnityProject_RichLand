using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll_Btn_StateMachine : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("out");
        animator.gameObject.SetActive(false);
    }

}
