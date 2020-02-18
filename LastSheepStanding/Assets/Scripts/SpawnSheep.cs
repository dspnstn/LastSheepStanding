using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSheep : MonoBehaviour
{
    [SerializeField]    private GameObject _sheepPrefab;
    public Transform[] spawnPoints;

    void Start()
    {
        foreach (Transform _spawnPoint in spawnPoints)
        {
            float directionToFace = Random.Range(0f, 360.0f);
            Instantiate(_sheepPrefab, _spawnPoint.position, Quaternion.Euler(new Vector3(0f, directionToFace, 0f)));
        }        
    }
}
