using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageCtrl : MonoBehaviour
{
    public List<GameObject> Pages = new List<GameObject>();
    public GameObject Mask, Mask2;
    GameObject _page;
    bool isopen;

    public void SwitchToPage(string pageName)
    {
        foreach (var i in Pages)
        {
            if (i.name == pageName)
            {                
                i.SetActive(true);
            }
            else
            {
                i.SetActive(false);
            }
        }
    }

    public void closeAll()
    {
        foreach (var i in Pages)
        {
            i.SetActive(false);
        }
    }
    public void Close_Page(string pageName)
    {
        foreach (var i in Pages)
        {
            if (i.name == pageName)
            {
                i.SetActive(false);
                break;
            }
        }
    }
    public void Open_Page(string pageName)
    {
        foreach (var i in Pages)
        {
            if (i.name == pageName)
            {
                i.SetActive(true);
                break;
            }
        }
    }

    public bool IsOpen(string pageName)
    {
        foreach (var i in Pages)
        {
            if (i.name.Contains(pageName))
            {
                isopen = i.activeInHierarchy;
                break;
            }
        }
        return isopen;
    }
    public GameObject GetPage(string pageName)
    {
        foreach (var i in Pages)
        {
            if (i.name.Contains(pageName))
            {
                _page = i;
                break;
            }
        }
        return _page;
    }
}
