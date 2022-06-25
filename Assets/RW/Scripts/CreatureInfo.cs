using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class CreatureInfo : MonoBehaviour // to store various information about the creature NPC.
{

    public bool isTamed, monsterNearby;

    public GameObject owner;

    public float shieldCooldown;

    public void TameCreature(GameObject owner) // to tame the creature.
    {
        this.owner = owner;
        isTamed = true;
    }

}


