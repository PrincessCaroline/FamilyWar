using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainController : MonoBehaviour
{
    public UIDocument Root;

    private void OnEnable()
    {
        Button ChildrenMenuButton = Root.rootVisualElement.Q<Button>("PlayerMenu");
        ChildrenMenuButton.clicked += () => SceneManager.LoadScene((int)ConfigurationModel.Scene.PLAYER);

        Button TaskMenuButton = Root.rootVisualElement.Q<Button>("TaskMenu");
        TaskMenuButton.clicked += () => SceneManager.LoadScene((int)ConfigurationModel.Scene.TASK);

        Button RewardMenuButton = Root.rootVisualElement.Q<Button>("RewardMenu");
        RewardMenuButton.clicked += () => SceneManager.LoadScene((int)ConfigurationModel.Scene.REWARD);
    }
}
