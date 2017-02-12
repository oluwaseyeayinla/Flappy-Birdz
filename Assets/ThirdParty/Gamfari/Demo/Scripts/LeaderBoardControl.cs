using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GamfariHandler;

public class LeaderBoardControl : MonoBehaviour
{

    //Login/Scores Users
    public Text Score;
    public Text Status;
    public InputField User;
    int LocalUserScore;

    public GameObject LeaderBoardCanvas;
    public GameObject LoggedIn;
    public GameObject NotLoggedIn;
    public List<GameObject> ScoreBoards = new List<GameObject>();

    int Leadercount;
    string Username;

    List<string> UserName = new List<string>();
    List<string> UserScore = new List<string>();
    List<string> UserRank = new List<string>();

    //Register Users
    public InputField RUsername;
    public InputField REmail;
    public InputField RPassword;
    public GameObject RegisterCanvas;

    public void Awake()
    {
        GF.Init();
    }

    // Update is called once per frame
    void Update ()
    {
        Username = User.text;
	}

    /// <summary>
    /// Method to login and get users score from server
    /// </summary>
    public void GetScores()
    {
        if (Username.Length > 0)
        {
            //GF.GetScore Collects Username and a function to call if retrieving of score was successful
            GF.GetScore(Username, RequestScoreStatus);
        }
        else
        {
            Status.text = "Please Enter Username";
        }
    }

    /// <summary>
    /// Method to Register Users
    /// </summary>
    public void RegisterUser()
    {
       
        if (RUsername.text.Length > 0 && RPassword.text.Length > 0 && REmail.text.Length > 0)
        {
            if (RPassword.text.Length < 6)
            {
                Status.text = "Please enter more than 6 characters for password";
            }
            else
            {
                //GF.Register Collects Username,password and email  and a function to call if retrieving of score was successful
                GF.RegisterUser(RUsername.text, RPassword.text, REmail.text, RequestRegisterStatus);
            }

        }
        else
        {
            Status.text = "Please enter all details";

        }
    }

    /// <summary>
    /// Method to save scores to server
    /// </summary>
    public void SaveScores()
    {
        //GF.GetScore Collects Score and a function to call if retrieving of score was successful
        //Please note userscore most have been retrieved before sending
        //You should also check if the score you got is lower than the score you are about to send, depending on your game type
        GF.SaveScore(Random.Range(100,200),1, RequestSaveStatus);
    }

    /// <summary>
    /// Method to retrieve leaderboard from server
    /// </summary>
    public void GetLeaderboard()
    {
        //GF.GetScore Collects a function to call if retrieving of score was successful
        GF.GetLeaderboard(RequestLeaderboardStatus);
    }

    /// <summary>
    /// Create a method with two arguements
    /// The First (bool) will receive the status of request
    /// The Second (string) will get the score if it was successful
    /// Callback is always a string
    /// if true you can attempt to convert to int for use
    /// </summary>
    /// <param name="status"></param>
    /// <param name="ScoreCallBack"></param>
    public void RequestScoreStatus(bool status,string ScoreCallBack)
    {
        
        if (status)
        {
            int.TryParse(ScoreCallBack, out LocalUserScore);
            Score.text = LocalUserScore.ToString();
            LoggedIn.SetActive(true);
            NotLoggedIn.SetActive(false);
            Status.text = "Successfully logged in user and got scores";
        }
        else
        {
            Status.text = ScoreCallBack;
        }
       
    }

    /// <summary>
    /// Create a method with two arguements
    /// The First (bool) will receive the status of request
    /// The Second (string) will get the status text if it was successful
    /// </summary>
    /// <param name="status"></param>
    /// <param name="SaveCallBack"></param>
    public void RequestSaveStatus(bool status, string SaveCallBack)
    {
        if (status)
        {
            Status.text = "Successfully Saved Score";
            GF.GetScore(Username, RequestScoreStatus);
        }
        else
        {
            Status.text = SaveCallBack;
        }
    }
    /// <summary>
    /// The First (bool) will receive the status of request
    /// The Second (string) will get the status text if it was successful
    /// </summary>
    /// <param name="status"></param>
    /// <param name="RegisterCallBack"></param>
    public void RequestRegisterStatus(bool status, string RegisterCallBack)
    {
        if (status)
        {
            Status.text = RegisterCallBack;
            RegisterCanvas.SetActive(false);
        }
        else
        {
            Status.text = RegisterCallBack;
        }
    }
    /// <summary>
    /// The First (bool) will receive the status of request
    /// The Second (List of type string) will get the status text if it was successful
    /// Each row of the string contains the username,score and rank of user seperated by space
    /// You can split by space to get individual text
    /// </summary>
    /// <param name="status"></param>
    /// <param name="LeaderboardCallBack"></param>
    public void RequestLeaderboardStatus(bool status, List<string> LeaderboardCallBack,string msg)
    {
        if (status)
        {
            SplitUsers(LeaderboardCallBack.ToArray());
            Status.text = msg;               
        }
        else
        {
            Status.text = msg;
       
        }
    }

    /// <summary>
    /// Logs out user
    /// </summary>
    public void LogOut()
    {
        LoggedIn.SetActive(false);
        NotLoggedIn.SetActive(true);
        LeaderBoardCanvas.SetActive(false);
        User.text = "";
        UserName.Clear();
        UserScore.Clear();
        UserRank.Clear();
        Leadercount = 0;
        Score.text = 0.ToString();
    }

    /// <summary>
    /// Collects an array and splits it to get each data
    /// Used in creating leaderboard
    /// </summary>
    /// <param name="Users"></param>
    public void SplitUsers(string[] Users)
    {
        UserName.Clear();
        UserScore.Clear();
        UserRank.Clear();
        Leadercount = Users.Length;
        string[] split;

        for (int i = 0; i < Users.Length; i++)
        {
            split = Users[i].Split(' ');
            UserName.Add(split[0]);
            UserScore.Add(split[1]);
            UserRank.Add(split[2]);
        }
         CreateBoard();
    }

    /// <summary>
    /// Creates a leaderboad and assigns each text to it
    /// </summary>
    public void CreateBoard()
    {
        for (int i = 0; i < Leadercount; i++)
        {
            GameObject Go = ScoreBoards[i];
            foreach (Transform TR in Go.transform)
            {
                switch (TR.name)
                {
                    case "Username":
                        ChangeText(UserName[i], TR);
                        break;
                    case "Score":
                        ChangeText(UserScore[i], TR);
                        break;
                    case "Rank":
                        ChangeText(UserRank[i], TR);
                        break;
                }
            }
        }
        LeaderBoardCanvas.SetActive(true);
    }
    /// <summary>
    /// Used to change text, collects transform and check for child text to assign new text
    /// </summary>
    /// <param name="NewText"></param>
    /// <param name="TR"></param>
    public void ChangeText(string NewText, Transform TR)
    {
        foreach (Transform trans in TR)
        {
            trans.GetComponent<Text>().text = NewText;
        }
    }
}
