using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundData
{
    protected int _ID;
    protected string _Name;
    protected int _Area;
    protected int _Owner;
    protected int _HouseLv;
    protected int _Tolls;
    protected int _GroundValue;
    protected int _Build0;
    protected int _Build1;
    protected int _Build2;
    protected int _Build3;
    protected int _Build4;
}

public class GroundInfo : GroundData
{
    public GroundInfo(int id,string name, int area, int owner, int houseLv,int tolls, int value,int bulid0, int bulid1, int bulid2, int bulid3, int bulid4)
    {
        this._ID = id;
        this._Name = name;
        this._Area = area;
        this._Owner = owner;
        this._HouseLv = houseLv;
        this._Tolls = tolls;
        this._GroundValue = value;
        this._Build0 = bulid0;
        this._Build1 = bulid1;
        this._Build2 = bulid2;
        this._Build3 = bulid3;
        this._Build4 = bulid4;
    }

    public int ID
    {
        get { return this._ID; }
        set { this._ID = value; }
    }
    public string Name
    {
        get { return this._Name; }
        set { this._Name = value; }
    }

    public int Owner
    {
        get { return this._Owner; }
        set { this._Owner = value; }
    }
    public int Tolls
    {
        get { return this._Tolls; }
        set { this._Tolls = value; }
    }
    public int GroundValue
    {
        get { return this._GroundValue; }
        set { this._GroundValue = value; }
    }
    public int Area
    {
        get { return this._Area; }
        set { this._Area = value; }
    }
    public int HouseLv
    {
        get { return this._HouseLv; }
        set { this._HouseLv = value; }
    }
    public int Build0
    {
        get { return this._Build0; }
        set { this._Build0 = value; }
    }
    public int Build1
    {
        get { return this._Build1; }
        set { this._Build1 = value; }
    }
    public int Build2
    {
        get { return this._Build2; }
        set { this._Build2 = value; }
    }
    public int Build3
    {
        get { return this._Build3; }
        set { this._Build3 = value; }
    }
    public int Build4
    {
        get { return this._Build4; }
        set { this._Build4 = value; }
    }
}
