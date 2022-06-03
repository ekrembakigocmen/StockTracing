
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using UnityEngine.UI;


public class SystemManager : MonoBehaviour
{
    EmployeeList newEmployeeList = new EmployeeList();
    DailyRecordController dailyRecordController = new DailyRecordController();

    public GameObject zReportPanel;
    public GameObject checkPanel;

    [Header("Button Area")]
    public Button tic;
    public Button addPersonBtn;
    public Button calculateBtn;
    public Button saveBtn;
    public Button dataBtn;
    public Button closeBtn;
    public Button zReportBtn;

    [Header("InputField Area")]
    public TMP_InputField addName;
    public TMP_InputField total;
    public TMP_InputField sent;

    [Header("Text Area")]
    public TextMeshProUGUI remain;
    public TextMeshProUGUI sentTxt;
    public TextMeshProUGUI totalTxt;
    public TextMeshProUGUI carTourTxt;
    public TextMeshProUGUI totalDoneTxt;

    [Header("Employee List Area")]
    public EmployeeList[] employeeList;
    public List<JsonDailyRecord> recordListInstance;



    [Header("Layout List Area")]
    public TMP_InputField[] inputInfoPiece;
    public TextMeshProUGUI[] inputInfoName;
    public TextMeshProUGUI[] inputInfoTotal;
    public GameObject[] dailyList;
    private string jsonString = "Workers.json";
    private string jsonString2 = "DailyRecord.json";


    private int tempDay = 0;



    private void Start()
    {

        Load();
        LoadDaily();
        dailyRecordController.dailyRecords = new JsonDailyRecord[10];

        zReportPanel.SetActive(false);
        if (newEmployeeList.remain != 0)
        {

            total.gameObject.SetActive(false);
            tic.gameObject.SetActive(false);

        }
        else
        {

            addName.gameObject.SetActive(false);
            sent.gameObject.SetActive(false);
            totalDoneTxt.gameObject.SetActive(false);
            addPersonBtn.interactable = false;
            calculateBtn.interactable = false;
            saveBtn.interactable = false;
            zReportBtn.interactable = false;

        }



        for (int i = 0; i < 10; i++)
        {

            if (recordListInstance[i].day == 0)
            {
                tempDay = i;

                i = 10;
            }

        }


        for (int i = 0; i < employeeList[0].personName.Length; i++)
        {

            if (newEmployeeList.personName[i] != "")
            {
                inputInfoName[i].text = newEmployeeList.personName[i];
                employeeList[0].personName[i] = newEmployeeList.personName[i];
                employeeList[0].personTotal[i] = newEmployeeList.personTotal[i];

                inputInfoTotal[i].text = newEmployeeList.personTotal[i].ToString();
                totalDoneTxt.text = (Convert.ToInt32(newEmployeeList.total) - employeeList[0].remain).ToString();
                inputInfoName[i].gameObject.SetActive(true);
                inputInfoPiece[i].gameObject.SetActive(true);
                inputInfoTotal[i].gameObject.SetActive(true);
            }
            if (newEmployeeList.remain != 0)
            {
                remain.text = "Kalan : " + newEmployeeList.remain.ToString();
                totalTxt.text = "Toplam : " + newEmployeeList.total;
                sentTxt.text = "Gönderilen : " + newEmployeeList.sent;
                carTourTxt.text = "Teslimat     : " + newEmployeeList.carTour;
                employeeList[0].remain = newEmployeeList.remain;
                employeeList[0].sent = newEmployeeList.sent;
                employeeList[0].carTour = newEmployeeList.carTour;
            }
        }
        if (newEmployeeList.remain <= 0)
        {
            remain.text = "Kalan : " + 0.ToString();
        }
    }
    public void Update()
    {
        if (addName.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            AddEmployee();

        }
        if (Input.GetKeyDown(KeyCode.Tab) && InputManager.index != -1 && InputManager.select)
        {
            PressTAB();
        }

    }
    public void AddEmployee()
    {
        if (addName.text != "")
        {
            for (int i = 0; i < employeeList[0].personName.Length; i++)
            {
                if (employeeList[0].personName[i] == "")
                {
                    employeeList[0].personName[i] = addName.text;
                    inputInfoName[i].text = addName.text;
                    inputInfoName[i].gameObject.SetActive(true);
                    inputInfoPiece[i].gameObject.SetActive(true);
                    inputInfoTotal[i].gameObject.SetActive(true);
                    addName.Select();
                    addName.text = "";
                    return;
                }
            }
        }
        
    }


