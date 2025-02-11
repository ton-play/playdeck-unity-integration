using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PlayDeck
{
    public class PlayDeckBridge : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void PlayDeckBridge_PostMessage(string method);

        [DllImport("__Internal")]
        private static extern void PlayDeckBridge_PostMessage_IntValue(string method, int value);

        [DllImport("__Internal")]
        private static extern void PlayDeckBridge_PostMessage_StringValue(string method, string value);
        
        [DllImport("__Internal")]
        private static extern void PlayDeckBridge_PostMessage_SetData(string key, string value);
        
        [DllImport("__Internal")]
        private static extern void PlayDeckBridge_PostMessage_GetData(string key);
        
        [DllImport("__Internal")]
        private static extern void PlayDeckBridge_PostMessage_RequestPayment(string data);
        
        [DllImport("__Internal")]
        private static extern void PlayDeckBridge_PostMessage_GetPaymentInfo(string data);
        
        [DllImport("__Internal")]
        private static extern void PlayDeckBridge_PostMessage_CustomShare(string data);
        
        [DllImport("__Internal")]
        private static extern void PlayDeckBridge_PostMessage_GetShareLink(string data);
        
        [DllImport("__Internal")]
        private static extern void PlayDeckBridge_PostMessage_OpenTelegramLink(string data);
        
        [DllImport("__Internal")]
        private static extern void PlayDeckBridge_PostMessage_GetPlaydeckState();
        
        [DllImport("__Internal")]
        private static extern void PlayDeckBridge_PostMessage_GameEnd();
        
        [DllImport("__Internal")]
        private static extern void PlayDeckBridge_PostMessage_SendGameProgress(string data);
        
        [DllImport("__Internal")]
        private static extern void PlayDeckBridge_PostMessage_SendAnalytics(string data);
        
        [DllImport("__Internal")]
        private static extern void PlayDeckBridge_PostMessage_SendAnalyticsNewSession();
        
        [DllImport("__Internal")]
        private static extern void PlayDeckBridge_PostMessage_ShowAd();
        
        [SerializeField] private long _debugTelegramId;
        [SerializeField] private string _debugUsername;
        [SerializeField] private string _debugReferralId;
        [SerializeField] private PaymentResponseData _debugPaymentResponse;
        [SerializeField] private GetPaymentInfoResponseData _debugGetPaymentInfoResponse;
        [SerializeField] private bool _debugPlaydeckState;

        private UserData _userData;
        private string _dataJson;
        private PaymentResponseData _paymentResponseJson;
        private GetPaymentInfoResponseData _getPaymentInfoResponseJson;

        private Action<UserData> _getUserCallback;
        private Action<string> _getDataCallback;
        private Action<PaymentResponseData> _paymentRequestCallback;
        private Action<GetPaymentInfoResponseData> _getPaymentInfoRequestCallback;
        private Action<string> _invoiceClosedCallback;
        private Action<string> _getShareLinkCallback;
        private Action<bool> _getPlaydeckStateCallback;
        
        private Action<string> _rewardedAdCallback;
        private Action<string> _errAdCallback;
        private Action<string> _skipAdCallback;
        private Action<string> _notFoundAdCallback;
        private Action<string> _startAdCallback;

        public UserData User => _userData;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void GetData(string key, Action<string> callback)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake GetData[{key}]");
#else
            _getDataCallback = callback;

            Debug.Log($"[PlayDeckBridge]: GetData {key}");
         
            PlayDeckBridge_PostMessage_GetData(key);
#endif
        }

        public void SetData(string key, string data)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake SetData[{key}]: {data}");
#else
            Debug.Log($"[PlayDeckBridge]: SetData [{key}]: {data}");

            PlayDeckBridge_PostMessage_SetData(key, data);
#endif
        }

        public void GetUserProfile(Action<UserData> callback)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake GetUserProfile");

            _userData = new UserData()
            {
                telegramId = _debugTelegramId,
                username = _debugUsername
            };

            Debug.Log(JsonConvert.SerializeObject(_userData));

            callback?.Invoke(_userData);
#else
            _getUserCallback = callback;
            
            PlayDeckBridge_PostMessage(Constants.GET_USER_PROFILE);
#endif
        }

        public void RequestPayment(PaymentRequestData requestData, Action<PaymentResponseData> callback)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake RequestPayment");

            callback?.Invoke(_debugPaymentResponse);
#else
            _paymentRequestCallback = callback;

            var json = JsonConvert.SerializeObject(requestData);
            
            Debug.Log($"[PlayDeckBridge]: RequestPayment {json}");
            PlayDeckBridge_PostMessage_RequestPayment(json);
#endif
        }

        public void TrackInvoiceClosed(Action<string> callback)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake TrackInvoiceClosed");
#else
            _invoiceClosedCallback = callback;

            Debug.Log($"[PlayDeckBridge]: TrackInvoiceClosed");
