using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect_StateMachine : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Card card = animator.GetComponentInParent<Card>();
        card.transform.gameObject.SetActive(false);
        card.cardEffect.isAniEnd = true;
    }
}
