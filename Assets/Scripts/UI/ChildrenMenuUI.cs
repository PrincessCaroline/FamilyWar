using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ChildrenMenuUI : MonoBehaviour
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
        Button ShowInsertPlayerPanelButton = Root.rootVisualElement.Q<Button>("AddChildrenButton");
        ShowInsertPlayerPanelButton.clicked += () => { _selectedPlayer = Guid.Empty; ShowPanel(ActionPanel, true); };

        // Initialize InsertPanel
        Button InsertPlayerButton = ActionPanel.rootVisualElement.Q<Button>("InsertChildren");
        InsertPlayerButton.clicked += () => ActionPlayer();

        ShowPanel(ActionPanel, false);
        LoadPlayerList();
    }

    public void ShowPanel(UIDocument panel, bool show)
    {
        panel.rootVisualElement.style.display = (show) ? DisplayStyle.Flex : DisplayStyle.None;

        if (show)
        {
            ResetPanel(panel);
        }
    }

    public void ResetPanel(UIDocument panel)
    {
        TextField firstnameInput = panel.rootVisualElement.Q<TextField>("FirstnameInput");
        IntegerField ageInput = panel.rootVisualElement.Q<IntegerField>("AgeInput");

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

    public void ActionPlayer()
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

    public void InsertPlayer()
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
        ShowPanel(ActionPanel, false);
    }

    public void UpdatePlayer()
    {
        TextField firstnameInput = ActionPanel.rootVisualElement.Q<TextField>("FirstnameInput");
        IntegerField ageInput = ActionPanel.rootVisualElement.Q<IntegerField>("AgeInput");

        ApplicationModel backup = SaveModel.Instance.Load();
        backup.players.FirstOrDefault(p => p.id == _selectedPlayer).name = firstnameInput.text;
        backup.players.FirstOrDefault(p => p.id == _selectedPlayer).age = ageInput.value;
        SaveModel.Instance.Save(backup);

        LoadPlayerList();
        ShowPanel(ActionPanel, false);
    }

    public void LoadPlayerList()
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

                PlayerItemTemp.RegisterCallback<ClickEvent>((evt) => { { _selectedPlayer = player.id; }; ShowPanel(ActionPanel, true); });

                // Child the template to the UIDocument so it will be rendered and updated
                Root.rootVisualElement.Q<ScrollView>("ChildrenListRoot").Add(PlayerItemTemp);
            }
        }
    }
}





// [x] Init bouton
// [] Init list
// [] Read JSON

// [] Add children to list
// [] write JSON

// [] Clic sur element de la liste