#endif
        }

        public void GetPaymentInfo(GetPaymentInfoRequestData requestData, Action<GetPaymentInfoResponseData> callback)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake GetPaymentInfo");

            callback?.Invoke(_debugGetPaymentInfoResponse);
#else
            _getPaymentInfoRequestCallback = callback;

            var json = JsonConvert.SerializeObject(requestData);
            
            Debug.Log($"[PlayDeckBridge]: GetPaymentInfo {json}");
            PlayDeckBridge_PostMessage_GetPaymentInfo(json);
#endif
        }
        
        public void CustomShare(Dictionary<string, string> @params)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake CustomShare");
#else
            var json = JsonConvert.SerializeObject(@params);

            Debug.Log($"[PlayDeckBridge]: CustomShare {json}");
            PlayDeckBridge_PostMessage_CustomShare(json);
#endif
        }
        
        public void GetShareLink(Dictionary<string, string> @params, Action<string> callback)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake GetShareLink");
            
            callback?.Invoke("some_query");
#else
            var json = JsonConvert.SerializeObject(@params);

            Debug.Log($"[PlayDeckBridge]: GetShareLink {json}");
            PlayDeckBridge_PostMessage_GetShareLink(json);
            
            _getShareLinkCallback = callback;
#endif
        }
        
        public void OpenTelegramLink(string link)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake OpenTelegramLink");
#else
            Debug.Log($"[PlayDeckBridge]: OpenTelegramLink {link}");
            PlayDeckBridge_PostMessage_OpenTelegramLink(link);
#endif
        }
        
        public void GetPlaydeckState(Action<bool> callback)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake GetPlaydeckState");
            
            callback?.Invoke(_debugPlaydeckState);
#else
            Debug.Log($"[PlayDeckBridge]: GetPlaydeckState");
            PlayDeckBridge_PostMessage_GetPlaydeckState();
            
            _getPlaydeckStateCallback = callback;
#endif
        }
        
        public void GameEnd()
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake GameEnd");
#else
            Debug.Log($"[PlayDeckBridge]: GameEnd");
            PlayDeckBridge_PostMessage_GameEnd();
#endif
        }
        
        public void SendGameProgress(AnalyticsGameProgress analyticsGameProgress)
        {
            
            var json = JsonConvert.SerializeObject(analyticsGameProgress);
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake SendGameProgress {json}");
#else
            Debug.Log($"[PlayDeckBridge]: SendGameProgress {json}");
            PlayDeckBridge_PostMessage_SendGameProgress(json);
#endif
        }
        
        public void SendAnalyticsNewSession()
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake SendAnalyticsNewSession");
#else
            Debug.Log($"[PlayDeckBridge]: SendAnalyticsNewSession");
            PlayDeckBridge_PostMessage_SendAnalyticsNewSession();
#endif
        }
        
        public void SendAnalytics(AnalyticsEvent analyticsEvent)
        {
            var json = JsonConvert.SerializeObject(analyticsEvent);
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake SendAnalytics {json}");
#else
            Debug.Log($"[PlayDeckBridge]: SendAnalytics {json}");
            PlayDeckBridge_PostMessage_SendAnalytics(json);
#endif
        }
        
        public void ShowAd(
            Action<string> rewardedAdCallback,
            Action<string> errAdCallback,
            Action<string> skipAdCallback,
            Action<string> notFoundAdCallback,
            Action<string> startAdCallback)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake ShowAd");
            
            rewardedAdCallback?.Invoke("");
#else
            Debug.Log($"[PlayDeckBridge]: ShowAd");

            _rewardedAdCallback = rewardedAdCallback;
            _errAdCallback = errAdCallback;
            _skipAdCallback = skipAdCallback;
            _notFoundAdCallback = notFoundAdCallback;
            _startAdCallback = startAdCallback;
            
            PlayDeckBridge_PostMessage_ShowAd();
