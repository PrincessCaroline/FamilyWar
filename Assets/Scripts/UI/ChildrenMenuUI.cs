using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Text.Json;

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
        Button AddChildrenButton = Root.rootVisualElement.Q<Button>("AddChildrenButton");
        AddChildrenButton.clicked += () => { InsertPanel.enabled = true; };

        Button InsertChildrenButton = InsertPanel.rootVisualElement.Q<Button>("InsertChildren");
        InsertChildrenButton.clicked += () => InsertChildren();

        InitializeChildrenList();
    }

    public void InsertChildren()
    {
        TextField FirstnameInput = InsertPanel.rootVisualElement.Q<TextField>("FirstnameInput");
        IntegerField AgeInput = InsertPanel.rootVisualElement.Q<IntegerField>("AgeInput");

        ChildrenModel temp = new ChildrenModel();
        temp.name = FirstnameInput.text;
        temp.age = AgeInput.intValue;


        
 

        TemplateContainer childrenItemTemp = ChildrenItemTemplate.Instantiate();

        Label name = childrenItemTemp.Q<Label>("Name");
        if (name != null)
            name.text = "1";
        else
            Debug.LogWarning($"Could not find \"Name\" Label.");

        Label age = childrenItemTemp.Q<Label>("Age");
        if (age != null)
            age.text = "1";
        else
            Debug.LogWarning($"Could not find \"age\" Label.");

        //child the template to the UIDocument so it will be rendered and updated
        Root.rootVisualElement.Q<ScrollView>("ChildrenListRoot").Add(childrenItemTemp);
    }

    public void InitializeChildrenList()
    {
        // Read JSON
        // Empty List
        // Init List
    }
}





// [x] Init bouton
// [] Init list
// [] Read JSON

// [] Add children to list
// [] write JSON

// [] Clic sur element de la liste
