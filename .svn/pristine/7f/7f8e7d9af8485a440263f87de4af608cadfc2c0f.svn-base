
using UnityEngine;
using System.Collections;
using System.IO;

public class safequiz : MonoBehaviour {

    //문제의 갯수.
    [SerializeField]
    int quizNum;

    //문제 UILabel.
    public UILabel label_question;
    //문제 보기 UI.
    public UILabel[] label_quizViews;
    //상태 UI.
    public UITexture UI;

    //정답일 경우 UI.
    public GameObject goodUI;
    //틀릴경우 UI.
    public GameObject badUI;
    //통과 UI.
    public GameObject passUI;
    //실격 UI.
    public GameObject failUI;

    //결과U.
    public ResultManager result;

    //삽화 UI.
    Texture[] quizUIs;

    private string[] values;                // 줄단위로 나눔	
    public string[] examples;               // 줄안에서 문제를 나눔.
    private int random;

    private int rotate;
    private int success;
    private int fail;

    //문제중복 방지 배열.
    private int[] completionNum;

    float time;
    bool next;

    string[] resultText = new string[3];

    int resultCount;

    void Start()
    {
        //퀴즈 정보 불러오기.
        LodeQuizData();

        //퀴즈의 갯수만큼 숫자 생성.
        completionNum = new int[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
            completionNum[i] = i + 1;
        }

        //퀴즈 시작.
        SetQuizData();
    }

    void LodeQuizData()
    {
        //리소스 불러오기.
        TextAsset data = Resources.Load("Test", typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(data.text);

        string source = sr.ReadToEnd();

        while (source != null)
        {
            values = source.Split('#');

            if (values.Length == 0)
            {
                sr.Close();
                return;
            }

            source = sr.ReadLine();
        }

        quizUIs = new Texture[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
            quizUIs[i] = Resources.Load("Image/0" + (i + 1), typeof(Texture)) as Texture;
        }

    }

    //보기중에 선택했을 경우 호출됨.
    public void answer(string an)
    {
        if (rotate > quizNum) { return; }

        if (next) { return; }

        //정답 불러오기.
        string _examples = examples[6];

        for (int i = 1; i < 6; i++)
        {
            if (_examples.Trim() == (i - 1).ToString())
            {
                resultText[resultCount] += "[FFF000]" + examples[i] + " [-] \n";
            }
            else
            {
                resultText[resultCount] += examples[i] + "\n";
            }
        }

        resultText[resultCount] += "\n\n";

        resultCount++;

        //정답과 비교후 결과 확인.
        if (an == _examples.Trim())
        {
            result.Check(true);
            goodUI.SetActive(true);
            success++;
        }
        else
        {
            result.Check(false);
            badUI.SetActive(true);
            fail++;
        }

        
        //다음 문제 시작.
        next = true;
    }

    void Update()
    {
        if (next)
        {
            time += Time.deltaTime;

            if (time > 1f)
            {
                time = 0;

                next = false;

                goodUI.SetActive(false);
                badUI.SetActive(false);
                SetQuizData();
            }
        }
    }

    void SetQuizData()
    {
        //랜덤 숫자를 random함수에 입력함.
        RandomNumber();

        UI.mainTexture = quizUIs[random];

        examples = values[random].Split('\n');

        label_question.text = examples[1];

        for (int i = 0; i < label_quizViews.Length; i++)
        {
            label_quizViews[i].text = examples[i + 2];
        }

        rotate++;

        if (rotate > quizNum)
        {
            QuizResult();
        }
    }

    void RandomNumber()
    {
        bool isloof = true;

        int a;

        while (isloof)
        {
            a = Random.Range(0, values.Length - 1);

            for (int i = 0; i < completionNum.Length; i++)
            {
                if (a == completionNum[i])
                {
                    random = a;

                    completionNum[i] = -1;

                    isloof = false;
                }
            }
        }
    }

    void QuizResult()
    {
        goodUI.SetActive(false);
        badUI.SetActive(false);
        UI.gameObject.SetActive(false);

        result.gameObject.SetActive(true);

        result.OnResult(resultText);
        ///result.text = resultText;

        label_question.text = "";

        for (int i = 0; i < label_quizViews.Length; i++)
        {
            label_quizViews[i].text = "";
        }

        if (success >= 3)
        {
            passUI.SetActive(true);
        }
        else
        {
            failUI.SetActive(true);
        }
    }


    public void GoMenu()
    {
        Application.LoadLevel("menu");
    }


    public void GoQuiz()
    {
        Application.LoadLevel("Quiz");
    }
}
