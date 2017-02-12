using System;
using System.Collections.Generic;
#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class SceneController : MonoBehaviour {

    [Serializable]
    public struct Level
    {
        public int score;
        public List<TreeType> trees;
    }

    [Serializable]
    public struct Reward
    {
        public Sprite medal;
        public int score;
    }

    public static SceneController Instance = null;

    public bool IsGameRunning
    {
        get { return Instance.gameHasStarted && !Instance.gameIsPaused && !Instance.gameIsOver; }
    }

    public bool IsGameOver
    {
        get { return Instance.gameIsOver; }
    }

    [Header("North Panel References")]
    public Text scoreText;
    public Button pauseButton;

    [Header("GameOver Panel References")]
    public Text playerScoreLabel;
    public Text bestScoreLabel;
    public Image medal;

    [Header("Medal References")]
    public Reward gold;
    public Reward silver;
    public Reward bronze;

    [Header("UI References References")]
    public GameObject startPanel;
    public GameObject pausePanel;
    public GameObject gameoverPanel;
    public GameObject southPanel;

    [Header("Player Reference")]
    public BirdController bird;

    [Header("Tree Spawnner Property")]
    public TreeSpawnner treeSpawnner;
    public Level[] levels;



    private int playerScore = 0, bestScore = 0;
    private bool gameHasStarted;
    private bool gameIsOver;
    private bool gameIsPaused;


    // Use this for initialization
    void Awake()
    {
        Init();

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

        southPanel.SetActive(true);

        Time.timeScale = 0;
    }

    void Init()
    {
        bool freshInstall = GameData.LoadFreshInstall();

        if (!freshInstall)
        {
            GameData.SaveIntValue("Birdz Unlocked", 1);
            GameData.SaveFreshInstall(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }

        if (IsGameRunning && bird.IsAlive)
        {
            if (treeSpawnner.TimeSinceLastSpawn >= treeSpawnner.SpawnRate)
            {
                treeSpawnner.SpawnNewTree(GetRandomMovementBasedOnScore(playerScore));
            }

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

    TreeType GetRandomMovementBasedOnScore(int score)
    {
        for (int i = levels.Length - 1; i >= 0; i--)
        {
            if (levels[i].score <= score)
            {
                return levels[i].trees[Random.Range(0, levels[i].trees.Count)];
            }
        }

        return TreeType.Easy;
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
        southPanel.SetActive(false);

        bird.StartFlapping();
        bird.FlapBird();

        treeSpawnner.SpawnNewTree(TreeType.Easy);
    }

    public void IncrementScore()
    {
        if (!IsGameRunning)
        {
            return;
        }

        playerScore += 1;
        scoreText.text = playerScore.ToString();

        int birdzUnlocked = GameData.LoadIntValue("Birdz Unlocked");

        if (birdzUnlocked < 4)
        {
            AchievementController.Instance.BirdzAchievement.SetValue("Score25", (uint)playerScore);
            AchievementController.Instance.BirdzAchievement.SetValue("Score50", (uint)playerScore);
            AchievementController.Instance.BirdzAchievement.SetValue("Score80", (uint)playerScore);

            List<Milestone> milestones = AchievementController.Instance.BirdzAchievement.CheckMilestones();

            for (int i = 0; i < milestones.Count; i++)
            {
                if (milestones[i].Title == "Velvet" && birdzUnlocked == 1)
                {
                    GameData.SaveIntValue("Birdz Unlocked", birdzUnlocked + 1);
                    AchievementController.Instance.Fill(milestones[i]);
                    //AchievementController.Instance.Toast();
                }
                else if (milestones[i].Title == "Cool" && birdzUnlocked == 2)
                {
                    GameData.SaveIntValue("Birdz Unlocked", birdzUnlocked + 1);
                    AchievementController.Instance.Fill(milestones[i]);
                    //AchievementController.Instance.Toast();
                }
                else if (milestones[i].Title == "Funny" && birdzUnlocked == 3)
                {
                    GameData.SaveIntValue("Birdz Unlocked", birdzUnlocked + 1);
                    AchievementController.Instance.Fill(milestones[i]);
                    //AchievementController.Instance.Toast();
                }
            }
        }
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
        bestScore = GameData.LoadIntValue("Best", 0);

        if (playerScore > bestScore)
        {
            bestScore = playerScore;
            // new high score
            GameData.SaveIntValue("Best", playerScore);
        }

        medal.color = Color.clear;

        if (playerScore >= gold.score)
        {
            medal.sprite = gold.medal;
            medal.color = Color.white;
        } else if (playerScore >= silver.score)
        {
            medal.sprite = silver.medal;
            medal.color = Color.white;
        } else if (playerScore >= bronze.score)
        {
            medal.sprite = bronze.medal;
            medal.color = Color.white;
        }

        playerScoreLabel.text = playerScore.ToString();
        bestScoreLabel.text = bestScore.ToString();

        startPanel.SetActive(false);
        pausePanel.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        gameoverPanel.SetActive(true);
        gameIsOver = true;
    }
}
