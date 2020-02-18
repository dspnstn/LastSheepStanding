using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volk : MonoBehaviour
{
    private float _speed = 12.0f;
    private bool _canMove, _switch, _capture, _toEnd;
    private GameObject _prey;
    private Vector3 _startPos, _target, _endPos;

    void Start()
    {
        _endPos = new Vector3(0f, 0f, 25.0f);
        _capture = true;

        Search();
    }

    private void Search()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player && !player.transform.parent && player.transform.position.y >= 0f)
        {
            Debug.Log("Hunt the Player");
            _prey = player;
            _target = player.transform.position + new Vector3(0.5f, 0f, 0f);
            _canMove = true;
        }
        else if (!player || player.transform.parent || player.transform.position.y < 0f)
        {
            List<GameObject> Sheeps = new List<GameObject>();
            Sheeps.AddRange(GameObject.FindGameObjectsWithTag("Sheep"));

            List<GameObject> SheepsToHunt = new List<GameObject>();

            foreach (GameObject sheep in Sheeps)
            {
                if(!sheep.transform.parent)
                {
                    SheepsToHunt.Add(sheep);
                }
            }

            if (SheepsToHunt.Count > 0)
            {
                GameObject randomSheep = SheepsToHunt[UnityEngine.Random.Range(0, SheepsToHunt.Count)];
                _prey = randomSheep;
                _target = _prey.transform.position + new Vector3(0.5f, 0f, 0f);
                _canMove = true;

                if (Sheeps.Count == 1 && SheepsToHunt.Count == 1)
                {
                    _toEnd = true;
                }
            }
            else if (SheepsToHunt.Count == 0)
            {
                StartCoroutine(NoOneToCatchRoutine());
            }
        }
    }

    void FixedUpdate()
    {
        if (_canMove && !_switch)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, _target, _speed * Time.deltaTime);
        }
        else if (_switch)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, _endPos, _speed * Time.deltaTime);
        }

        if (this.transform.position == _target && _capture)
        {
            _prey.transform.parent = this.gameObject.transform;
            if (_prey.tag == "Player")
            {
                _prey.gameObject.GetComponent<Player>().Submission();
            }
            else if (_prey.tag == "Sheep")
            {
                _prey.gameObject.GetComponent<Sheeps>().Submission();
            }

            StartCoroutine(AttackRoutine());
            _capture = false;
        }
        else if (this.transform.position == _endPos && _switch)
        {
            Sky.instance.SpawnCloud(_prey.tag);

            if (_toEnd)
            {
                ActivateManager.instance.Finale();
                SpawnPomidorka.instance.Finale();
                Scene_Manager.instance.OnVictory();
            }
            
            Destroy(this.gameObject);
        }
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.15f);
        _prey.transform.rotation = Quaternion.Euler(0f, 0f, 90.0f);
        _prey.transform.position = this.transform.position + new Vector3(0.47f, 1.0f, -0.09f);
        yield return new WaitForSeconds(1.0f);
        _switch = true;
    }

    IEnumerator NoOneToCatchRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        this.transform.position = new Vector3(this.transform.position.x, -2.0f, this.transform.position.z);
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }
}
