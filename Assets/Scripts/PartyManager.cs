using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : Singleton<PartyManager>
{
    [SerializeField]
    private GameObject[] _partyMembers;

    public void SpawnPartyMembers(Transform _spawn)
    {
        Vector3[] spawnOffsets = {Vector3.forward, Vector3.right, Vector3.left, Vector3.back};
        int i = 0;
        int offset = 1;
        foreach (GameObject partyMember in _partyMembers)
        {
            Instantiate(partyMember, _spawn.position + (offset * spawnOffsets[i]), _spawn.rotation);

            if (i + 1 > spawnOffsets.Length)
                i = 0;
            else
                i++;
        }
    }
}
