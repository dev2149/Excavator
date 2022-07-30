using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogScene : MonoBehaviour {

    //저장 관련
    private SaveData data;
    private Users user = new Users();
    private List<string> lLog = new List<string>();
    private List<string> lContent = new List<string>();

    public GameObject Quest;
    //private UILabel _Title;
    //private UILabel _Content;

    public UILabel _Name;
    public UILabel _StartDate;
    public UILabel _LastDate;
    public UISprite _Img;

	// Use this for initialization
	void Start ()
    {

        //PlayerPrefs.SetString("UserID", data.GetValue<string>(user.ID));
        //데이터 로드
        data = SaveData.Load(Application.streamingAssetsPath + "\\" + PlayerPrefs.GetString("UserID") + ".xml");

        for (int i = 0; i < data.GetValue<List<string>>(user.Log).Count; i++)
        {
           
                Debug.Log(data.GetValue<List<string>>(user.Log)[i]);
                Debug.Log(data.GetValue<List<string>>(user.Content)[i]);
                //_Title.text = data.GetValue<List<string>>(user.Log)[i];
                //_Content.text = data.GetValue<List<string>>(user.Content)[i];
                //            Debug.Log(Quest.GetComponentsInChildren<UILabel>().Length);

                for (int j = 0; j < Quest.GetComponentsInChildren<UILabel>().Length; j++)
                {
                    Debug.Log(Quest.GetComponentsInChildren<UILabel>()[j].name);

                    if (Quest.GetComponentsInChildren<UILabel>()[j].name == "Title")
                    {
                        Quest.GetComponentsInChildren<UILabel>()[j].text = data.GetValue<List<string>>(user.Log)[i];
                    }
                    else if (Quest.GetComponentsInChildren<UILabel>()[j].name == "Label")
                    {
                        Quest.GetComponentsInChildren<UILabel>()[j].text = data.GetValue<List<string>>(user.Content)[i];
                        Quest.GetComponentsInChildren<UILabel>()[j].gameObject.SetActive(false);
                    }
                }

                NGUITools.SetActive(Quest, true);
                gameObject.GetComponent<UITable>().Reposition();
                NGUITools.AddChild(gameObject, Quest);
        }

        if (data.HasKey(user.Name))
            _Name.text = data.GetValue<string>(user.Name);

        if (data.HasKey(user.StartDATE))
            _StartDate.text = data.GetValue<string>(user.StartDATE);

        if (data.HasKey(user.LastDATE))
            _LastDate.text = data.GetValue<string>(user.LastDATE);
	}


    public void GoMenu()
    {
        Application.LoadLevel("menu");
    }
	
	// Update is called once per frame
	
}
