using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardView_StateMachine : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Card card = animator.GetComponentInParent<Card>();

        if (card.isDirect)
        {
            card.roundManager.cardSubject.setState(card.cardID);
            card.owner.uiManager.isActDone = false;
        }
        else
        {
            card.owner.uiManager.isActDone = true;
        }
        card.transform.gameObject.SetActive(false);
    }
}