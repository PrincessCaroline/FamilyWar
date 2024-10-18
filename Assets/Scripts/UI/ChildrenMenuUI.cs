using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ChildrenMenuUI : MonoBehaviour
{
    [SerializeField]
    UIDocument Root;

    [SerializeField]
    UIDocument InsertPanel;

    [SerializeField] 
    VisualTreeAsset ChildrenItemTemplate;

    private void OnEnable()
    {
        // Initialize Main
        Button ShowInsertPlayerPanelButton = Root.rootVisualElement.Q<Button>("AddChildrenButton");
        ShowInsertPlayerPanelButton.clicked += () => ShowPanel(InsertPanel, true);

        // Initialize InsertPanel
        Button InsertPlayerButton = InsertPanel.rootVisualElement.Q<Button>("InsertChildren");
        InsertPlayerButton.clicked += () => InsertPlayer();

        ShowPanel(InsertPanel, false);
        LoadPlayerList();
    }

    public void ShowPanel(UIDocument panel, bool show)
    {
        panel.rootVisualElement.style.display = (show) ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void InsertPlayer()
    {
        TextField FirstnameInput = InsertPanel.rootVisualElement.Q<TextField>("FirstnameInput");
        IntegerField AgeInput = InsertPanel.rootVisualElement.Q<IntegerField>("AgeInput");

        PlayerModel newPlayer = new PlayerModel();
        newPlayer.name = FirstnameInput.text;
        newPlayer.age = AgeInput.value;

        /// SAVE DATA
        ApplicationModel backup = SaveModel.Instance.Load();
        backup.players.Add(newPlayer);
        SaveModel.Instance.Save(backup);

        LoadPlayerList();
        ShowPanel(InsertPanel, false);
    }


    public void LoadPlayerList()
    {
        ApplicationModel backup = SaveModel.Instance.Load();

        if (backup != null)
        {
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
