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
        else if (playdeck.method === "getScore") {
            _unityInstance?.SendMessage("PlayDeckBridge", "GetScoreHandler", JSON.stringify(playdeck.value))
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
        else if (playdeck.method === "setScore") {
            console.log(playdeck);
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
