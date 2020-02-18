using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{
    public static Points instance = null;

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

    private int _score;
    [SerializeField]    private Text _pointsText;

    void Start()
    {
        _pointsText.text = "0";        
    }

    void Update()
    {
        _pointsText.text = _score.ToString();
    }

    public void AddScore()
    {
        _score++;
    }
}
