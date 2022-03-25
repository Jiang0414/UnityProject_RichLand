using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarContentV2 : MonoBehaviour
{
    public Scrollbar scrollbar;
    RectTransform Content;
    GridLayoutGroup grid;
    float spacing;
    int count;

    private void OnEnable()
    {
        if (Content == null)
        {
            Content = GetComponent<RectTransform>();
            grid = Content.GetComponent<GridLayoutGroup>();
        }
        spacing = grid.spacing.x;
        float _count = (float)Content.childCount / (float)grid.constraintCount;
        count = (int)Math.Ceiling(_count);
        SetContentHigh();
    }

    public void SetContentHigh()
    {
        Content.sizeDelta = new Vector2((count - 1) * spacing + grid.cellSize.x * count
            + grid.padding.right + grid.padding.left, Content.sizeDelta.y);
        Content.transform.position = new Vector3(0, Content.transform.position.y, 0);
    }

}
