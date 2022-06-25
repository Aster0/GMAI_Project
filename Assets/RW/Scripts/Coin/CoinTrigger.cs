using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Creature") || other.name.Equals("Character")) // if a creature or a character triggers it
        {
             Destroy(this.gameObject, 1); // to mimic coin collection 
        }
    }
}
