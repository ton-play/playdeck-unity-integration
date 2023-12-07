# Unity PlayDeck Bridge

## Introduction
This guide provides step-by-step instructions for integrating PlayDeck into your Unity project. By following these steps, you'll be able to seamlessly set up and use PlayDeck in your project.

## Setup Guide

### Step 1: Download source code

### Step 2: Setup the PlayDeck Bridge
1. Begin by copying the "Assets/PlayDeck" folder into your Unity project "Assets" directory.
2. Open your start scene in Unity.
3. Drag and drop prefab "PlayDeckBridge" from the "Assets/PlayDeck/Prefabs" to the first scene in your project.
4. Integrate PlayDeck with your game using public methods from the "PlayDeckBridge".

### Step 3: Setting Up Your WebGL Template
- If you're using the provided custom WebGL template:
    - Simply copy and paste "WebGLTemplate" folder into your "Assets". And that's all, skip next steps.
- If you are using your own custom WebGL template:
    - Copy the `playDeckBridge.js` file to the same directory as your `index.html` file.

### (Additional) Step 4: Modifying index.html of custom WebGL template
1. **Adding the PlayDeck Script:**
    - Open your `index.html` file.
    - Inside the `<head></head>` tags, add the following line:
      ```html
      <script src="playDeckBridge.js"></script>
      ```
2. **Updating Unity Loading Code:**
    - Find the section in your `index.html` where Unity is loading (creating the Unity instance).
    - Update it using the following code example:
      ```javascript
      const playdeckBridgeInstance = playDeckBridge();
      createUnityInstance(canvas, config, (progress) => {
          playdeckBridgeInstance?.setLoadingProgress(progress * 100)
      }).then(unityInstance => {
          playdeckBridgeInstance?.init(unityInstance);
      });
      ```

### Step 5: Testing
- After making these changes, save your `index.html` file.
- Return to Unity, make and deploy build, test your project to ensure that PlayDeck is integrated and functioning correctly.

## Additional Notes
- This guide assumes a basic familiarity with Unity and web development.
- If you encounter any difficulties, you may contact us for additional help.
