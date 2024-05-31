using System;
using PlayDeck;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private const string DATA_KEY = "test";

    [SerializeField] private PlayDeckBridge _playDeckBridge;
    [SerializeField] private InputField _setDataInput;
    [SerializeField] private InputField _setScoreInput;
    
    public void GetScore()
    {
        _playDeckBridge.GetScore((score) => Debug.Log($"Score from PlayDeckBridge: [position: {score.position}, score: {score.score}]"));
    }
    
    public void GetData()
    {
        _playDeckBridge.GetData(DATA_KEY, (data) => Debug.Log($"Data from PlayDeckBridge: [key: {DATA_KEY}][value: {data}]"));
    }
    
    public void GetUserProfile()
    {
        _playDeckBridge.GetUserProfile((userData) => Debug.Log($"UserData from PlayDeckBridge: {JsonUtility.ToJson(userData)}"));
    }
    
    public void SetScore()
    {
        _playDeckBridge.SetScore(Convert.ToInt32(_setScoreInput.text));
    }
    
    public void SetData()
    {
        _playDeckBridge.SetData(DATA_KEY, _setDataInput.text);
    }
}