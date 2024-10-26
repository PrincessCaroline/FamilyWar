using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private const string PRENOM_PLACEHOLDER = "PRENOM";
    private const int AGE_PLACEHOLDER = 0;

    private TemplateContainer _mainPanelRoot;
    private TemplateContainer _actionPanelRoot;
    private VisualTreeAsset _childrenItemTemplate;

    private Guid _selectedPlayer;

    public void Initialize(TemplateContainer mainPanelRoot, TemplateContainer actionPanelRoot, VisualTreeAsset childrenItemTemplate)
    {
        _mainPanelRoot = mainPanelRoot;
        _actionPanelRoot = actionPanelRoot;
        _childrenItemTemplate = childrenItemTemplate;

        // Initialize Main
        Button ShowActionPanelButton = _mainPanelRoot.Q<Button>("AddPlayerButton");
        ShowActionPanelButton.clicked += () => { _selectedPlayer = Guid.Empty; ShowActionPanel(true); };

        // Initialize ActionPanel
        Button ActionButton = _actionPanelRoot.Q<Button>("InsertPlayer");
        ActionButton.clicked += () => ActionPlayer();

        ShowActionPanel(false);
        LoadPlayerList();
    }

    #region UI
    private void ShowActionPanel(bool show)
    {
        _actionPanelRoot.style.display = (show) ? DisplayStyle.Flex : DisplayStyle.None;

        if (show)
        {
            ResetActionPanel();
        }
    }

    private void ResetActionPanel()
    {
        TextField firstnameInput = _actionPanelRoot.Q<TextField>("FirstnameInput");
        IntegerField ageInput = _actionPanelRoot.Q<IntegerField>("AgeInput");

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
            _mainPanelRoot.Q<ScrollView>("PlayerListRoot").Clear();

            foreach (PlayerModel player in backup.players)
            {
                TemplateContainer PlayerItemTemp = _childrenItemTemplate.Instantiate();

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
                _mainPanelRoot.Q<ScrollView>("PlayerListRoot").Add(PlayerItemTemp);
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
        TextField firstnameInput = _actionPanelRoot.Q<TextField>("FirstnameInput");
        IntegerField ageInput = _actionPanelRoot.Q<IntegerField>("AgeInput");

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
        TextField firstnameInput = _actionPanelRoot.Q<TextField>("FirstnameInput");
        IntegerField ageInput = _actionPanelRoot.Q<IntegerField>("AgeInput");

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
