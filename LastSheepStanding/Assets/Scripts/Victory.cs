using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    public static Victory instance = null;

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

    [SerializeField] private GameObject _victory;

    public void Activate()
    {
        StartCoroutine(VictoryRoutine());
    }

    IEnumerator VictoryRoutine()
    {
        _victory.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        _victory.SetActive(false);
    }

}
