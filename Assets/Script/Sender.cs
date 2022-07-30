using UnityEngine;
using System.Collections;

[System.Serializable]
public class Sender
{
    [System.Serializable]
    public class RECIEVE
    {
        public GameObject obj;
        public string method;
    }

    public RECIEVE[] recieves;

    public void SendMessage()
    {
        foreach (RECIEVE recieve in recieves)
        {
            recieve.obj.SendMessage(recieve.method);
        }
    }

    public void SendMessage(string method)
    {
        foreach (RECIEVE recieve in recieves)
        {
            recieve.obj.SendMessage(method);
        }
    }

    public void SendMessage(string method, object arg)
    {
        foreach (RECIEVE recieve in recieves)
        {
            recieve.obj.SendMessage(method, arg);
        }
    }
}
