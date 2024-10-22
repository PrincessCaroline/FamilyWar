using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    UIDocument Root;

    [SerializeField]
    UIDocument ActionPanel;

    [SerializeField]
    VisualTreeAsset ChildrenItemTemplate;

    private const string PRENOM_PLACEHOLDER = "PRENOM";
    private const int AGE_PLACEHOLDER = 0;

    private Guid _selectedPlayer;

    private void OnEnable()
    {
        // Initialize Main
        Button ShowActionPanelButton = Root.rootVisualElement.Q<Button>("AddChildrenButton");
        ShowActionPanelButton.clicked += () => { _selectedPlayer = Guid.Empty; ShowActionPanel(true); };

        // Initialize InsertPanel
        Button ActionButton = ActionPanel.rootVisualElement.Q<Button>("InsertChildren");
        ActionButton.clicked += () => ActionPlayer();

        ShowActionPanel(false);
        LoadPlayerList();
    }

    #region UI
    private void ShowActionPanel(bool show)
    {
        ActionPanel.rootVisualElement.style.display = (show) ? DisplayStyle.Flex : DisplayStyle.None;

        if (show)
        {
            ResetActionPanel();
        }
    }

    private void ResetActionPanel()
    {
        TextField firstnameInput = ActionPanel.rootVisualElement.Q<TextField>("FirstnameInput");
        IntegerField ageInput = ActionPanel.rootVisualElement.Q<IntegerField>("AgeInput");

        if (_selectedPlayer == Guid.Empty)
        {
            firstnameInput.value = PRENOM_PLACEHOLDER;
            ageInput.value = AGE_PLACEHOLDER;
        }
        else
        {
            ApplicationModel backup = SaveModel.Instance.Load();

            firstnameInput.value = backup.players.FirstOrDefault(p => p.id == _selectedPlayer).name;
            ageInput.value = backup.players.FirstOrDefault(p => p.id == _selectedPlayer).age;
        }
    }

    private void LoadPlayerList()
    {
        ApplicationModel backup = SaveModel.Instance.Load();

        if (backup != null)
        {
            Root.rootVisualElement.Q<ScrollView>("ChildrenListRoot").Clear();

            foreach (PlayerModel player in backup.players)
            {
                TemplateContainer PlayerItemTemp = ChildrenItemTemplate.Instantiate();

                // SET Name
                Label name = PlayerItemTemp.Q<Label>("Name");
                if (name != null)
                    name.text = player.name;
                else
                    Debug.LogWarning($"Could not find \"Name\" Label.");

                // SET Age
                Label age = PlayerItemTemp.Q<Label>("Age");
                if (age != null)
                    age.text = player.age.ToString();
                else
                    Debug.LogWarning($"Could not find \"age\" Label.");

                // SET DeleteButton
                Button deletePlayerButton = PlayerItemTemp.Q<Button>("DeleteButton");
                if (deletePlayerButton != null)
                    deletePlayerButton.clicked += () => DeletePlayer(player.id);
                else
                    Debug.LogWarning($"Could not find \"age\" Label.");

                PlayerItemTemp.RegisterCallback<ClickEvent>((evt) => { { _selectedPlayer = player.id; }; ShowActionPanel(true); });

                // Child the template to the UIDocument so it will be rendered and updated
                Root.rootVisualElement.Q<ScrollView>("ChildrenListRoot").Add(PlayerItemTemp);
            }
        }
    }
    #endregion

    private void ActionPlayer()
    {
        if (_selectedPlayer == Guid.Empty)
        {
            InsertPlayer();
        }
        else
        {
            UpdatePlayer();
        }
    }

    private void InsertPlayer()
    {
        TextField firstnameInput = ActionPanel.rootVisualElement.Q<TextField>("FirstnameInput");
        IntegerField ageInput = ActionPanel.rootVisualElement.Q<IntegerField>("AgeInput");

        PlayerModel newPlayer = new PlayerModel();
        newPlayer.name = firstnameInput.text;
        newPlayer.age = ageInput.value;
        newPlayer.id = Guid.NewGuid();

        /// SAVE DATA
        ApplicationModel backup = SaveModel.Instance.Load();
        backup.players.Add(newPlayer);
        SaveModel.Instance.Save(backup);

        LoadPlayerList();
        ShowActionPanel(false);
    }

    private void UpdatePlayer()
    {
        TextField firstnameInput = ActionPanel.rootVisualElement.Q<TextField>("FirstnameInput");
        IntegerField ageInput = ActionPanel.rootVisualElement.Q<IntegerField>("AgeInput");

        ApplicationModel backup = SaveModel.Instance.Load();
        backup.players.FirstOrDefault(p => p.id == _selectedPlayer).name = firstnameInput.text;
        backup.players.FirstOrDefault(p => p.id == _selectedPlayer).age = ageInput.value;
        SaveModel.Instance.Save(backup);

        LoadPlayerList();
        ShowActionPanel(false);
    }

    private void DeletePlayer(Guid idPlayer)
    {
        ApplicationModel backup = SaveModel.Instance.Load();
        backup.players.RemoveAll(p => p.id == idPlayer);
        SaveModel.Instance.Save(backup);

        LoadPlayerList();
        ShowActionPanel(false);
    }
}
