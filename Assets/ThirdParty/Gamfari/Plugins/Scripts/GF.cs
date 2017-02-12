using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using SimpleJSON;


namespace GamfariHandler
{
     public class GF : MonoBehaviour
    {

        #region Variables
        private static GAMFARI gamfari;
        private static GF m_Instance = null;
        private static string SavedUsername;
        private static string AdLink;
        private static Sprite AdImage;
        private static GameObject AdDisplay;
        private static GameObject SavedAd;
        private static string AdURL;
        private static string AD_ID;
        private static string hash;
        private static bool ShowingAds;
        private static GameObject LandScapeAD;
        private static GameObject PortraitAD;
        private static int LocalScore;
        #endregion

        #region Initialize
        public static void Init()
        {
            gamfari = Resources.Load<GAMFARI>("gamfari");
            AdDisplay = Resources.Load<GameObject>("AdObj");
            if (gamfari.GameID == 0 || gamfari.SecretKey == "")
            {
                Debug.LogError("Please Assign Game ID and Secret Key on the Gamfari Object Located at Assets/Gamfari/Resources");
            }
        }
        private void Awake()
        {
            gamfari = Resources.Load<GAMFARI>("gamfari");
            AdDisplay = Resources.Load<GameObject>("AdObj");
            if (gamfari.GameID == 0 || gamfari.SecretKey == "")
            {
                Debug.LogError("Please Assign Game ID and Secret Key on the Gamfari Object Located at Assets/Gamfari/Resources");
            }
           
        }
        private static GF Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = (GF)FindObjectOfType(typeof(GF));
                    if (m_Instance == null)
                        m_Instance = (new GameObject("GFRequest")).AddComponent<GF>();
                    DontDestroyOnLoad(m_Instance.gameObject);
                }
                return m_Instance;
            }
        }

        #endregion

        #region Coroutines
        private static Coroutine Get(string Username, System.Action<bool, string> aCallback)
        {
            return Instance.StartCoroutine(GeneralGetScores(Username, aCallback));
        }

        private static Coroutine Save(int Score, int CanCharge,System.Action<bool, string> aCallback)
        {
            return Instance.StartCoroutine(GeneralPostScores(Score,CanCharge, aCallback));
        }

        private static Coroutine Leaderboard(System.Action<bool,List<string>,string> aCallback)
        {
            return Instance.StartCoroutine(GeneralGetLeaderboard(aCallback));
        }

        private static Coroutine Register(string Username, string Password, string Email,System.Action<bool, string> aCallback)
        {
            return Instance.StartCoroutine(GeneralRegisterUser(Username,Password,Email,aCallback));
        }
        private static Coroutine GetStatus(System.Action<bool, string, Sprite> aCallback)
        {
            return Instance.StartCoroutine(GetingAdStatus(aCallback));
        }

        //private static Coroutine GetLink(System.Action<bool, string, Sprite> aCallback)
        //{
        //    return Instance.StartCoroutine(GetingAdLink(aCallback));
        //}

        private static Coroutine GetAd(System.Action<bool, string, Sprite> aCallback)
        {
            return Instance.StartCoroutine(GetingAd(aCallback));
        }
        #endregion

        #region Public Functions
        public static void GetScore(string Username, System.Action<bool, string> aCallback)
        {
            Get(Username, (success, text) =>
            {
                if (success)
                {
                    JSONNode jsonvalue = JSON.Parse(text);
                    bool webstatus = false;
                    bool.TryParse(jsonvalue["status"], out webstatus);

                    if (jsonvalue["msg"].ToString().Contains("Unable to find user"))
                    {
                        aCallback.Invoke(false, jsonvalue["msg"]);
                    }
                    else if (jsonvalue["msg"].ToString().Contains("Failed to authenticate: Incorrect auth details"))
                    {
                        aCallback.Invoke(false, jsonvalue["msg"]);
                    }
                    else if (jsonvalue["msg"].ToString().Contains("Unable to find developer"))
                    {
                        aCallback.Invoke(false, jsonvalue["msg"]);
                    }
                  
                    else
                    {
                        print(jsonvalue["msg"]);
                        int.TryParse(jsonvalue["data"], out LocalScore);
                        aCallback.Invoke(success, jsonvalue["data"]);
                    }
                }
                else
                {
                    aCallback.Invoke(false, "Connection failed");
                }
            });
        }

        public static void SaveScore(int Score,int CanCharge, System.Action<bool, string> aCallback)
        {
            if (SavedUsername != null)
            {
                Save(Score,CanCharge,(success, text) =>
                {
                    JSONNode jsonvalue = JSON.Parse(text);
                    bool webstatus = false;
                    bool.TryParse(jsonvalue["status"], out webstatus);

                    if (success)
                    {
                        if (jsonvalue["msg"].ToString().Contains("Insufficient fund to save score"))
                        {
                            aCallback.Invoke(false, jsonvalue["msg"]);
                        }
                        else if (jsonvalue["msg"].ToString().Contains("Unable to find user"))
                        {
                            aCallback.Invoke(false, jsonvalue["msg"]);
                        }
                        else if (jsonvalue["msg"].ToString().Contains("Page Not Found"))
                        {
                            aCallback.Invoke(false, "Error,Please Try Again in Some Minutes");
                        }
                        else if (jsonvalue["msg"].ToString().Contains("Failed to authenticate: Incorrect auth details"))
                        {
                            aCallback.Invoke(false, jsonvalue["msg"]);
                        }
                        else if (jsonvalue["msg"].ToString().Contains("query"))
                        {
                            aCallback.Invoke(false, "Querry limit exceeded");
                        }
                        else
                        {
                            print(jsonvalue["msg"].ToString());
                            aCallback.Invoke(success, jsonvalue["msg"]);
                        }
                    }
                    else
                    {
                        aCallback.Invoke(false, "Connection failed");
                    }
                });
            }
            else
            {
                aCallback.Invoke(false, "Please Log in user first");
            }
        }

        public static void RegisterUser(string Username, string Password, string Email, System.Action<bool, string> aCallback)
        {
                Register(Username, Password, Email, (success, text) =>
                {
                    if (success)
                    {
                        if (text.Contains("User already exists"))
                        {
                            aCallback.Invoke(false, text);
                        }
                        else if (text.Contains("Registration Failed"))
                        {
                            aCallback.Invoke(false, text);
                        }
                        else if (text.Contains("Page Not Found"))
                        {
                              aCallback.Invoke(false, text);
                        }
                        else if (text.Contains("Hash value is wrong"))
                        {
                            aCallback.Invoke(false, text);
                        }
                        else if (text.Contains("query"))
                        {
                            aCallback.Invoke(false, "Querry limit exceeded");
                        }
                        else if(text.Contains("Registration Successful"))
                        {
                            aCallback.Invoke(success, "Successfully Registered Player");
                        }
                        else
                        {
                            aCallback.Invoke(false, "Did not get any response from server");
                        }
                    }
                    else
                    {
                        aCallback.Invoke(false, text);
                        
                    }
                    print(text);
                });
            

        }

        public static void GetLeaderboard(System.Action<bool, List<string>,string> aCallback)
        {
                Leaderboard((success, text,msg) =>
                {
                    aCallback.Invoke(success, text, msg);
                });       
        }

        public static void FetchStatus(System.Action<bool, string> aCallback)
        {
            GetStatus((success, text, spr) =>
            {
                
                if (success)
                {
                    FetchAd(aCallback);
                }
                else
                {
                    aCallback.Invoke(success, "Ads Disabled");
                }
            });
        }

        private static void FetchAd(System.Action<bool, string> aCallback)
        {
            GetAd((success, text, spr) =>
            {
                if (success)
                {
                    AdImage = spr;
                    aCallback.Invoke(success, "Ads Ready");
                }
            });
        }

        public static void ShowAd()
        {
            if (AdImage != null)
            {
                SavedAd = Instantiate(AdDisplay) as GameObject;
                foreach (Transform TR in SavedAd.transform)
                {
                    if (TR.name == "MainAdL")
                    {
                        TR.GetComponent<Image>().sprite = AdImage;
                        TR.GetComponent<Button>().onClick.AddListener(VisitLink);
                        LandScapeAD = TR.gameObject;
                    }
                    else if (TR.name == "MainAdP")
                    {
                        TR.GetComponent<Image>().sprite = AdImage;
                        TR.GetComponent<Button>().onClick.AddListener(VisitLink);
                        PortraitAD = TR.gameObject;
                    }
                    else if (TR.name == "Cancel")
                    {
                        TR.GetComponent<Button>().onClick.AddListener(CloseAd);
                    }
                }
                if (Screen.orientation == ScreenOrientation.Landscape)
                {
                    LandScapeAD.SetActive(true);
                    PortraitAD.SetActive(false);
                }
                else
                {
                    PortraitAD.SetActive(true);
                    LandScapeAD.SetActive(false);
                }
                ShowingAds = true;
            }
            else
            {
                print("Did not get Ad from server");
            }
        }

        private static void CloseAd()
        {
            ShowingAds = false;
            Destroy(SavedAd);
        }

        private static void VisitLink()
        {
            string myhash = Instance.Md5Sum(gamfari.GameID + AD_ID + gamfari.SecretKey);
            string AdLinkUrl = gamfari.Adlink + gamfari.GameID + "/" + AD_ID + "/" + myhash;
            Application.OpenURL(AdLinkUrl);
        }

        #endregion

        #region IEnumerators
        private static IEnumerator GeneralGetScores(string Username, System.Action<bool, string> aCallback)
        {
            string hash = Instance.Md5Sum((gamfari.GameID + Username + gamfari.SecretKey));

            string post_url = gamfari.GetScoreURL + "/" + gamfari.GameID + "/" + hash + "/" + Username;

            WWW request = new WWW(post_url);
   
            yield return request;
            if (request.isDone)
            {

                if (string.IsNullOrEmpty(request.error) && aCallback != null)
                {
  
                   if (request.text.Contains("query"))
                    {
                        aCallback(false, "Querry limit exceeded");
                    }
                    else if (request.text.Contains("query"))
                    {
                        aCallback(false, "Querry limit exceeded");
                    }
                    else if (request.text.Contains("Access Denied"))
                    {
                        aCallback(false, "Access Denied");
                    }
                    else
                    {
                      
                            aCallback(true, request.text);
                            SavedUsername = Username;
                    }
                   

                }
                else
                {
                    if (aCallback != null)
                        aCallback(false, request.error);
                }
            }
        }

        private static IEnumerator GeneralPostScores(int Score,int CanCharge, System.Action<bool, string> aCallback)
        {
          
            string hash = Instance.Md5Sum(gamfari.GameID + SavedUsername + gamfari.SecretKey);


            string post_url = gamfari.SaveScoreURL + "/" + gamfari.GameID + "/" + hash + "/" + SavedUsername + "/" + (Score + LocalScore) +"/"+ CanCharge;
   
            WWW request = new WWW(post_url);

            yield return request;

            if (string.IsNullOrEmpty(request.error) && aCallback != null) 
            {
                if (request.text.Contains("query"))
                {
                    aCallback(false, "Querry limit exceeded");
                }
                else if (request.text.Contains("query"))
                {
                    aCallback(false, "Querry limit exceeded");
                }
                else if (request.text.Contains("Access Denied"))
                {
                    aCallback(false, "Access Denied");
                }
                else
                {

                    aCallback(true, request.text);
                }
            }
            else
            {
                if (aCallback != null)
                    aCallback(false, request.error);
            }
        }

        private static IEnumerator GeneralGetLeaderboard(System.Action<bool, List<string>, string> aCallback)
        {

            string hash = Instance.Md5Sum(gamfari.GameID + gamfari.SecretKey);

            string post_url = gamfari.LeaderboardURL + "/" + gamfari.GameID + "/" + hash;

            WWW request = new WWW(post_url);

            yield return request;

            JSONNode jsonvalue = JSON.Parse(request.text);

            bool webstatus = false;
            bool.TryParse(jsonvalue["status"], out webstatus);
            if (string.IsNullOrEmpty(request.error) && (aCallback != null))
            {
                List<string> Emptylst = new List<string>();

                if (request.text.Contains("query"))
                {
                    aCallback(false, Emptylst, "Querry limit exceeded");
                }
                else if (request.text.Contains("Access Denied"))
                {
                    aCallback(false, Emptylst, "Access Denied");
                }
                else if (request.text.Contains("Error"))
                {


                    if (aCallback != null)
                    {
                        List<string> lst = new List<string>();
                        lst.Add(request.error);
                        aCallback(false, lst, "Connection Failed");
                    }

                }
                else
                {
                    if (webstatus)
                    {
                        List<string> lst = new List<string>();
                        for (int i = 0; i < 5; i++)
                        {
                            lst.Add((jsonvalue["data"][i]["username"]) + " " + (jsonvalue["data"][i]["score"]) + " " + (jsonvalue["data"][i]["rank"]));
                        }
                        aCallback(webstatus, lst, jsonvalue["msg"]);
                    }

                    else
                    {
                        List<string> lst = new List<string>();
                        lst.Add(request.error);
                        aCallback(false, lst, "Connection Failed");
                    }
                }
            }
            else
            {

                if (aCallback != null)
                {
                    List<string> lst = new List<string>();
                    lst.Add(request.error);
                    aCallback(false, lst, request.error);
                }
            }
        }
       
        private static IEnumerator GeneralRegisterUser(string Username, string Password, string Email, System.Action<bool, string> aCallback)
        {
            if (Username.Length > 1 && Password.Length > 6 && Email.Length > 1 && Email.Contains("@")){
                string hash = Instance.Md5Sum(Password + Username + Email + "HerTa123@suMkey!?");

                string post_url = gamfari.RegisterURL + "&username=" + Username + "&password=" + Password + "&email=" + Email + "&hash=" + hash;

                WWW request = new WWW(post_url);
                yield return request;


                if (string.IsNullOrEmpty(request.error))
                {
                    if (aCallback != null)
                        aCallback(true, request.text);
                }
                else
                {
                    if (aCallback != null)
                        aCallback(false, request.error);
                }
            }
            else
            {
                if (aCallback != null)
                {
                    aCallback(false, "Please fill in correct information");
                }
            }
                  
        }

        private static IEnumerator GetingAdStatus(System.Action<bool, string, Sprite> aCallback)
        {
            string hash = Instance.Md5Sum(gamfari.GameID.ToString()+gamfari.SecretKey);
            string Post_Url = gamfari.GetAd + gamfari.GameID +  "/" + hash;
            WWW request = new WWW(Post_Url);

            yield return request;

            if (string.IsNullOrEmpty(request.error) && request.isDone)
            {
                JSONNode jsonvalue = JSON.Parse(request.text);

                if (aCallback != null)
                {
                    Sprite spr = null;
                    bool AdStatus = false;
                    bool.TryParse(jsonvalue["status"],out AdStatus);
                    if (AdStatus)
                    {
                        AdURL = jsonvalue["data"][1];
                        AD_ID = jsonvalue["data"][0];
                    }                    
                    aCallback(AdStatus,request.text, spr);
                }
            }
            else
            {
                if (aCallback != null)
                {
                    Sprite spr = null;
                    aCallback(false, request.error, spr);
                }
            }

        }

        public static IEnumerator GetingAd(System.Action<bool, string, Sprite> aCallback)
        {

            WWW request=null;
            if(!string.IsNullOrEmpty(AdURL))
            request = new WWW(AdURL);

            yield return request;

            if (string.IsNullOrEmpty(request.error)&&request.isDone)
            {
                if (aCallback != null)
                {
                    Texture2D _tex = request.texture;
                    Rect rec = new Rect(0, 0, _tex.width, _tex.height);
                    Sprite spr = Sprite.Create(_tex, rec, new Vector2(1f, 1f), 100);
                    aCallback(true, "Got ad", spr);
                }
            }
            else
            {
                if (aCallback != null)
                {
                    Sprite spr = null;
                    aCallback(false, "failed", spr);
                }
            }
        }

        #endregion

        #region Helpers
        private string Md5Sum(string strToEncrypt)
        {
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(strToEncrypt);

            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0');
        }
        #endregion

        void Update()
        {
           
            if (ShowingAds)
            {
                if (Screen.orientation == ScreenOrientation.Landscape)
                {
                    LandScapeAD.SetActive(true);
                    PortraitAD.SetActive(false);
                }
                else
                {
                    PortraitAD.SetActive(true);
                    LandScapeAD.SetActive(false);
                }
            }
        }

    }

}

