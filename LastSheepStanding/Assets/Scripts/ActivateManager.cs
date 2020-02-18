using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateManager : MonoBehaviour
{
    public static ActivateManager instance = null;

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

    private int _spawnTimes;
    private float _radius = 8.0f;
    [SerializeField] private bool _spawned, _again; 
    private Vector3 _position, _previousScale;
    [SerializeField] private float _speed = 1.0f;
    private Player _player;
    [SerializeField] private GameObject _platformPrefab, _UFO, _volk;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (!_player)
        {
            Debug.LogError("Player on ActicvateManger is NULL.");
        }

        StartCoroutine(WaitForFirstSpawnRoutine());
    }

    private void Update()
    {
        if (_again)
        {
            _spawnTimes = 0;
            Debug.Log(_spawnTimes);
            StartCoroutine(WaitForSpawnRoutine());
            _again = false;
        }

    }

    private void SpawnPlatform()
    {
        SpawnPomidorka.instance.ToSpawn(false);
        Vector3 _spawnPoint = new Vector3(UnityEngine.Random.Range(-5.5f, 5.5f), 0.1f, UnityEngine.Random.Range(-5.5f, 5.5f));
        GameObject spawn = Instantiate(_platformPrefab, _spawnPoint, Quaternion.identity) as GameObject;
        _spawned = true;
        _player.CollideWithSheep(true);

        MessageToSheeps(_spawnPoint);

        StartCoroutine(VolkSpawnRoutine());

        if (_spawnTimes >= 1)
        {
            spawn.GetComponent<Platform>().Scale(_previousScale);
        }
    }

    IEnumerator VolkSpawnRoutine()
    {
        yield return new WaitForSeconds(12.0f);
        VolkOut();
    }

    private void SpawnUFO()
    {
        Vector3 UFOspawnPoint = new Vector3(-20.0f, 5.5f, 0f);
        GameObject UFOspawn = Instantiate(_UFO, UFOspawnPoint, Quaternion.identity) as GameObject;        
    }

    public void OnPlatformDeath(Vector3 scale)
    {
        _previousScale = scale;
        _spawnTimes++;
        _spawned = false;
        
        StartCoroutine(PomidorkaSpawnRoutine());

        if (_spawnTimes == 1 || _spawnTimes == 4 || _spawnTimes == UnityEngine.Random.Range(8, 10) || _spawnTimes == 13)
        {
            StartCoroutine(SpawnUFORoutine());
        }

        if (_spawnTimes < 14)
        {
            StartCoroutine(WaitForSpawnRoutine());
        }
        else if (_spawnTimes >= 14)
        {
            Again();
        }
    }

    private void Again()
    {
        _again = true;
    }

    private void MessageToSheeps(Vector3 spot)
    {
        List<GameObject> Sheeps = new List<GameObject>();
        Sheeps.AddRange(GameObject.FindGameObjectsWithTag("Sheep"));

        foreach (GameObject sheep in Sheeps)
        {
            sheep.GetComponent<Sheeps>().Destination(spot);
        }
    }

    private void VolkOut()
    {
        Vector3 volkSpawnPoint = new Vector3(0f, 0f, -9.2f);
        GameObject volkSpawn = Instantiate(_volk, volkSpawnPoint, Quaternion.identity) as GameObject;
    }

    public void Finale()
    {
        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    IEnumerator WaitForFirstSpawnRoutine()
    {
        yield return new WaitForSeconds(15.0f);
        SpawnPlatform();
    }

    IEnumerator WaitForSpawnRoutine()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(35.0f, 50.0f));
        SpawnPlatform();
    }

    IEnumerator SpawnUFORoutine()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(3.0f, 6.0f));
        SpawnUFO();
    }

    IEnumerator PomidorkaSpawnRoutine()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(15.0f, 30.0f));
        _player.CollideWithSheep(false);
        SpawnPomidorka.instance.ToSpawn(true);
    }
}
