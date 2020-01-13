using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public GameObject authorsPanel, startPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenAuthors()
    {
        startPanel.SetActive(false);
        authorsPanel.SetActive(true);
    }

    public void OpenStartPanel()
    {
        startPanel.SetActive(true);
        authorsPanel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
        SceneManager.UnloadSceneAsync("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
