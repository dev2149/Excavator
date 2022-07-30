using System;
using UnityEngine;
using System.Diagnostics;

public class SimgData
{
    public string GetData()
    {
        string data = string.Empty;

        try
        {
            var proInfo = new ProcessStartInfo();
            proInfo.FileName = Application.streamingAssetsPath + "/SimgComPortData.exe";

            proInfo.CreateNoWindow = true;
            proInfo.UseShellExecute = false;

            proInfo.RedirectStandardOutput = true;
            proInfo.RedirectStandardError = true;

            var pro = new Process();
            pro.StartInfo = proInfo;
            pro.Start();

            data = pro.StandardOutput.ReadLine();
        }
        catch(Exception e)
        {
        }

        return data;
    }
}