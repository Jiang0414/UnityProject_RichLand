using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�[��̤���
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

//�D�D����
public abstract class Subject
{
    List<Obsever> m_obsevers = new List<Obsever>();

    //�[�J�[���
    public void Attach(Obsever theObsever)
    {
        m_obsevers.Add(theObsever);
    }

    //�����[���
    public void Detach(Obsever theObsever)
    {
        m_obsevers.Remove(theObsever);
    }

    //�D�D���A
    public int subjectState { get; set; }

    //�q���Ҧ��[���
    public void Notify()
    {
        foreach (Obsever theObserver in m_obsevers)
        {
            theObserver.Update();
        }
    }
}

//�[��̥D�D
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

//�[��̹�@
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

    //�q����s
    public override void Update()
    {
        //Debug.Log("�ڬO" + m_name + "�A���쪱�a" + m_state);
        m_state = m_Subject.GetState();
    }
}