    public void Calculate()
    {
        int temp = 0;

        for (int i = 0; i < inputInfoPiece.Length; i++)
        {

            if (inputInfoPiece[i].text != "")
            {
                temp += Convert.ToInt32(inputInfoPiece[i].text);

                if (inputInfoTotal[i].text == "")
                {
                    inputInfoTotal[i].text = inputInfoPiece[i].text.ToString();

                }
                else
                {
                    inputInfoTotal[i].text = (Convert.ToInt32(newEmployeeList.personTotal[i]) + Convert.ToInt32(inputInfoPiece[i].text)).ToString();
                    newEmployeeList.personTotal[i] = Convert.ToInt32(inputInfoTotal[i].text);

                }
            }
        }
        employeeList[0].remain -= temp;
        totalDoneTxt.text = (Convert.ToInt32(newEmployeeList.total) - employeeList[0].remain).ToString();
        remain.text = "Kalan : " + employeeList[0].remain;


    }
    public void TotalBtn()
    {
        if (total.text != "")
        {
            totalTxt.text = "Toplam : " + total.text.ToString();
            remain.text = "Kalan : " + total.text.ToString();
            sentTxt.text = "Gönderilen : " + employeeList[0].sent;
            carTourTxt.text = "Teslimat     : " + employeeList[0].carTour.ToString();
            totalDoneTxt.text = employeeList[0].sent.ToString();
            employeeList[0].remain = Convert.ToInt32(total.text);
            employeeList[0].total = total.text;
            total.gameObject.SetActive(false);
            tic.gameObject.SetActive(false);
            addName.gameObject.SetActive(true);
            sent.gameObject.SetActive(true);
            totalDoneTxt.gameObject.SetActive(true);
            addPersonBtn.interactable = true;
            calculateBtn.interactable = true;
            saveBtn.interactable = true;
            zReportBtn.interactable = true;
            newEmployeeList.remain = employeeList[0].remain;
            newEmployeeList.total = employeeList[0].total;
        }
        SaveJson();
    }

    public void SentCalculate()
    {
        newEmployeeList.sent = Convert.ToInt32(sent.text) + employeeList[0].sent;
        newEmployeeList.carTour = employeeList[0].carTour + 1;
        employeeList[0].sent = newEmployeeList.sent;
        employeeList[0].carTour = newEmployeeList.carTour;
        carTourTxt.text = "Teslimat     : " + newEmployeeList.carTour.ToString();
        sentTxt.text = "Gönderilen : " + newEmployeeList.sent.ToString();
        sent.text = "";
    }


    public void Save()
    {
        bool a = true;
        for (int i = 0; i < employeeList[0].personName.Length; i++)
        {

            if (employeeList[0].personName[i] != "")
            {
                newEmployeeList.personName[i] = employeeList[0].personName[i];
            }
            if (employeeList[0].personName[i] != "" && inputInfoTotal[i].text != "")
            {

                newEmployeeList.personTotal[i] = Convert.ToInt32(inputInfoTotal[i].text);
            }
            if (a)
            {
                newEmployeeList.remain = employeeList[0].remain;
                a = false;
            }
            if (sent.text != "")
            {
                SentCalculate();
            }
            SaveJson();
        }

    }

