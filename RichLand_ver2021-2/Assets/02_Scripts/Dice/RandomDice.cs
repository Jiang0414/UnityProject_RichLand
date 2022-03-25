using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomDice : MonoBehaviour
{   
    private Rigidbody diceRigid;
    private GameObject[] counts = new GameObject[6];
    [HideInInspector]
    public Vector3 inPos;
    [HideInInspector]
    public int number;

    private int turnAmtRange = 500;
    private int x = 300;
    private int y = 800;       //系統重力原數值: -9.81(Project Setting)
    private int z = 400;
    private int defaultValue = 300;
    private float turnAmt;

    private float turnAmtSpeed;
    public bool throwed, frozen;
    private Vector3 oldPos;

    public GameObject fx_hit;
    private AudioSource audioSource;


    private void Awake()
    {
        turnAmtSpeed = 2f;
        throwed = false;
        frozen = false;
        inPos = transform.position;
        diceRigid = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < counts.Length; i++)
        {
            counts[i] = transform.GetChild(i).gameObject;
        }
    }
    private void FixedUpdate()
    {
        CheckMove();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="floor")
        {
            Instantiate(fx_hit, new Vector3(transform.position.x, -0.5f, transform.position.z), Quaternion.identity);
            audioSource.Play();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            audioSource.Pause();
        }
    }

    public void ThrowTheDice()
    {
        if (throwed) return;
        throwed = true;
        frozen = false;
        number = 0;
        transform.position = inPos;
        oldPos = transform.position;
        turnAmt = (UnityEngine.Random.RandomRange(-turnAmtRange, turnAmtRange) + turnAmtSpeed) * turnAmtSpeed; //先加再乘是防止隨機為0
        diceRigid.AddForce(UnityEngine.Random.RandomRange(defaultValue, x), y, UnityEngine.Random.RandomRange(-defaultValue, -z));
        diceRigid.AddTorque(new Vector3(turnAmt, turnAmt, turnAmt), ForceMode.VelocityChange);
    }
    private void CheckMove()
    {
        if (!throwed) return;
        if (transform.position.y > 1.8f)
        {
            TorqueDice();
        }
        if (oldPos != transform.position || inPos == transform.position)
        {
            oldPos = transform.position;
            frozen = false;
        }
        else
        {
            if(number != null && number != 0)
            {
                frozen = true;
            }
            else 
            {
                throwed = false;
                frozen = false;
                ThrowTheDice();
            }
        }
    }
    private void TorqueDice()
    {
        diceRigid.AddTorque(new Vector3(turnAmt, turnAmt, turnAmt), ForceMode.Acceleration);
    }
}
