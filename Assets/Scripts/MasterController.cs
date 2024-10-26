using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.LowLevel;
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
    VisualTreeAsset MainPageContent;

    [SerializeField]
    VisualTreeAsset ActionPanelContent;

    [SerializeField]
    VisualTreeAsset ListItemTemplate;

    private Guid _selectedItem = Guid.Empty;

    private void OnEnable()
    {
        Label menuTitle = Root.rootVisualElement.Q<Label>("HeaderTitle");
        menuTitle.text = Title;

        Button backButton = Root.rootVisualElement.Q<Button>("BackButton");
        backButton.clicked += () => SceneManager.LoadScene((int)GoToScene);

        TemplateContainer mainContent = MainPageContent.Instantiate();
        TemplateContainer actionContent = ActionPanelContent.Instantiate();

        VisualElement ContentRoot = Root.rootVisualElement.Q<VisualElement>("Content");
        ContentRoot.Add(mainContent);
        ContentRoot.Add(actionContent);
        mainContent.StretchToParentWidth();
        actionContent.StretchToParentWidth();

        // Initialize Main
        PlayerController playerController = GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.Initialize(mainContent, actionContent, ListItemTemplate);
        }
    }
}
