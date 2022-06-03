

[System.Serializable]
public class DailyRecordController
{

    public JsonDailyRecord[] dailyRecords = new JsonDailyRecord[10];
}
[System.Serializable]
public class JsonDailyRecord
{
    public int day;
    public int dailyTotal;
    public int[] dailyPiece = new int[10];
    public string[] nameArray = new string[10];
}
