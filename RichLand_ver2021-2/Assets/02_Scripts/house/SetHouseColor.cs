using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHouseColor : MonoBehaviour
{
    public Material player1, player2;
    private MeshRenderer meshRender;
    private Ground_Info ground;
    private void Awake()
    {
        meshRender = GetComponent<MeshRenderer>();
        ground = GetComponentInParent<Ground_Info>();
    }

    private void OnEnable()
    {
        if (ground != null)
        {
            SetColor();
        }
    }

    public void SetColor()
    {
        if (ground.Info.Owner == 1)
        {
            meshRender.material = player1;
        }
        if (ground.Info.Owner == 2)
        {
            meshRender.material = player2;
        }
    }
}
