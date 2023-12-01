using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : Singleton<PartyManager>
{
    [SerializeField]
    private GameObject[] _partyMembers;
    public GameObject[] PartyMembers
    {
        get { return _partyMembers; }
    }

    public void SpawnPartyMembers(Transform _spawn)
    {
        foreach (GameObject partyMember in _partyMembers)
        {
            Instantiate(partyMember, _spawn.position, _spawn.rotation);
        }
    }
}
