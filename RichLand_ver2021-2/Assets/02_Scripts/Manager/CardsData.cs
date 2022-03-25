using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsData 
{
    protected int _CardID;
    protected int _ContinuedRound;
    protected float _EffectValue;
    protected bool _IsDirect;//能否直接使用
    protected bool _CanAntim;//能否反彈 Antimissile
    protected bool _CanRobbery;

    protected int _GetRound;
    protected int _EndRound;
    protected ConcreteObserver_1 _CardObserver;
}
public class CardInfo : CardsData
{
    public CardInfo(int cardID, int continueRound, float effectValue, bool isDirect, bool canAntim, bool canRobbery, int getRound, int endRound, ConcreteObserver_1 observer)
    {
        this._CardID = cardID;
        this._ContinuedRound = continueRound;
        this._EffectValue = effectValue;
        this._IsDirect = isDirect;
        this._CanAntim = canAntim;
        this._CanRobbery = canRobbery;
        this._GetRound = getRound;
        this._EndRound = endRound;
        this._CardObserver = observer;
    }
    public int CardID
    {
        get { return this._CardID; }
        set { this._CardID = value; }
    }
    public int ContinuedRound
    {
        get { return this._ContinuedRound; }
        set { this._ContinuedRound = value; }
    }
    public float EffectValue
    {
        get { return this._EffectValue; }
        set { this._EffectValue = value; }
    }
    public bool IsDirect
    {
        get { return this._IsDirect; }
        set { this._IsDirect = value; }
    }
    public bool CanAntim
    {
        get { return this._CanAntim; }
        set { this._CanAntim = value; }
    }
    public bool CanRobbery
    {
        get { return this._CanRobbery; }
        set { this._CanRobbery = value; }
    }
    public int GetRound
    {
        get { return this._GetRound; }
        set { this._GetRound = value; }
    }
    public int EndRound
    {
        get { return this._EndRound; }
        set { this._EndRound = value; }
    }
    public ConcreteObserver_1 CardObserver
    {
        get { return this._CardObserver; }
        set { this._CardObserver = value; }
    }
}