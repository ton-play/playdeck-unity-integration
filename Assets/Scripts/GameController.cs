using System;
using PlayDeck;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayDeckBridge _playDeckBridge;
    
    public void GetScore()
    {
        _playDeckBridge.GetScore((score) => Debug.Log($"Score from PlayDeckBridge: [position: {score.position}, score: {score.score}]"));
    }
    
    public void GetData()
    {
        _playDeckBridge.GetData((data) => Debug.Log($"Data from PlayDeckBridge: {data}"));
    }
    
    public void GetUserProfile()
    {
        _playDeckBridge.GetUserProfile((userData) => Debug.Log($"UserData from PlayDeckBridge: {JsonUtility.ToJson(userData)}"));
    }
    
    public void SetScore(string input)
    {
        _playDeckBridge.SetScore(Convert.ToInt32(input));
    }
    
    public void SetData(string input)
    {
        _playDeckBridge.SetData(input);
    }

    public void SetAd(string link)
    {
        _playDeckBridge.SetAd(link);
    }
}