using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public void ExitFromApp()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("SceneMain");
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("SceneGame");
    }

}
