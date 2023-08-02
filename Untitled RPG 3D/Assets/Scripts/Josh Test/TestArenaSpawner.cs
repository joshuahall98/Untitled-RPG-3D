using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArenaSpawner : MonoBehaviour
{

    [SerializeField]GameObject Wurgle;
    [SerializeField]Vector3 spawnLocation;

    public void SpawnAI()
    {
        Instantiate(Wurgle, spawnLocation, Quaternion.identity);
    }

}
