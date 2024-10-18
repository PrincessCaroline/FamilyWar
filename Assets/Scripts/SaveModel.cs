using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveModel
{
    // Instance statique unique de la classe
    private static SaveModel    _instance;
    private static string       _filePath = Path.Combine("C:\\Users\\Jeremy\\Documents\\Projects\\Unity\\Family War Game\\Assets\\Saves", "data.json");

    // Propriété publique pour accéder à l'instance unique
    public static SaveModel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SaveModel();
            }
            return _instance;
        }
    }

    public void Save(ApplicationModel applicationModel)
    {
        string json = JsonConvert.SerializeObject(applicationModel, Formatting.Indented);

        
        File.WriteAllText(_filePath, json);
    }

    public ApplicationModel Load() 
    {
        ApplicationModel applicationModel;

        string json = File.ReadAllText(_filePath);

        applicationModel = JsonConvert.DeserializeObject<ApplicationModel>(json);

        if (applicationModel == null) 
        {
            applicationModel = new ApplicationModel();
        }

        return applicationModel;
    }
}
