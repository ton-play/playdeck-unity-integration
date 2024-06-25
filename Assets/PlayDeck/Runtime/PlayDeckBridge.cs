using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;

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

        [SerializeField] private long _debugTelegramId;
        [SerializeField] private string _debugUsername;
        [SerializeField] private string _debugReferralId;
        [SerializeField] private PaymentResponseData _debugPaymentResponse;
        [SerializeField] private GetPaymentInfoResponseData _debugGetPaymentInfoResponse;

        private UserData _userData;
        private string _dataJson;
        private GetScoreData _score;
        private PaymentResponseData _paymentResponseJson;
        private GetPaymentInfoResponseData _getPaymentInfoResponseJson;

        private Action<UserData> _getUserCallback;
        private Action<string> _getDataCallback;
        private Action<GetScoreData> _getScoreCallback;
        private Action<PaymentResponseData> _paymentRequestCallback;
        private Action<GetPaymentInfoResponseData> _getPaymentInfoRequestCallback;

        public UserData User => _userData;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void GetScore(Action<GetScoreData> callback)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake GetScore");
#else
            _getScoreCallback = callback;
            
            PlayDeckBridge_PostMessage(Constants.GET_SCORE);
#endif
        }

        public void SetScore(int score)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake SetScore: {score}");
#else
            PlayDeckBridge_PostMessage_IntValue(Constants.SET_SCORE, score);
#endif
        }

        public void GetData(string key, Action<string> callback)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake GetData[{key}]");
#else
            _getDataCallback = callback;
         
            PlayDeckBridge_PostMessage_GetData(key);
#endif
        }

        public void SetData(string key, string data)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake SetData[{key}]: {data}");
#else
            PlayDeckBridge_PostMessage_SetData(key, data);
#endif
        }
        
        public void SetAd(string link)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake SetAd: {link}");
#else
            PlayDeckBridge_PostMessage_StringValue(Constants.SET_AD, link);
#endif
        }

        public void GetUserProfile(Action<UserData> callback)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake GetUserProfile");

            _userData = new UserData()
            {
                telegramId = _debugTelegramId,
                username = _debugUsername,
                @params = new UserData.Params()
                {
                    @default = _debugReferralId
                }
            };

            Debug.Log(JsonUtility.ToJson(_userData));

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

            var json = JsonUtility.ToJson(requestData);
            
            Debug.Log($"[PlayDeckBridge]: RequestPayment {json}");
            PlayDeckBridge_PostMessage_RequestPayment(json);
#endif
        }
        
        public void GetPaymentInfo(GetPaymentInfoRequestData requestData, Action<GetPaymentInfoResponseData> callback)
        {
#if UNITY_EDITOR
            Debug.Log($"[PlayDeckBridge]: Fake GetPaymentInfo");

            callback?.Invoke(_debugGetPaymentInfoResponse);
#else
            _getPaymentInfoRequestCallback = callback;

            var json = JsonUtility.ToJson(requestData);
            
            Debug.Log($"[PlayDeckBridge]: GetPaymentInfo {json}");
            PlayDeckBridge_PostMessage_GetPaymentInfo(json);
#endif
        }

        private void GetUserHandler(string userJson)
        {
            var converted = JsonUtility.FromJson<UserData>(userJson);
            _userData = converted;
            _getUserCallback?.Invoke(converted);
        }

        private void GetScoreHandler(string getScoreDataJson)
        {
            var converted = JsonUtility.FromJson<GetScoreData>(getScoreDataJson);
            _score = converted;
            _getScoreCallback?.Invoke(_score);
        }

        private void GetDataHandler(string dataJson)
        {
            _dataJson = dataJson;
            _getDataCallback?.Invoke(_dataJson);
        }
        
        private void RequestPaymentHandler(string paymentRequestJson)
        {
            var converted = JsonUtility.FromJson<PaymentResponseData>(paymentRequestJson);
            _paymentResponseJson = converted;
            _paymentRequestCallback?.Invoke(converted);
        }
        
        private void GetPaymentInfoHandler(string getPaymentInfoJson)
        {
            var converted = JsonUtility.FromJson<GetPaymentInfoResponseData>(getPaymentInfoJson);
            _getPaymentInfoResponseJson = converted;
            _getPaymentInfoRequestCallback?.Invoke(converted);
        }

        [Serializable]
        public class UserData
        {
            public string avatar;
            public bool canSeeOnboarding;
            public string firstName;
            public string lastName;
            public ulong gameSessionsCount;
            public string locale;
            public string sessionId;
            public long telegramId;
            public string token;
            public string username;
    
            public Params @params;

            public List<Wallet> wallets;

            [Serializable]
            public class Params
            {
                public string @default;
            }
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
    }
}