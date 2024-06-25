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

        [SerializeField] private string _debugTelegramId;
        [SerializeField] private string _debugUsername;
        [SerializeField] private string _debugReferralId;
        [SerializeField] private PaymentResponseData _debugPaymentResponse;

        private UserData _userData;
        private string _dataJson;
        private GetScoreData _score;
        private PaymentResponseData _paymentResponseJson;
        
        private Action<UserData> _getUserCallback;
        private Action<string> _getDataCallback;
        private Action<GetScoreData> _getScoreCallback;
        private Action<PaymentResponseData> _paymentRequestCallback;

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
            public string telegramId;
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
        
        /// <summary>
        /// Represents the response data for a payment.
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
        public class PaymentRequestData
        {
            /// <summary>
            /// Gets or sets the amount.
            /// </summary>
            /// <value>
            /// The amount is represented as an integer.
            /// </value>
            public int amount;

            /// <summary>
            /// Gets or sets the description.
            /// </summary>
            /// <value>
            /// The description of the payment order.
            /// </value>
            public string description;

            /// <summary>
            /// Gets or sets the external identifier.
            /// </summary>
            /// <value>
            /// A unique order identifier in your system, which you can use to check in postback that payment was successful.
            /// </value>
            public string externalId;

            /// <summary>
            /// Gets or sets the photo URL.
            /// </summary>
            /// <value>
            /// The photo URL is an optional field.
            /// </value>
            public string photoUrl;
        }
    }
}