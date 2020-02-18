using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public static Scene_Manager instance = null;

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

#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
    }

    private Scene currentScene;
    private int index;
    [SerializeField]    private GameObject _clouds;
    private bool _paused;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        index = currentScene.buildIndex;
        Instantiate(_clouds.gameObject);
    }    

    void Update()
    {
        if (_paused)
        {
            Time.timeScale = 0;
        }
        else if (!_paused)
        {
            Time.timeScale = 1;
        }
    }

    public void Pause()
    {      
        if (Time.timeScale == 1)
        {
            _paused = true;
        }
        else if (Time.timeScale == 0)
        {
            _paused = false;
        }
    }

    public void Reload()
    {
        DontDestroyOnLoad(Points.instance.gameObject);
        SceneManager.LoadScene(index);
    }

    public void ClickIn()
    {
        SceneManager.LoadScene(1);
    }

    public void ClickOut()
    {
        DontDestroyOnLoad(Points.instance.gameObject);
        SceneManager.LoadScene(0);
    }

    public void OnVictory()
    {
        StartCoroutine(VictoryRoutine());
    }

    IEnumerator VictoryRoutine()
    {
        Victory.instance.Activate();
        yield return new WaitForSeconds(2.5f);
        Destroy(Points.instance.gameObject);
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
    Application.Quit();
#elif (UNITY_WEBGL)
    Application.OpenURL("about:blank");
#endif
    }
}
