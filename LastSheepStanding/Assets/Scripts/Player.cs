using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private NavMeshAgent _agent;
    private NavMeshObstacle _obstacle;
    private Rigidbody _rg;
    private bool _collideWithSheep = true;
    private bool _paused;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (!_agent)
        {
            Debug.LogError("NavMeshAgent on the Player is NULL.");
        }
        _agent.enabled = true;

        _obstacle = GetComponent<NavMeshObstacle>();
        if (!_obstacle)
        {
            Debug.LogError("NavMeshObstacle on the Player is NULL.");
        }
        _obstacle.enabled = false;

        _rg = GetComponent<Rigidbody>();
        if (!_rg)
        {
            Debug.LogError("Rigidbody on the Player is NULL.");
        }
    }

    void Update()
    {       
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && _agent.enabled && !EventSystem.current.IsPointerOverGameObject())
            {
                _agent.SetDestination(hit.point);
            }
            else if (Physics.Raycast(ray, out hit) && !_agent.enabled && !EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Agent not ready yet");
            }
        }

        if (!this.transform.parent && !_agent.enabled && !_collideWithSheep)
        {
            _obstacle.enabled = false;
            _agent.enabled = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sheep" && _collideWithSheep)
        {
            float distance = Vector3.Distance(this.transform.position, Vector3.zero);

            if (distance > 6)
            {
                _rg.isKinematic = false;
                _agent.enabled = false;
                _rg.AddForce(-transform.forward * 45, ForceMode.Impulse);
            }            
        }

        if (other.tag == "Platform")
        {
            this.transform.parent = other.gameObject.transform;
            Vector3 center = other.gameObject.GetComponent<MeshRenderer>().bounds.center;
            this.transform.position = center + new Vector3(0f, 0.2f, 0f);
            _agent.enabled = false;
            _obstacle.enabled = true;
        }
    }

    public void CollideWithSheep(bool ok)
    {
        if (ok) 
        { 
            _collideWithSheep = false;
        }
        else if (!ok) 
        { 
            _collideWithSheep = true;
        }
    }

    public void Submission()
    {
        _agent.enabled = false;
    }
}
