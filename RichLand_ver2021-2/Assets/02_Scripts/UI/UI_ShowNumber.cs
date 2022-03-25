using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShowNumber : MonoBehaviour
{
    public GameObject ui_NumberView;
    private Animator animator;
    private GameObject digit, double_digit;
    private Image digit_Sprite, double_Sprite1, double_Sprite2;
    public List<Sprite> sprites = new List<Sprite>();

    private void Start()
    {
        animator = ui_NumberView.GetComponent<Animator>();
        digit = ui_NumberView.transform.Find("digit").gameObject;
        double_digit = ui_NumberView.transform.Find("double_digit").gameObject;
        digit_Sprite = digit.GetComponent<Image>();
        double_Sprite1 = double_digit.transform.Find("double_digit").GetComponent<Image>();
        double_Sprite2= double_digit.transform.Find("digit").GetComponent<Image>();
    }

    public void OpenDiceNumberView(int number)
    {
        StartCoroutine(NumberView(number.ToString(), number));
    }

    IEnumerator NumberView(string number, int intNumber)
    {
        animator.SetBool("open", true);
        digit.SetActive(intNumber < 10 ? true : false);
        double_digit.SetActive(intNumber < 10 ? false : true);
        digit_Sprite.sprite = digit.activeSelf ? sprites[Int32.Parse(number)] : null;
        double_Sprite1.sprite = double_digit.activeSelf ? sprites[Int32.Parse(number[0].ToString())] : null;
        double_Sprite2.sprite = double_digit.activeSelf ? sprites[Int32.Parse(number[1].ToString())] : null;
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("open", false);
    }
}
