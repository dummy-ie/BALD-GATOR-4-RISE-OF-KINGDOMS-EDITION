using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum EJobClass
{
    // One class that masters each main stat and one secondary
    ARTIFICER, // int / con ranged
    BARBARIAN, // con / str melee
    MONK, // wis / dex melee
    PALADIN, // str / cha melee
    ROGUE, // dex / int ranged
    WARLOCK, // cha / wis ranged
}
