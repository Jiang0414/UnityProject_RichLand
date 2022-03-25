using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarViewContent : MonoBehaviour
{
    public Scrollbar scrollbar;
    RectTransform Content;
    GridLayoutGroup grid;
    float spacing;
    int count;

    private void OnEnable()
    {
        if(Content == null)
        {
            Content = GetComponent<RectTransform>();
            grid = Content.GetComponent<GridLayoutGroup>();
        }        
        spacing = grid.spacing.y;
        float _count = (float)Content.childCount / (float)grid.constraintCount;
        count = (int)Math.Ceiling(_count);
        SetContentHigh();
    }

    public void SetContentHigh()
    {
        Content.sizeDelta = new Vector2(Content.sizeDelta.x, (count - 1) * spacing + grid.cellSize.y * count);
    }

}
