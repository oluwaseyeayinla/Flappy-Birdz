using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GamfariHandler;

public class AdsManager : MonoBehaviour {

    bool CanShowAds;
    public Text StatusText;

	// Use this for initialization
	void Start ()
    {
        GF.Init();
        GF.FetchStatus(AdsStatusCallback);
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void AdsStatusCallback(bool Status,string text)
    {
        if (Status)
        {
            CanShowAds = Status;
            StatusText.text = text;
            print(text);
        }
        else
        {
            StatusText.text = text;
            print(text);
        }
    }

    public void ShowAds()
    {
        if (CanShowAds)
        {
            GF.ShowAd();
        }
        else
        {
            print("Ads not ready yet");
        }
    }
}
