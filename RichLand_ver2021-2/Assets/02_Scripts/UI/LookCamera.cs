using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    private bool isCharacter;
    private void Awake()
    {
        SetAngle();
    }
    private void OnEnable()
    {
        SetAngle();
    }
    private void FixedUpdate()
    {
        if (!isCharacter) return;
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
    private void SetAngle()
    {
        isCharacter = transform.name.Contains("Character") ? true : false;
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
