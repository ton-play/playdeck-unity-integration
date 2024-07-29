var playDeckBridge = (function() {
    const _wrapper = window.parent.window;
    var _unityInstance = null;

    const handleReceiveMessage = (message) => {
        const playdeck = message?.data?.playdeck;

        if (!playdeck) return;

        console.log(playdeck);

        if (playdeck.method === "getUserProfile") {
            _unityInstance?.SendMessage("PlayDeckBridge", "GetUserHandler", JSON.stringify(playdeck.value))
        }
        else if (playdeck.method === "getData") {
            _unityInstance?.SendMessage("PlayDeckBridge", "GetDataHandler", JSON.stringify(playdeck.value.data))
        }
        else if (playdeck.method === "requestPayment") {
            console.log(playdeck.value);
            _unityInstance?.SendMessage("PlayDeckBridge", "RequestPaymentHandler", JSON.stringify(playdeck.value))
        }
        else if (playdeck.method === "getPaymentInfo") {
            console.log(playdeck.value);
            _unityInstance?.SendMessage("PlayDeckBridge", "GetPaymentInfoHandler", JSON.stringify(playdeck.value))
        }
        else if (playdeck.method === "getShareLink") {
            console.log(playdeck.value);
            _unityInstance?.SendMessage("PlayDeckBridge", "GetShareLinkHandler", playdeck.value);
        }
        else if (playdeck.method === "getPlaydeckState") {
            console.log(playdeck.value);
            _unityInstance?.SendMessage("PlayDeckBridge", "GetPlaydeckStateHandler", playdeck.value ? 1 : 0 );
        }
        else if (playdeck.method === "rewardedAd") {
            console.log(playdeck.value);
            _unityInstance?.SendMessage("PlayDeckBridge", "RewardedAdHandler", JSON.stringify(playdeck.value) );
        }
        else if (playdeck.method === "errAd") {
            console.log(playdeck.value);
            _unityInstance?.SendMessage("PlayDeckBridge", "ErrAdHandler", JSON.stringify(playdeck.value) );
        }
        else if (playdeck.method === "skipAd") {
            console.log(playdeck.value);
            _unityInstance?.SendMessage("PlayDeckBridge", "SkipAdHandler", JSON.stringify(playdeck.value) );
        }
        else if (playdeck.method === "notFoundAd") {
            console.log(playdeck.value);
            _unityInstance?.SendMessage("PlayDeckBridge", "NotFoundAdHandler", JSON.stringify(playdeck.value) );
        }
        else if (playdeck.method === "startAd") {
            console.log(playdeck.value);
            _unityInstance?.SendMessage("PlayDeckBridge", "StartAdHandler", JSON.stringify(playdeck.value) );
        }
    }

    return {
        init: function(unityInstance){
            _unityInstance = unityInstance;
            window.addEventListener("message", handleReceiveMessage);
        },

        setLoadingProgress: function (progressValue) {
            _wrapper.postMessage({ playdeck: { method: "loading", value: progressValue } }, "*")
        }
    };
});
