using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//觀察者介面
public abstract class Obsever 
{
    protected string Name;
    protected ConcreteSubject ConcreteSubject;
    public Obsever(string name, ConcreteSubject concreteSubject)
    {
        this.Name = name;
        this.ConcreteSubject = concreteSubject;
    }
    public abstract void Update();
}

//主題介面
public abstract class Subject
{
    List<Obsever> m_obsevers = new List<Obsever>();

    //加入觀察者
    public void Attach(Obsever theObsever)
    {
        m_obsevers.Add(theObsever);
    }

    //移除觀察者
    public void Detach(Obsever theObsever)
    {
        m_obsevers.Remove(theObsever);
    }

    //主題狀態
    public int subjectState { get; set; }

    //通知所有觀察者
    public void Notify()
    {
        foreach (Obsever theObserver in m_obsevers)
        {
            theObserver.Update();
        }
    }
}

//觀察者主題
public class ConcreteSubject : Subject
{
    public void setState(int state)
    {
        subjectState = state;
        Notify();
    }

    public int GetState()
    {
        return subjectState;
    }
}

//觀察者實作
public class ConcreteObserver_1 : Obsever
{
    ConcreteSubject m_Subject = null;
    public string m_name;
    public int m_state;

    public ConcreteObserver_1(string name, ConcreteSubject theSubject) : base(name, theSubject)
    {
        m_Subject = theSubject;
        m_name = name;
    }

    //通知更新
    public override void Update()
    {
        //Debug.Log("我是" + m_name + "，輪到玩家" + m_state);
        m_state = m_Subject.GetState();
    }
}