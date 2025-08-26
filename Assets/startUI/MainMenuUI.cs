using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("No UIDocument found on this GameObject!");
            return;
        }

        var root = uiDocument.rootVisualElement;

        // Play button
        Button playButton = root.Q<Button>("PlayButton");
        if (playButton != null)
            playButton.clicked += () =>
            {
                Debug.Log("Loading GameScene...");
                SceneManager.LoadScene("PlaneDet");
            };

        Button quitButton = root.Q<Button>("QuitButton");
        if (quitButton != null)
            quitButton.clicked += () =>
            {
                Debug.Log("Quitting game...");
                Application.Quit();
            };


    }
}
