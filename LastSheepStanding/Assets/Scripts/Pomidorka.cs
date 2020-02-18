using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pomidorka : MonoBehaviour
{
    private NavMeshObstacle _obstacle;

    void Start()
    {
        _obstacle = GetComponent<NavMeshObstacle>();
        if (!_obstacle)
        {
            Debug.LogError("NavMeshObstacle on the Pomidorka is NULL.");
        }

        StartCoroutine(ObstacleRemove());
        StartCoroutine(SelfDestructionRoutine());
    }

    IEnumerator ObstacleRemove()
    {
        yield return new WaitForSeconds(1.0f);
        _obstacle.enabled = false;
    }

    IEnumerator SelfDestructionRoutine()
    {
        yield return new WaitForSeconds(Random.Range(6.0f, 7.9f));
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Points.instance.AddScore();
            Destroy(this.gameObject);
        }
        else if(other.tag == "Sheep")
        {
            Destroy(this.gameObject);
        }
    }
}
