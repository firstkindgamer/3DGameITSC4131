using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTower : Tower
{
    public static FreezeTower fTower;
    public void Awake()
    {
        fTower = this;
    }
}
