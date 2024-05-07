using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBag : MonoBehaviour
{
    public int value;

    public void Initilize(int v)
    {
        this.value = v;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().money += value;
             FindObjectOfType<AudioManager>().Play("MoneyPickup");
        }
    }
}

