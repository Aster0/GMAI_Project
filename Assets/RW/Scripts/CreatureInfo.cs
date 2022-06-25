using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class CreatureInfo : MonoBehaviour // to store various information about the creature NPC.
{

    public bool isTamed, monsterNearby;

    public GameObject owner;

}

// custom shared value so we can share this CreatureInfo class in the behavior designer tree.
[System.Serializable]
public class SharedCreatureInfo : SharedVariable<CreatureInfo>
{
    public override string ToString() { return mValue == null ? "null" : mValue.ToString(); }
    public static implicit operator SharedCreatureInfo(CreatureInfo value) { return new SharedCreatureInfo { mValue = value }; }
}
