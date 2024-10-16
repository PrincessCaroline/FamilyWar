using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Application : MonoBehaviour
{
    #region Navigation
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GoToChildrenMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToTaskMenu()
    {
        SceneManager.LoadScene(2);
    }

    public void GoToRewardMenu()
    {
        SceneManager.LoadScene(3);
    }
    #endregion
}
