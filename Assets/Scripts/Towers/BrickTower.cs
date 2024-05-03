using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickTower : Tower
{
    public static BrickTower bTower;
    public void Awake()
    {
        bTower = this;
    }
}
