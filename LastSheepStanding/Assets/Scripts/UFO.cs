using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    public static UFO instance = null;

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

    private GameObject _prey;
    private Vector3 _startPos,_target;
    private float _speed = 5.0f;
    private bool _canMove, _switch, _capture, _one;
    [SerializeField]    private GameObject _rayLight;

    void Start()
    {
        _capture = true;
        _startPos = this.transform.position;

        List<GameObject> Sheeps = new List<GameObject>();
        Sheeps.AddRange(GameObject.FindGameObjectsWithTag("Sheep"));              
        
        if (Sheeps.Count > 0)
        {
            GameObject randomSheep = Sheeps[UnityEngine.Random.Range(0, Sheeps.Count)];
            _prey = randomSheep;
            _target = _prey.transform.position + new Vector3(0f, this.transform.position.y, 0f);

            if (Sheeps.Count == 1)
            {
                _one = true;                
            }
        }
        Debug.Log(_target);

        StartCoroutine(SheepnappingRoutine());
    }

    IEnumerator SheepnappingRoutine()
    {
        _canMove = true;
        yield return new WaitForSeconds(10.0f);
        _switch = true;        
    }

    void FixedUpdate()
    {
        if (_canMove && !_switch)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, _target, _speed * Time.deltaTime);
        }
        else if (_switch)
        {
            Debug.Log(_switch);
            transform.position = Vector3.MoveTowards(this.transform.position, _startPos, _speed * Time.deltaTime);
        }

        if (this.transform.position == _target && _capture)
        {
            _rayLight.SetActive(true);
            _prey.transform.parent = this.gameObject.transform;
            _prey.gameObject.GetComponent<Sheeps>().Submission();
            StartCoroutine(SubmissionRoutine());
            this.transform.position += new Vector3(0.1f, 0f, 0f);
            _capture = false;                  
        }
        else if (this.transform.position == _startPos && _switch)
        {
            if (_one)
            {
                ActivateManager.instance.Finale();
                SpawnPomidorka.instance.Finale();
                Scene_Manager.instance.OnVictory();
            }

            Destroy(this.gameObject);
        }
    }

    IEnumerator SubmissionRoutine()
    {
        Vector3 center = _rayLight.gameObject.GetComponent<MeshRenderer>().bounds.center;
        _prey.transform.position = new Vector3(center.x, 0f, center.z);
        yield return new WaitForSeconds(1.0f);
        _prey.transform.rotation = Quaternion.Euler(180.0f, 0f, 0f);
        _prey.transform.position = new Vector3(_prey.transform.position.x, 0.5f, _prey.transform.position.z);
        yield return new WaitForSeconds(2.0f);
        _prey.transform.position = new Vector3(_prey.transform.position.x, 2.5f, _prey.transform.position.z);
    }
}
