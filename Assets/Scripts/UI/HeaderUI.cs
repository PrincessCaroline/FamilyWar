using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HeaderUI : MonoBehaviour
{
    public UIDocument Root;
    public Application Application;
    public string Title;

    private void OnEnable()
    {
        Label ChildrenMenuTitle = Root.rootVisualElement.Q<Label>("Title");
        ChildrenMenuTitle.text = Title;

        Button BackButton = Root.rootVisualElement.Q<Button>("BackButton");
        BackButton.clicked += () => Application.GoToMainMenu();
    }
}
