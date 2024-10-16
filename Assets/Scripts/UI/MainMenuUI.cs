using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    public UIDocument Root;
    public Application Application;

    private void OnEnable()
    {
        Button ChildrenMenuButton = Root.rootVisualElement.Q<Button>("ChildrenMenu");
        ChildrenMenuButton.clicked += () => Application.GoToChildrenMenu();

        Button TaskMenuButton = Root.rootVisualElement.Q<Button>("TaskMenu");
        TaskMenuButton.clicked += () => Application.GoToTaskMenu();

        Button RewardMenuButton = Root.rootVisualElement.Q<Button>("RewardMenu");
        RewardMenuButton.clicked += () => Application.GoToRewardMenu();
    }
}
