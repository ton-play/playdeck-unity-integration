using System;
using PlayDeck;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private const string DATA_KEY = "test";

    [SerializeField] private PlayDeckBridge _playDeckBridge;
    [SerializeField] private InputField _setDataInput;
    [SerializeField] private InputField _setScoreInput;
    
    [Header("Payment Request")]
    [SerializeField] private InputField _paymentRequestAmount;
    [SerializeField] private InputField _paymentRequestDescription;
    [SerializeField] private InputField _paymentRequestExternalId;
    [SerializeField] private InputField _paymentRequestPhotoUrl;
    
    [Header("Get Payment Info")]
    [SerializeField] private InputField _getPaymentInfoExternalId;
    
    private string _paymentRequestAmountValue;
    private string _paymentRequestDescriptionValue;
    private string _paymentRequestExternalIdValue;
    private string _paymentRequestPhotoUrlValue;
    
    private string _getPaymentInfoExternalIdValue;

    public void Start()
    {
        _paymentRequestAmount.onValueChanged.AddListener(val => _paymentRequestAmountValue = val);
        _paymentRequestDescription.onValueChanged.AddListener(val => _paymentRequestDescriptionValue = val);
        _paymentRequestExternalId.onValueChanged.AddListener(val => _paymentRequestExternalIdValue = val);
        _paymentRequestPhotoUrl.onValueChanged.AddListener(val => _paymentRequestPhotoUrlValue = val);
        _getPaymentInfoExternalId.onValueChanged.AddListener(val => _getPaymentInfoExternalIdValue = val);
    }

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

    public void RequestPayment()
    {
        _playDeckBridge.RequestPayment(new PlayDeckBridge.PaymentRequestData()
        {
            amount = Convert.ToInt32(_paymentRequestAmountValue),
            description = _paymentRequestDescriptionValue,
            externalId = _paymentRequestExternalIdValue,
            photoUrl = _paymentRequestPhotoUrlValue
        }, responseData =>
        {
            Debug.Log("[Unity] Request payment response: " + JsonUtility.ToJson(responseData));
            Application.OpenURL(responseData.url);
        });
    }
    
    public void GetPaymentInfo()
    {
        _playDeckBridge.GetPaymentInfo(new PlayDeckBridge.GetPaymentInfoRequestData()
        {
            externalId = _getPaymentInfoExternalIdValue,
        }, responseData =>
        {
            Debug.Log("[Unity] Request payment response: " + JsonUtility.ToJson(responseData));
        });
    }
}