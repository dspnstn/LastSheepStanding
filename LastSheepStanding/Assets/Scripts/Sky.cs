using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    public static Sky instance = null;

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

    [SerializeField] private GameObject _playerCloud, _sheepCloud;
    private Vector3 _spawnPoint;

    void Start()
    {
        _spawnPoint = new Vector3(-6.7f, this.transform.position.y, 13.0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {            
            SpawnCloud("Player");
            Destroy(other.gameObject);
        }
    }

    public void SpawnCloud(string tag)
    {
        if (tag == "Player")
        {
            GameObject spawn = Instantiate(_playerCloud, _spawnPoint, Quaternion.Euler(0f, 60.0f, 0f)) as GameObject;
        }
        else if (tag == "Sheep")
        {
            GameObject spawn = Instantiate(_sheepCloud, _spawnPoint, Quaternion.Euler(0f, 60.0f, 0f)) as GameObject;
        }
    }
}