    public void Load()
    {

        if (File.Exists(Application.persistentDataPath + jsonString))
        {
            string jsonRead = File.ReadAllText(Application.persistentDataPath + jsonString);

            newEmployeeList = JsonUtility.FromJson<EmployeeList>(jsonRead);


        }
        else
            Debug.Log("Dosya bulunamadi");

    }

    public void SaveJson()
    {
        string jsonWrite = JsonUtility.ToJson(newEmployeeList);
        File.WriteAllText(Application.persistentDataPath + jsonString, jsonWrite);

    }

    public void SaveDaily()
    {
        string jsonWrite = JsonUtility.ToJson(dailyRecordController);
        File.WriteAllText(Application.persistentDataPath + jsonString2, jsonWrite);
    }

    public void LoadDaily()
    {
        if (File.Exists(Application.persistentDataPath + jsonString2))
        {
            string jsonRead = File.ReadAllText(Application.persistentDataPath + jsonString2);

            dailyRecordController = JsonUtility.FromJson<DailyRecordController>(jsonRead);

            for (int i = 0; i < dailyRecordController.dailyRecords.Length; i++) // kayitlari cektigimiz gibi ornek listeye atiyoruz
            {
                recordListInstance[i] = dailyRecordController.dailyRecords[i];
            }
        }
        else
            Debug.Log("Dosya bulunamadi");

    }

    public void PressTAB()
    {

        if (InputManager.index + 1 >= inputInfoPiece.Length || !inputInfoPiece[InputManager.index + 1].gameObject.activeSelf)
        {
            InputManager.index = -1;
        }
        InputManager.index++;
        inputInfoPiece[InputManager.index].Select();
    }

    public void GetDataBtn()
    {
        DailyDataSheet();
        zReportPanel.SetActive(true);
    }

    public void CloseBtn()
    {

        zReportPanel.SetActive(false);
    }

    public void YesBtn()
    {
       
        recordListInstance[tempDay].day = tempDay + 1;
        for (int i = 0; i < 10; i++)
        {
            if (inputInfoPiece[i].text != "")
            {

                recordListInstance[tempDay].dailyPiece[i] = Convert.ToInt32(inputInfoPiece[i].text);
                recordListInstance[tempDay].dailyTotal += recordListInstance[tempDay].dailyPiece[i];
                recordListInstance[tempDay].nameArray[i] = inputInfoName[i].text.ToString();

            }
        }
        for (int i = 0; i < 10; i++)
        {
            dailyRecordController.dailyRecords[i] = recordListInstance[i];
        }
        if (recordListInstance[tempDay].day < 10)
        {
            SaveDaily();
        }

        zReportBtn.interactable = false;
        checkPanel.SetActive(false);
        
    }


    public void DailyDataSheet()
    {

        for (int i = 0; i < recordListInstance[0].dailyPiece.Length; i++)
        {
            if (recordListInstance[i].day != 0)
            {
                dailyList[i].SetActive(true);
                dailyList[i].transform.GetChild(0).transform.GetComponent<Text>().text = recordListInstance[i].day.ToString() + ". GÜN";
                dailyList[i].transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().text = "Toplam : " + recordListInstance[i].dailyTotal.ToString();
                for (int j = 0; j < 10; j++)
                {
                    if (recordListInstance[i].nameArray[j] != "")
                    {
                        dailyList[i].transform.GetChild(2).transform.GetChild(j).gameObject.SetActive(true);
                        dailyList[i].transform.GetChild(3).transform.GetChild(j).gameObject.SetActive(true);
                        dailyList[i].transform.GetChild(2).transform.GetChild(j).transform.GetComponent<TextMeshProUGUI>().text = recordListInstance[i].nameArray[j].ToString();
                        dailyList[i].transform.GetChild(3).transform.GetChild(j).transform.GetComponent<TextMeshProUGUI>().text = recordListInstance[i].dailyPiece[j].ToString();

                    }
                }

            }

        }


    }
    public void ZReportBtn()
    {
        checkPanel.SetActive(true);

    }
    public void NoBtn()
    {
        checkPanel.SetActive(false);
    }

}
