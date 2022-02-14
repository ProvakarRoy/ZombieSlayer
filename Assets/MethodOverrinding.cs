using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodOverrinding : MonoBehaviour
{
    Provakar p = new Provakar();
    public void Start()
    {
        p.ShowName();
    }
}


public class Sauvik
{
   public int val1 = 2;
    public virtual void ShowName()
    {
        val1 = 3;
        Debug.Log("my name is Sauvik");
    }
}

public class Provakar : Sauvik
{
    public int val2 = 3;
    public override void ShowName()
    {
       Debug.Log( val1 + val2);
       base.ShowName();
    }
}