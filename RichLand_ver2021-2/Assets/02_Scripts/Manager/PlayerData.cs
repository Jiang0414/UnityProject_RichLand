using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    protected int _ID;
    protected int _Assets;
    protected int _TotalAssets;
}
public class PlayerInfo : PlayerData
{
    public PlayerInfo(int id,int assets, int totalAssets)
    {
        this._ID = id;
        this._Assets = assets;
        this._TotalAssets = totalAssets;
    }

    public int ID
    {
        get { return this._ID; }
        set { this._ID = value; }
    }
    public int Assets
    {
        get { return this._Assets; }
        set { this._Assets = value; }
    }
    public int TotalAssets
    {
        get { return this._TotalAssets; }
        set { this._TotalAssets = value; }
    }
}
