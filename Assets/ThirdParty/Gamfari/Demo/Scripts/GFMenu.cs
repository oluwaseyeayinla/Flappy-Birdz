using UnityEngine;

public class GFMenu : MonoBehaviour {

    public void LoadLeaderboardScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LearderboardDemoScene");
    }
    public void LoadAdsScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("AdDemo");
    }
    public void LoadMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GFMenu");
    }
}
