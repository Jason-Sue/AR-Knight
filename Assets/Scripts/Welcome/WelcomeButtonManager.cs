using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WelcomeButtonManager : MonoBehaviour
{
    public Button EnterButton;
    public Button ExitButton;

    // Start is called before the first frame update
    void Start()
    {
        EnterButton.onClick.AddListener(EnterGame);
        ExitButton.onClick.AddListener(ExitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EnterGame()
    {
        SceneManager.LoadScene("ARScene");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
