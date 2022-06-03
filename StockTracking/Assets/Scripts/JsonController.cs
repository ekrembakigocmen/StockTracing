
using System.IO;
using UnityEngine;

public class JsonController : MonoBehaviour
{
    
    EmployeeList employeeList = new EmployeeList();
    DailyRecordController dailyRecordController = new DailyRecordController();
    private string jsonString = "Workers.json";
    private string jsonString2 = "DailyRecord.json";
    

    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + jsonString))
        {
            return;
        }
        else
        {
            Debug.Log("dosya olusturuldu");
            Save();
            SaveDaily();
        }
    }



    public void Save()
    {
            
            string jsonWrite = JsonUtility.ToJson(employeeList);
            File.WriteAllText(Application.persistentDataPath + jsonString, jsonWrite);
       

    }

    public void SaveDaily()
    {
        string jsonWrite = JsonUtility.ToJson(dailyRecordController);
        File.WriteAllText(Application.persistentDataPath + jsonString2, jsonWrite);

    }

    public void Load()
    {

        if (File.Exists(Application.persistentDataPath + jsonString))
        {
            string jsonRead = File.ReadAllText(Application.persistentDataPath + jsonString);

            employeeList = JsonUtility.FromJson<EmployeeList>(jsonRead);
            

        }
        else
            Debug.Log("Dosya bulunamadi");

    }

    public void LoadDaily()
    {
        if (File.Exists(Application.persistentDataPath + jsonString2))
        {
            string jsonRead = File.ReadAllText(Application.persistentDataPath + jsonString2);

            dailyRecordController = JsonUtility.FromJson<DailyRecordController>(jsonRead);


        }
        else
            Debug.Log("Dosya bulunamadi");
    }
}
