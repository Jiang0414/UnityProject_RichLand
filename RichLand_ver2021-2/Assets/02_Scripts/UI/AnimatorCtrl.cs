using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCtrl : MonoBehaviour
{
    public Animator animator;

    public void btn_SetBoolTrue(string boolName)
    {
        animator.SetBool(boolName, true);
    }
    public void btn_SetBoolFalse(string boolName)
    {
        animator.SetBool(boolName, false);
    }
}
