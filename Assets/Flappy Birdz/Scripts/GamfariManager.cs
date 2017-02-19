#if UNITY_IOS && !UNITY_EDITOR || UNITY_ANDROID && !UNITY_EDITOR
#define IOS_OR_ANDROID
#endif

using GamfariHandler;
using UnityEngine;

public class GamfariManager : MonoBehaviour {

    public string url = "http://www.gamfari.com/portals/register.php";

    public string username = "oluwaseyeayinla";
        
    void Awake()
    {
        GF.Init();
    }

    public void OnLoginButton()
    {
       GF.GetScore(username, delegate(bool status, string result)
       {
           int score;
           if (status)
           {
               Debug.Log("Result: " + result);

               if (int.TryParse(result, out score))
               {
                   Debug.Log("Score retrieved: " + score);
               }
               else
               {
                   score = 0;
               }

               Debug.Log("Local score: " + score);
           }
           else
           {
               Debug.Log("Result: " + result);
           }
       });
    }

    void OpenURL(string url)
    {
        #if IOS_OR_ANDROID
	    InAppBrowser.DisplayOptions options = new InAppBrowser.DisplayOptions();
        options.androidBackButtonCustomBehaviour = false;
        options.displayURLAsPageTitle = false;
        options.hidesTopBar = true;
        options.pageTitle = "Gamfari Signup";
        options.pinchAndZoomEnabled = true;
        InAppBrowser.OpenURL(url, options);
        #elif UNITY_WEBGL && !UNITY_EDITOR
		Application.ExternalEval("window.open(" + url + ");");
        #else
        Application.OpenURL(url);
        #endif
    }
}
