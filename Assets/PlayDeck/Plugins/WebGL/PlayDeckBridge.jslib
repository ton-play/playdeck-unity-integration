mergeInto(LibraryManager.library, {
  PlayDeckBridge_PostMessage: function (method) {
    const parent = window.parent.window;
    parent.postMessage(
      { playdeck: { method: UTF8ToString(method) } },
      "*"
    );
  },

  PlayDeckBridge_PostMessage_IntValue: function (method, value) {
    const parent = window.parent.window;
    parent.postMessage(
      { playdeck: { method: UTF8ToString(method), value: value } },
      "*"
    );
  },
  
  PlayDeckBridge_PostMessage_StringValue: function (method, value) {
    const parent = window.parent.window;
    parent.postMessage(
      { playdeck: { method: UTF8ToString(method), value: UTF8ToString(value) } },
      "*"
    );
  },
  
  PlayDeckBridge_PostMessage_SetData: function (key, value) {
    const parent = window.parent.window;
    parent.postMessage(
      { playdeck: { method: "setData", key: UTF8ToString(key), value: UTF8ToString(value) } },
      "*"
    );
  },
     
  PlayDeckBridge_PostMessage_GetData: function (key) {
    const parent = window.parent.window;
    parent.postMessage(
      { playdeck: { method: "getData", key: UTF8ToString(key)} },
      "*"
    );
  }
});