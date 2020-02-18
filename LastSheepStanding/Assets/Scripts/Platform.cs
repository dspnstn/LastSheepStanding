using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Vector3 _target;
    public bool _ok, _canMove, _switching;
    private Vector3 _oldScale, _startPos;
    private float _speed = 1.0f;
    private int _times;
    private MeshCollider _meshColl;

    void Start()
    {
        _startPos = this.transform.position;
        _meshColl = GetComponent<MeshCollider>();
        _target = new Vector3(this.transform.position.x, 3.5f, this.transform.position.z);
        StartCoroutine(BeforeMovingRoutine());
    }

    void FixedUpdate()
    {
        if (_canMove && !_switching)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, _target, _speed * Time.deltaTime);
            _meshColl.enabled = false;
        }
        else if (_switching)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, _startPos, _speed * Time.deltaTime);
        }

        if (this.transform.position.y == 3.5f)
        {
            this.transform.position = _target;
            StartCoroutine(SwitchRoutine());
        }
        else if (this.transform.position.y == 0.1f && _ok)
        {
            this.transform.position = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
            Deparenting();
            _switching = false;
            _canMove = false;
            OnBack();
        }
    }

    private void Deparenting()
    {
        if (this.transform.childCount > 0)
        {
            Debug.Log(this.transform.childCount);

            Transform[] children = new Transform[this.transform.childCount];
            for (int i = 0; i < this.transform.childCount; i++)
            {
                children[i] = this.transform.GetChild(i);
            }

            foreach (Transform child in children)
            {
                if (child.tag == "Sheep")
                {
                    child.GetComponent<Sheeps>().ResumeSpeedAndAgent();
                }
                child.parent = null;
                Debug.Log("Rid of children");
            }
        }
    }

    IEnumerator BeforeMovingRoutine()
    {
        yield return new WaitForSeconds(8.0f);
        _canMove = true;
    }

    IEnumerator SwitchRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        _switching = true;
        _ok = true;
    }

    private void OnBack()
    {
        _ok = false;
        _oldScale = this.transform.localScale;
        ActivateManager.instance.OnPlatformDeath(_oldScale);
        Destroy(this.gameObject);
    }

    public void Scale(Vector3 scale)
    {
        _oldScale = scale;
        Debug.Log(_oldScale); 
        Vector3 newScale = new Vector3(_oldScale.x - 0.05f, _oldScale.y, _oldScale.z - 0.05f);
        this.transform.localScale = newScale;
        _oldScale = this.transform.localScale;
        Debug.Log(_oldScale);
    }
}

