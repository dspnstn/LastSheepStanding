using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCloud : MonoBehaviour
{
    private float _speed = 1.75f;
    private bool _ok = true;
    
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime, Space.World);

        if (this.transform.position.y >= 12.0f && _ok)
        {
            Scene_Manager.instance.Reload();
            Destroy(this.gameObject, 0.1f);
            _ok = false;
        }
    }
}