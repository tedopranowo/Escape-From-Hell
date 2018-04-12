using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneManagerSingleton : MonoBehaviour
{
    private static SceneManagerSingleton s_instance = null;

    private static string s_currentScene = null;
    private static string s_nextScene = null;
    private static string s_pastScene = null;

    private static int s_nextSceneIndex = 0;

    public bool hasPlayer = false;

    [SerializeField]
    private GameObject m_player = null;

    [SerializeField]
    private string m_mainMenuScene = null;

    [SerializeField]
    private string[] m_scenes;

    [SerializeField]
    private float m_endGameWaitTime = 5.0f;

    [SerializeField]
    private GameObject m_gameplayMenuCanvas = null;

    public GameObject player
    {
        get { return m_player; }
        set { m_player = value; }
    }


    public string nextScene
    {   
        get { return s_nextScene; }
    }

    public string currentScene
    {
        get { return s_currentScene; }
    }

    public string pastScene
    {
        get { return s_pastScene; }
    }

    void Awake()
    {
        s_instance = this;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        //Open the menu if escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape) && s_currentScene != "Main Menu")
        {
            m_gameplayMenuCanvas.SetActive(!m_gameplayMenuCanvas.activeSelf);
        }
    }

    public static SceneManagerSingleton instance
    {
        set
        {
            if (s_instance != null)
            {
                Destroy(value);
                return;
            }

            s_instance = value;
        }
        get
        {
            if (!s_instance)
            {
                new GameObject("SceneManagerSingleton", typeof(SceneManagerSingleton));
            }
            return s_instance;
        }
    }

    /// <summary>
    /// Sets previous scene
    /// Loads scene
    /// SetsCurrentScene to scene
    /// </summary>
    /// <param name="scene"></param>
    public void LoadNextLevel()
    {
        Debug.Log(s_nextSceneIndex);
        SetPreviousScene();

        if (hasPlayer)
        {
            DontDestroyOnLoad(m_player.gameObject);
        }

        if (s_nextSceneIndex > m_scenes.Length - 1)
        {
            ShowGameWin();
        }

        SceneManager.LoadScene(m_scenes[s_nextSceneIndex]);
        SetCurrentScene(m_scenes[s_nextSceneIndex]);
        ++s_nextSceneIndex;

        if (s_nextSceneIndex > m_scenes.Length - 1)
        {
            //s_nextSceneIndex = 0;

            //[Alberto]
            // Why is this part down here destroying the players children already?


            //quick and dirty
            for(int i = 0; i < m_player.transform.childCount; i++)
            {
                Destroy(m_player.transform.GetChild(i));
            }
        }
    }

    /// <summary>
    /// Sets s_pastScene to s_currentScene
    /// </summary>
    private void SetPreviousScene()
    {
        Debug.Log("Setting previous scene to " + s_currentScene);
        s_pastScene = s_currentScene;
    }

    /// <summary>
    /// Sets s_currentScene to scene
    /// </summary>
    /// <param name="scene"></param>
    private void SetCurrentScene(string scene)
    {
        Debug.Log("Setting current scene to " + scene);
        s_currentScene = scene;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(m_mainMenuScene);
        DestroyEverything();
        SetCurrentScene(m_mainMenuScene);
        s_nextSceneIndex = 0;
    }

    public void LoadGameOver()
    {
        StartCoroutine(GameOverSequence());
    }

    private void ShowGameWin()
    {
        StartCoroutine(WinGameSequence());
    }

    private IEnumerator GameOverSequence()
    {

        GameObject canvas = gameObject.transform.GetChild(0).gameObject;
        canvas.SetActive(true);

        yield return new WaitForSeconds(m_endGameWaitTime);
        DestroyEverything();
        LoadMenu();
    }

    private IEnumerator WinGameSequence()
    {
        GameObject canvas = gameObject.transform.GetChild(1).gameObject;
        canvas.SetActive(true);

        yield return new WaitForSeconds(m_endGameWaitTime);
        DestroyEverything();
        LoadMenu();
    }

    private void DestroyEverything()
    {
        GameObject[] GameObjects = (FindObjectsOfType<GameObject>() as GameObject[]);
        for (int i = 0; i < GameObjects.Length; ++i)
        {
            Destroy(GameObjects[i]);
        }
        Destroy(gameObject);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