#endif
        }

        private void GetUserHandler(string userJson)
        {
            var converted = JsonConvert.DeserializeObject<UserData>(userJson);
            _userData = converted;
            _getUserCallback?.Invoke(converted);
        }
        
        private void GetDataHandler(string dataJson)
        {
            _dataJson = dataJson;
            _getDataCallback?.Invoke(_dataJson);
        }
        
        private void RequestPaymentHandler(string paymentRequestJson)
        {
            var converted = JsonConvert.DeserializeObject<PaymentResponseData>(paymentRequestJson);
            _paymentResponseJson = converted;
            _paymentRequestCallback?.Invoke(converted);
        }
        
        private void GetPaymentInfoHandler(string getPaymentInfoJson)
        {
            var settings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
            var converted = JsonConvert.DeserializeObject<GetPaymentInfoResponseData>(getPaymentInfoJson, settings);
            _getPaymentInfoResponseJson = converted;
            _getPaymentInfoRequestCallback?.Invoke(converted);
        }

        private void InvoiceClosedHandler(string status)
        {
            _invoiceClosedCallback?.Invoke(status);
        }

        private void GetShareLinkHandler(string shareLink)
        {
            _getShareLinkCallback?.Invoke(shareLink);
        }
        
        private void GetPlaydeckStateHandler(int state)
        {
            _getPlaydeckStateCallback?.Invoke(state != 0);
        }
        
        private void RewardedAdHandler(string data)
        {
            _rewardedAdCallback?.Invoke(data);
        }
        
        private void ErrAdHandler(string data)
        {
            _errAdCallback?.Invoke(data);
        }
        
        private void SkipAdHandler(string data)
        {
            _skipAdCallback?.Invoke(data);
        }
        
        private void NotFoundAdHandler(string data)
        {
            _notFoundAdCallback?.Invoke(data);
        }
        
        private void StartAdHandler(string data)
        {
            _startAdCallback?.Invoke(data);
        }

        [Serializable]
        public class UserData
        {
            public string avatar;
            public string username;
            public string firstName;
            public string lastName;
            public long telegramId;
            public string locale;
            public string token;
            public string sessionId;
            public ulong currentGameStarted;
            
            public Dictionary<string, string> @params;
        }

        public class GetScoreData
        {
            public long position;
            public int score;
        }
        
        public class Wallet
        {
            public string type;
            public float balance;
        }
        
        [Serializable]
        public class PaymentRequestData
        {
            /// <value>
            /// The amount is represented as an integer.
            /// </value>
            public int amount;
            
            /// <value>
            /// The description of the payment order.
            /// </value>
            public string description;
            
            /// <value>
            /// A unique order identifier in your system, which you can use to check in postback that payment was successful.
            /// </value>
            public string externalId;
            
            /// <value>
            /// The photo URL is an optional field.
            /// </value>
            public string photoUrl;
        }
        
        /// <summary>
        /// Represents the response data for a payment request.
        /// </summary>
        [Serializable]
        public class PaymentResponseData
        {
            /// <summary>
            /// The URL for the purchase via Telegram.
            /// </summary>
            public string url;
        }
        
        [Serializable]
        public class GetPaymentInfoRequestData
        {
            /// <value>
            /// A unique order identifier in your system, which you can use to check in postback that payment was successful.
            /// </value>
            public string externalId;
        }
        
        /// <summary>
        /// Represents the response data for concrete payment.
        /// </summary>
        [Serializable]
        public class GetPaymentInfoResponseData
        {
            /// <summary>
            /// The URL for the purchase via Telegram.
            /// </summary>
            public bool paid;

            public long telegramId;

            public long dateTime;
        }
        
        /// <summary>
        /// Represents an achievement to be sent in the game progress analytics event.
        /// </summary>
        [Serializable]
        public class AnalyticsAchievement
        {
            /// <summary>
            /// The name of the achievement.
            /// </summary>
            public string name;

            /// <summary>
            /// The description of the achievement.
            /// </summary>
            public string description;

            /// <summary>
            /// The points awarded for achieving the achievement.
            /// </summary>
            public int points;

            /// <summary>
            /// The value associated with the achievement.
            /// </summary>
            public int value;

            /// <summary>
            /// Additional data related to the achievement.
            /// </summary>
            public Dictionary<string, object> additional_data;
        }

        /// <summary>
        /// Represents the player's progress to be sent in the game progress analytics event.
        /// </summary>
        [Serializable]
        public class AnalyticsProgress
        {
            /// <summary>
            /// The current level of the player's progress.
            /// </summary>
            public int level;

            /// <summary>
            /// The current experience points of the player's progress.
            /// </summary>
            public int xp;
        }

        /// <summary>
        /// Represents the overall game progress data to be sent in the analytics event.
        /// </summary>
        [Serializable]
        public class AnalyticsGameProgress
        {
            /// <summary>
            /// A list of achievements to be included in the game progress analytics event.
            /// </summary>
            public List<AnalyticsAchievement> achievements;

            /// <summary>
            /// The player's progress data to be included in the game progress analytics event.
            /// </summary>
            public AnalyticsProgress progress;
        }

        /// <summary>
        /// Represents an analytics event.
        /// </summary>
        [Serializable]
        public class AnalyticsEvent
        {
            /// <summary>
            /// The name of the event.
            /// </summary>
            public string name;

            /// <summary>
            /// The type of the event.
            /// </summary>
            public string type;

            /// <summary>
            /// A dictionary of user properties associated with the event.
            /// </summary>
            public Dictionary<string, string> user_properties;

            /// <summary>
            /// A dictionary of event properties associated with the event.
            /// </summary>
            public Dictionary<string, string> event_properties;
        }
    }
}