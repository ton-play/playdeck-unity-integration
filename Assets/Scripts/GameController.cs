using System;
using System.Collections.Generic;
using PlayDeck;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayDeckBridge _playDeckBridge;
    [SerializeField] private MessageViewer _messageViewer;
    
    [Header("Set Data")]
    [SerializeField] private InputField _setDataKeyInput;
    [SerializeField] private InputField _setDataValueInput;
    
    [Header("Get Data")]
    [SerializeField] private InputField _getDataKeyInput;
    
    [Header("Custom Share")]
    [SerializeField] private InputField _customShareKey;
    [SerializeField] private InputField _customShareValue;
    
    [Header("Get Share Link")]
    [SerializeField] private InputField _getShareLinkKey;
    [SerializeField] private InputField _getShareLinkValue;
    
    [Header("Open Telegram Link")]
    [SerializeField] private InputField _openTelegramLink;
    
    [Header("Payment Request")]
    [SerializeField] private InputField _paymentRequestAmount;
    [SerializeField] private InputField _paymentRequestDescription;
    [SerializeField] private InputField _paymentRequestExternalId;
    [SerializeField] private InputField _paymentRequestPhotoUrl;
    
    [Header("Get Payment Info")]
    [SerializeField] private InputField _getPaymentInfoExternalId;
    
    private string _setDataKeyValue;
    private string _setDataValueValue;
    
    private string _getDataKeyValue;

    private string _paymentRequestAmountValue;
    private string _paymentRequestDescriptionValue;
    private string _paymentRequestExternalIdValue;
    private string _paymentRequestPhotoUrlValue;
    
    private string _getPaymentInfoExternalIdValue;
    private string _customShareKeyValue;
    private string _customShareValueValue;
    
    private string _getShareLinkKeyValue;
    private string _getShareLinkValueValue;
    private string _openTelegramLinkValue;

    public void Start()
    {
        _paymentRequestAmount.onValueChanged.AddListener(val => _paymentRequestAmountValue = val);
        _paymentRequestDescription.onValueChanged.AddListener(val => _paymentRequestDescriptionValue = val);
        _paymentRequestExternalId.onValueChanged.AddListener(val => _paymentRequestExternalIdValue = val);
        _paymentRequestPhotoUrl.onValueChanged.AddListener(val => _paymentRequestPhotoUrlValue = val);
        _getPaymentInfoExternalId.onValueChanged.AddListener(val => _getPaymentInfoExternalIdValue = val);

        _customShareKey.onValueChanged.AddListener(val => _customShareKeyValue = val);
        _customShareValue.onValueChanged.AddListener(val => _customShareValueValue = val);
        
        _getShareLinkKey.onValueChanged.AddListener(val => _getShareLinkKeyValue = val);
        _getShareLinkValue.onValueChanged.AddListener(val => _getShareLinkValueValue = val);
        
        _getDataKeyInput.onValueChanged.AddListener(val => _getDataKeyValue = val);
        
        _setDataKeyInput.onValueChanged.AddListener(val => _setDataKeyValue = val);
        _setDataValueInput.onValueChanged.AddListener(val => _setDataValueValue = val);
        
        _openTelegramLink.onValueChanged.AddListener(val => _openTelegramLinkValue = val);
    }
    
    public void GetData()
    {
        _playDeckBridge.GetData(_getDataKeyValue, (data) =>
        {
            Debug.Log($"Data from PlayDeckBridge: [key: {_getDataKeyValue}][value: {data}]");
            
            _messageViewer.Show($"Data from PlayDeckBridge: [key: {_getDataKeyValue}][value: {data}]");
        });
    }
    
    public void GetUserProfile()
    {
        _playDeckBridge.GetUserProfile((userData) =>
        {
            Debug.Log($"UserData from PlayDeckBridge: {JsonUtility.ToJson(userData)}");
            
            _messageViewer.Show($"UserData from PlayDeckBridge: {JsonUtility.ToJson(userData)}");
        });
    }
    
    public void SetData()
    {
        _playDeckBridge.SetData(_setDataKeyValue, _setDataValueValue);
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
            
            _messageViewer.Show($"Request payment response: " + JsonUtility.ToJson(responseData));
            
            _playDeckBridge.OpenTelegramLink(responseData.url);
        });
    }
    
    public void GetPaymentInfo()
    {
        _playDeckBridge.GetPaymentInfo(new PlayDeckBridge.GetPaymentInfoRequestData()
        {
            externalId = _getPaymentInfoExternalIdValue,
        }, responseData =>
        {
            Debug.Log("[Unity] Get payment response: " + JsonUtility.ToJson(responseData));
            
            _messageViewer.Show($"Get payment response: " + JsonUtility.ToJson(responseData));
        });
    }

    public void CustomShare()
    {
        var @params = new Dictionary<string, string> { { _customShareKeyValue, _customShareValueValue } };
        
        Debug.Log($"{_customShareKeyValue}:{_customShareValueValue}");

        _playDeckBridge.CustomShare(@params);
    }
    
    public void GetShareLink()
    {
        var @params = new Dictionary<string, string> { { _getShareLinkKeyValue, _getShareLinkValueValue } };
        
        Debug.Log($"{_getShareLinkKeyValue}:{_getShareLinkValueValue}");
        
        _playDeckBridge.GetShareLink(@params, link =>
        {
            Debug.Log($"[Unity] Share Link: {link}");
            
            _messageViewer.Show($"Share Link: {link}");
        });
    }
    
    public void OpenTelegramLink()
    {
        _playDeckBridge.OpenTelegramLink(_openTelegramLinkValue);
    }
    
    public void GetPlaydeckState()
    {
        _playDeckBridge.GetPlaydeckState(state =>
        {
            Debug.Log($"[Unity] Get Playdeck State: {state}");
            
            _messageViewer.Show($"Get Playdeck State: {state}");
        });
    }
    
    public void GameEnd()
    {
        _playDeckBridge.GameEnd();
    }

    public void SendGameProgress()
    {
        _playDeckBridge.SendGameProgress(new PlayDeckBridge.AnalyticsGameProgress()
        {
            achievements = new List<PlayDeckBridge.AnalyticsAchievement>()
            {
                new PlayDeckBridge.AnalyticsAchievement()
                {
                    name = "test",
                    description = "test_description",
                    points = 5,
                    value = 6,
                    additional_data = new Dictionary<string, object>()
                    {
                        ["somekey"] = "somevalue"
                    }
                }
            },
            progress = new PlayDeckBridge.AnalyticsProgress()
            {
                level = 1,
                xp = 3
            }
        });
    }

    public void SendAnalyticsNewSession()
    {
        _playDeckBridge.SendAnalyticsNewSession();
    }
    
    public void SendAnalytics()
    {
        _playDeckBridge.SendAnalytics(new PlayDeckBridge.AnalyticsEvent()
        {
            name = "test_event",
            type = "click",
            event_properties = new Dictionary<string, string>()
            {
                ["some_event_property"] = "some_event_property_value"
            },
            user_properties = new Dictionary<string, string>()
            {
                ["some_user_property"] = "some_user_property_value"
            }
        });
    }
    
    public void ShowAd()
    {
        _playDeckBridge.ShowAd(
            OnRewardedAd,
            OnErrAd,
            OnSkipAd,
            OnNotFoundAd,
            OnStartAd);
    }
    
    private void OnRewardedAd(string data)
    {
        Debug.Log($"[Unity] OnRewardedAd: {data}");
        _messageViewer.Show($"OnRewardedAd: {data}");
    }
    
    private void OnErrAd(string data)
    {
        Debug.Log($"[Unity] OnErrAd: {data}");
        _messageViewer.Show($"OnErrAd: {data}");
    }
    
    private void OnSkipAd(string data)
    {
        Debug.Log($"[Unity] OnSkipAd: {data}");
        _messageViewer.Show($"OnSkipAd: {data}");
    }
    
    private void OnNotFoundAd(string data)
    {
        Debug.Log($"[Unity] OnNotFoundAd: {data}");
        _messageViewer.Show($"OnNotFoundAd: {data}");
    }

    private void OnStartAd(string data)
    {
        Debug.Log($"[Unity] OnStartAd: {data}");
        _messageViewer.Show($"OnStartAd: {data}");
    }
}