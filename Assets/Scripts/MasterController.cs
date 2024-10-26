using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MasterController : MonoBehaviour
{
    [SerializeField]
    public UIDocument Root;

    [SerializeField]
    public string Title;

    [SerializeField]
    public ConfigurationModel.Scene GoToScene;

    [SerializeField]
    VisualTreeAsset PageContent;

    private void OnEnable()
    {
        Label ChildrenMenuTitle = Root.rootVisualElement.Q<Label>("HeaderTitle");
        ChildrenMenuTitle.text = Title;

        Button BackButton = Root.rootVisualElement.Q<Button>("BackButton");
        BackButton.clicked += () => SceneManager.LoadScene((int)GoToScene);

        TemplateContainer Page = PageContent.Instantiate();

        // DO STUFF

        VisualElement ContentRoot = Root.rootVisualElement.Q<VisualElement>("Content");
        ContentRoot.Add(Page);
    }
}
