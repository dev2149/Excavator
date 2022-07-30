using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Logindata : MonoBehaviour
{
    private string fileName;
    private SaveData data;

    public Transform _Login;
    public Transform _Register;

    public UIInput ID;
    public UIInput PW;
    public UIInput rePassWord;
    public UIInput Name;
    public UIInput Email;

    //
    private Users user = new Users();
    private List<string> lLog = new List<string>();
    private List<string> lContent = new List<string>();
    // private Dictionary<string, List<string>> _Log = new Dictionary<string, List<string>>();

    private int TapCount = 0;

    void Update()
    {
        //tap 키 커서 변환
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //    if (!ID.isSelected)
            if (_Login.gameObject.activeInHierarchy)
            {
                ID = _Login.transform.Find(user.ID).GetComponent<UIInput>();
                PW = _Login.transform.Find(user.PW).GetComponent<UIInput>();

                if (ID.isSelected)
                {
                    ID.selected = false;
                    PW.selected = true;
                }
                else
                {
                    ID.selected = true;
                    PW.selected = false;
                }
            }

            if (_Register.gameObject.activeInHierarchy)
            {
                //ID = _Register.transform.FindChild(user.ID).GetComponent<UIInput>();
                //PW = _Register.transform.FindChild(user.PW).GetComponent<UIInput>();
                //rePassWord = _Register.transform.FindChild(user.REPASS).GetComponent<UIInput>();
                //Name = _Register.transform.FindChild(user.Name).GetComponent<UIInput>();
                //Email = _Register.transform.FindChild(user.Email).GetComponent<UIInput>();

                if (TapCount == 0)
                {
                    ID.selected = true;
                    PW.selected = false;
                    rePassWord.selected = false;
                    Name.selected = false;
                    Email.selected = false;
                    TapCount++;
                }
                else if (TapCount == 1)
                {
                    ID.selected = false;
                    PW.selected = false;
                    rePassWord.selected = false;
                    Name.selected = true;
                    Email.selected = false;
                    TapCount++;
                }
                else if (TapCount == 2)
                {
                    ID.selected = false;
                    PW.selected = true;
                    rePassWord.selected = false;
                    Name.selected = false;
                    Email.selected = false;
                    TapCount++;
                }
                else if (TapCount == 3)
                {
                    ID.selected = false;
                    PW.selected = false;
                    rePassWord.selected = true;
                    Name.selected = false;
                    Email.selected = false;
                    TapCount++;
                }
                else if (TapCount == 4)
                {
                    ID.selected = false;
                    PW.selected = false;
                    rePassWord.selected = false;
                    Name.selected = false;
                    Email.selected = true;
                    TapCount = 0;
                }
            }

        }
        //  Debug.Log(ID.selected);
    }

    // Use this for initialization
    void Start()
    {
        //  ID.selected = true;
        //    PW.selected = true;
        //data = SaveData.Load(Application.streamingAssetsPath + "\\" + fileName + ".uml");
        //data = new SaveData(fileName);
    }

    //아이디 등록
    public void RegisterOK2()
    {
        Debug.Log("생성");
    }

    //아이디 생성
    public void RegisterOK()//string id, string pw)
    {
        bool isMistake = false;

        //생성하기전 입력값 체크. 
        if (ID.value == "" || ID.value == null)
        {
            ID.value = "아이디를 입력 해주세요!";

            isMistake = true;
        }

        if (PW.value == "" || PW.value == null)
        {
            PW.value = "비밀번호를 입력 해주세요!";

            isMistake = true;
        }
        else if(!PW.value.Equals(rePassWord.value))
        {
            PW.value = "비밀번호가 일치하지 않습니다!";

            isMistake = true;
        }

        if (Name.value == "" || Name.value == null )
        {
            Name.value = "이름을 입력해주세요!";

            isMistake = true;
        }

        if (Email.value == "" || Email.value == null)
        {
            Email.value = "Email을 입력해주세요!";

            isMistake = true;
        }

        //모든 입력창 
        if (!isMistake)
        {
            data = new SaveData(ID.value);

            data[user.ID] = ID.value;
            data[user.PW] = PW.value;
            data[user.Name] = Name.value;
            data[user.Email] = Email.value;

            data[user.Log] = lLog;
            data[user.Content] = lContent;

            data.Save();

            _Register.gameObject.SetActive(false);
            _Login.gameObject.SetActive(true);
        }
    }

    //로그인
    public void Login()
    {
        ID = _Login.transform.Find(user.ID).GetComponent<UIInput>();
        PW = _Login.transform.Find(user.PW).GetComponent<UIInput>();

        if (ID.isSelected)
        {
            ID.selected = false;
            PW.selected = true;
        }
        else
        {
            ID.selected = true;
            PW.selected = false;
        }

        //  ID.selected = true;
        PW.selected = true;

        if (SaveData.IsUser(Application.streamingAssetsPath + "\\" + ID.value + ".xml"))
        {
            data = SaveData.Load(Application.streamingAssetsPath + "\\" + ID.value + ".xml");

            if (data.HasKey(user.PW))
            {
                if (data.GetValue<string>(user.PW) == PW.value)
                {
                    //유저 아이디 저장
                    PlayerPrefs.SetString("UserID", data.GetValue<string>(user.ID));

                    Application.LoadLevel("warning");
                }
                else
                {
                    PW.value = "비빌번호가 틀렸습니다!";
                }
            }
            else
            {
                PW.value = "비빌번호가 틀렸습니다!";
            }
        }
        else
        {
            ID.value = "아이디를 찾을수 없습니다!";
        }
    }
}
