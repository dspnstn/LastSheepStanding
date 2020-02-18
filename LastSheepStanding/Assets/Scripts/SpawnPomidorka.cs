using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPomidorka : MonoBehaviour
{
    public static SpawnPomidorka instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    [SerializeField] private GameObject _pomidorka;
    public Transform[] spawnPoints;
    private bool _toSpawn;

    void Start()
    {
        StartCoroutine(WaitABitRoutine());
    }

    void Update()
    {
        if(_toSpawn)
        {
            StartCoroutine(SpawnRoutine());
            _toSpawn = false;
        }
        else if(!_toSpawn)
        {
            StopCoroutine(SpawnRoutine());
        }
    }

    public void ToSpawn(bool ok)
    {
        if (!ok)
        { 
            _toSpawn = false; 
        }
        else if (ok) 
        { 
            _toSpawn = true; 
        }
    }

    private void Spawn()
    {
        int amount = Mathf.RoundToInt(Random.Range(2.0f, spawnPoints.Length));
        for (int i = 0; i < amount; i++)
        {
            GameObject spawn = Instantiate(_pomidorka, spawnPoints[i].position, Quaternion.identity) as GameObject;
        }        
    }

    public void Finale()
    {
        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    IEnumerator SpawnRoutine()
    {
        Spawn();
        yield return new WaitForSeconds(Random.Range(9.0f, 11.0f));
        Spawn();
    }

    IEnumerator WaitABitRoutine()
    {
        yield return new WaitForSeconds(Random.Range(3.0f, 5.0f));
        _toSpawn = true;
    }
}
