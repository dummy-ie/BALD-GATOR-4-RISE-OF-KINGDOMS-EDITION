using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : Singleton<PartyManager>
{
    [SerializeField]
    private GameObject[] _partyMemberPrefabs;
    [SerializeField]
    private List<GameObject> _partyMembers = new();
    public List<GameObject> PartyMembers
    {
        get { return _partyMembers; }
    }

    public void SpawnPartyMembers(Transform _spawn)
    {
        _partyMembers.Clear();
        Vector3[] spawnOffsets = {Vector3.forward, Vector3.right, Vector3.left, Vector3.back};
        int i = 0;
        int offset = 1;
        foreach (GameObject partyMemberPrefab in _partyMemberPrefabs)
        {
            GameObject partyMember = Instantiate(partyMemberPrefab, _spawn.position + (offset * spawnOffsets[i]), _spawn.rotation);
            _partyMembers.Add(partyMember);
            if (i + 1 > spawnOffsets.Length)
                i = 0;
            else
                i++;
        }
    }
}
