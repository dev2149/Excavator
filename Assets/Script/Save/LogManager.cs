using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;

using System.IO;


using Excel;

using System.Data;

using ArabicSupport;

public class LogManager : MonoBehaviour {

    #region #Singleton Encapsulate
    private static LogManager _instance = null;
    public static LogManager Instance
    {
        get
        {
            return LogManager._instance;
        }
    }
    #endregion

    void Awake()
    {
        if (null == LogManager._instance)
        {
            LogManager._instance = this;
        }
        else
        {
            LogManager.Destroy(this.gameObject);
            return;
        }
    }

    private string fileName;
    private SaveData data;

    private List<string> lLog = new List<string>();
    private List<string> ltime = new List<string>();
    private string MySheetName = "Sheet_Test";

    public List<string> MyCellArray = new List<string>();
    public List<string> MyCellArray02 = new List<string>();

    public DriveMap mMap;
    public digMap mDigMap;


    void CreatExcel(List<string> str, List<string> time)
    {
        HSSFWorkbook MyWorkbook = new HSSFWorkbook();
        HSSFSheet Sheet01 = (HSSFSheet)MyWorkbook.CreateSheet(MySheetName);
        FileStream MyAddress = new FileStream(Application.streamingAssetsPath + "\\" + MySheetName + ".xls", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

        MyCellArray = new List<string>(str.Count);
        MyCellArray02 = new List<string>(str.Count);

       

        for (int i = 0; i < str.Count; i++)
        {
            HSSFRow Row = (HSSFRow)Sheet01.CreateRow((short)i);
            HSSFCell cell = (HSSFCell)Row.CreateCell((short)0);
            HSSFCell cell02 = (HSSFCell)Row.CreateCell((short)1);
        //    HSSFCell cell03 = (HSSFCell)Row.CreateCell((short)2);

            cell.SetCellValue(ltime[i]);
            cell02.SetCellValue(str[i]);
        //    Debug.Log(ltime[i]);
            Debug.Log(str[i]);

         

            //if (i < MyCellArray02.Count)
            //{

            //    HSSFCell cell02 = (HSSFCell)Row.CreateCell((short)1);

            //    cell02.SetCellValue(str[i]);
            //}
            //else
            //{

            //    HSSFCell cell02 = (HSSFCell)Row.CreateCell((short)1);

            //    cell02.SetCellValue("");
            //}
            //Row.RowStyle = MyWorkbook.CreateCellStyle();

            //Row.RowStyle.BorderBottom = BorderStyle.Double;

            //cell.CellStyle = MyWorkbook.CreateCellStyle();

            //cell.CellStyle.BorderRight = BorderStyle.Thin;
            //cell.CellStyle.BorderBottom = BorderStyle.Dashed;
            //cell.CellStyle.BottomBorderColor = HSSFColor.Red.Index;

            HSSFFont MyFont = (HSSFFont)MyWorkbook.CreateFont();

            MyFont.FontName = "Tahoma";
            MyFont.FontHeightInPoints = 16;
            MyFont.Color = HSSFColor.Black.Index;
            MyFont.Boldweight = (short)FontBoldWeight.Normal;

            cell.CellStyle.SetFont(MyFont);
            cell02.CellStyle.SetFont(MyFont);
        }

        MyWorkbook.Write(MyAddress);

        MyWorkbook.Close();
    }

    int IsMode()        // 0 :drive , 1: dig
    {
        if (Application.loadedLevelName == "drive" || Application.loadedLevelName == "ViveDrive") { return 0; }
        else if (Application.loadedLevelName == "dig" || Application.loadedLevelName == "ViveDig") { return 1; }
        else
            return 2;
    }

    void Start()
    {
//        Debug.Log(Application.loadedLevelName);
        if (IsMode() == 0)
            mMap = GameObject.Find("Map").GetComponent<DriveMap>();
        else if (IsMode() == 1)
            mDigMap = GameObject.Find("Map").GetComponent<digMap>();

        MySheetName = Application.loadedLevelName + "_" + System.DateTime.Now.ToString("yyyy년 MM월 dd일 HH시 mm분");
        lLog.Add(MySheetName);
        ltime.Add("");
        lLog.Add("------------------------시작------------------------");
        ltime.Add("");
    }


    public bool Exist(string str)
    {
        if (lLog.Exists(e => e.EndsWith(str)))
            return true;
        else
            return false;
       // foreach(string s in lLog)
       // {
       //     Debug.Log(s);
       //     if (s.Contains(str))
       //     {
       //         return true;
       //     }
       //     else
       //         return false;
       // }

       //// return false;
    }


    public void Save(string str)
    {
        if (lLog.Count > 2)
        {
            if (lLog[lLog.Count - 1].Equals(str))
                return;

            if (lLog[lLog.Count - 2].Equals(str))
                return;
        }

//        Debug.Log(mDigMap.laptime.text);
        if (IsMode() == 0)
        {
            if (mMap.laptime != null)
             ltime.Add(mMap.laptime.text);
            else
                ltime.Add("");
        }
        else if (IsMode() == 1)
        {
            if (ViveDigScene.instance.IsVR() == false)
            {
                //            Debug.Log(mDigMap);
                if (mDigMap.laptime != null)
                    ltime.Add(mDigMap.laptime.text);
                else
                    ltime.Add("");
            }
            else
            {
                if (ViveDigScene.instance.laptime != null)
                    ltime.Add(ViveDigScene.instance.laptime.text);
                else
                    ltime.Add("");
            }
        }

        lLog.Add(str);
    }


    public void Save()
    {
        if (!LogManager.Instance.Exist("------------------------종료------------------------"))
        {
            if (IsMode() == 0)
            {
                if (mMap.laptime != null)
                    ltime.Add(mMap.laptime.text);
                else
                    ltime.Add("");
            }
            else if (IsMode() == 1)
            {
                if (ViveDigScene.instance.IsVR() == false)
                {
                    if (mDigMap.laptime != null)
                        ltime.Add(mDigMap.laptime.text);
                    else
                        ltime.Add("");
                }
                else
                {
                    if (ViveDigScene.instance.laptime != null)
                        ltime.Add(ViveDigScene.instance.laptime.text);
                    else
                        ltime.Add("");
                }
            }

            lLog.Add("------------------------종료------------------------");
        }
      //  data[Application.loadedLevelName] = lLog;
        //data.Save();
        CreatExcel(lLog, ltime);
    }


    void OnApplicationQuit()
    {
        if (!LogManager.Instance.Exist("------------------------종료------------------------"))
        {
            if (IsMode() == 0)
                ltime.Add(mMap.laptime.text);
            else if (IsMode() == 1)
            {
                if (ViveDigScene.instance.IsVR() == false)
                {
                    ltime.Add(mDigMap.laptime.text);
                }
                else
                {
                    ltime.Add(ViveDigScene.instance.laptime.text);
                }
            }
            lLog.Add("------------------------종료------------------------");
        }
    //    data[Application.loadedLevelName] = lLog;
       // data.Save();
        CreatExcel(lLog, ltime);
    }


    string NowTime()
    {
        return System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
    }
}
