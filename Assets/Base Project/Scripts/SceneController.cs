#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

    public static SceneController Instance = null;

    public bool IsGameRunning
    {
        get { return Instance.gameHasStarted && !Instance.gameIsPaused && !Instance.gameIsOver; }
    }

    public bool IsGameOver
    {
        get { return Instance.gameIsOver; }
    }

    [Header("UI References")]
    public Text scoreText;
    public Button pauseButton;
    public GameObject startPanel;
    public GameObject pausePanel;
    public GameObject gameoverPanel;

    [Header("Player Reference")]
    public BirdController bird;

    [Header("Scrolling Property")]
    public float scrollSpeed;

    private int playerScore = 0;
    private bool gameHasStarted;
    private bool gameIsOver;
    private bool gameIsPaused;


    // Use this for initialization
    void Awake()
    {
        gameHasStarted = false;
        gameIsOver = false;
        gameIsPaused = false;

        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        pauseButton.gameObject.SetActive(false);

        playerScore = 0;
        scoreText.gameObject.SetActive(true);
        scoreText.text = playerScore.ToString();

        startPanel.SetActive(true);
        pausePanel.SetActive(false);
        gameoverPanel.SetActive(false);

        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }

        if (IsGameRunning)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (!gameHasStarted)
            {
                StartGame();
            }
            else if (gameIsPaused)
            {
                ResumeGame();
            }
        }
    }

    public void RestartGame()
    {
        if (gameIsOver)
        {
            #if UNITY_5_3_OR_NEWER
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            #else
                Application.LoadLevel(Application.loadedLevel);
            #endif
        }
    }

    void StartGame()
    {
        Time.timeScale = 1f;
        gameHasStarted = true;
        pauseButton.gameObject.SetActive(true);
        startPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameoverPanel.SetActive(false);
        bird.StartFlapping();
    }

    public void IncrementScore()
    {
        if (!IsGameRunning)
        {
            return;
        }

        playerScore += 1;
        scoreText.text = playerScore.ToString();        
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        gameIsPaused = true;
        pausePanel.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        pauseButton.gameObject.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void GameOver()
    {
        startPanel.SetActive(false);
        pausePanel.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        gameoverPanel.SetActive(true);
        gameIsOver = true;
    }
}
