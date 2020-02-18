using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class Sheeps : MonoBehaviour
{
    [SerializeField] private float _speed, _startWaitTime;
    private float _waitTime;
    private bool _platform;
    private GameObject _spots;
    private Transform[] spots;
    private int _randomSpot;
    private NavMeshAgent _agentSheep;
    private NavMeshObstacle _obstacle;
    private Vector3 _newDest;
    private Player _player;
    

    void Awake()
    {
        _spots = GameObject.FindGameObjectWithTag("Spots");
        if (!_spots)
        {
            Debug.LogError("Spots on the SheepPrefab is NULL.");
        }

        spots = new Transform[_spots.transform.childCount];
        for (int i = 0; i < _spots.transform.childCount; i++)
        {
            spots[i] = _spots.transform.GetChild(i);
        }
    }

    void Start()
    {
        _agentSheep = GetComponent<NavMeshAgent>();
        if (!_agentSheep)
        {
            Debug.LogError("NavMeshAgent on the SheepPrefab is NULL.");
        }
        _agentSheep.enabled = true;

        _obstacle = GetComponent<NavMeshObstacle>();
        if (!_obstacle)
        {
            Debug.LogError("NavMeshObstacle on the SheepPrefab is NULL.");
        }
        _obstacle.enabled = false;


        _player = GameObject.Find("Player").GetComponent<Player>();
        if (!_player)
        {
            Debug.LogError("Player on Sheeps is NULL.");
        }

        _startWaitTime = Random.Range(2, 4.5f);
        _waitTime = _startWaitTime;
        _randomSpot = Random.Range(0, spots.Length);
    }

    void FixedUpdate()
    {
        if (_platform)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, _newDest, _speed * Time.deltaTime);            
        }
        else if(!_platform)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, spots[_randomSpot].position, _speed * Time.deltaTime);

            if (Vector3.Distance(this.transform.position, spots[_randomSpot].position) < 0.2f)
            {
                if (_waitTime <= 0)
                {
                    _randomSpot = Random.Range(0, spots.Length);
                    _startWaitTime = Random.Range(2, 4.5f);
                    _waitTime = _startWaitTime;
                }
                else if (_waitTime > 0)
                {
                    _waitTime -= Time.deltaTime;
                }
            }
        }        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Platform")
        {
            List<GameObject> Sheeps = new List<GameObject>();
            Sheeps.AddRange(GameObject.FindGameObjectsWithTag("Sheep"));
            Debug.Log(Sheeps.Count);

            foreach (GameObject sheep in Sheeps)
            {
                if ((sheep.transform.position - this.transform.position).sqrMagnitude < 2)
                {
                    _agentSheep.enabled = false;
                    _obstacle.enabled = true;
                }
            }
            _speed = 0;
            this.transform.parent = other.gameObject.transform;
        }
    }    

    public void ResumeSpeedAndAgent()
    {
         StartCoroutine(ResumeSpeedRoutine());                 
    }

    IEnumerator ResumeSpeedRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        if (_obstacle.enabled)
        {
            _obstacle.enabled = false;
        }
        _agentSheep.enabled = true;
        _speed = 0.35f;
    }

    public void Destination(Vector3 dest)
    {
        StartCoroutine(WalkTowardsPlatformRoutine());
        _newDest = dest;
    }

    IEnumerator WalkTowardsPlatformRoutine()
    {
        _platform = true;
        yield return new WaitForSeconds(6.25f);
        _platform = false;
    }

    public void Submission()
    {
        _agentSheep.enabled = false;
        _speed = 0;
    }

}