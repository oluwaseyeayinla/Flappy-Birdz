using UnityEngine;

public class GAMFARI : ScriptableObject {
    public int GameID;
    public string SecretKey;
    [HideInInspector]
    public string GetAd = "http://gamfari.com/developers/api/get_ads/";
    [HideInInspector]
    public string Adlink = "http://gamfari.com/developers/api/get_ad/";
    [HideInInspector]
    public string SaveScoreURL = "http://gamfari.com/developers/api/save_score";
    [HideInInspector]
    public string GetScoreURL= "http://gamfari.com/developers/api/get_score";
    [HideInInspector]
    public string LeaderboardURL= "http://gamfari.com/developers/api/get_score_board";
    [HideInInspector]
    public string RegisterURL= "http://gamfari.com/portals/game/scripts/mobile_reg.php?";



}
