using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ViveAccidentScene : MonoBehaviour {

    public Image imgAccident;
    public Sprite[] spriteAccident;
    public Text textImgTitle;
    public Text textInfo;
    public Text textStep;

    IEnumerator Start()
    {
        int r = Random.Range(0, 10);
        imgAccident.sprite = spriteAccident[r];        
        GetString(r);

        yield return new WaitForSeconds(3.0f);
        if (PlayerPrefs.GetInt("CurrentScene") == 0)
        {
            SceneManager.LoadScene("ViveDig");
        }
        else if (PlayerPrefs.GetInt("CurrentScene") == 1)
        {
            //Driving.start_at_end = false;   //전진
            SceneManager.LoadScene("ViveDrive");
        }
        else if (PlayerPrefs.GetInt("CurrentScene") == 2)
        {
            //Driving.start_at_end = true;    //후진
            SceneManager.LoadScene("ViveDrive");
        }
        //else if (PlayerPrefs.GetInt("CurrentScene") == 3)
        //{
            //Driving.tutorial = true;
            //Driving.start_at_end = false;       //케터필러
            //SceneManager.LoadScene("drive_C");
        //}
    }


    void GetString(int num)
    {
        switch (num)
        {
            case 0:
                {
                    textImgTitle.text = "-골재포설 중 굴삭기 선회부에 협착";
                    textInfo.text = "아파트 건설공사 현장 내 임시주차장 조성을 위해 굴삭기로 골재 포설 작업 중 작업종료 지시를 위해 굴삭기 옆을 지나던 피재자가 회전하던 굴삭기 선회부(카운터웨이트부)와 창고 사이에 두부가 협착되어 사망한 재해.";
                    textStep.text = "차량계 건설기계(굴삭기) 작업시 운전중인 차량계 건설기계에 접촉되어 근로자에게 위험을 미칠 우려가 있는 장소에는 근로자의 출입을 통제하거나 유도자를 배치하여 차량계 건설기계를 안전하게 유도하여야함.";
                    break;
                }
            case 1:
                {
                    textImgTitle.text = "-자재운반 중 회전하는 굴삭기에 충돌";
                    textInfo.text = "피재자 등 2명이 하수관을 운반하여 굴삭기 근처 바닥에 내려놓고 이동하던 중 굴착한 토사를 상차하려고 회전한느 굴삭기의 좌측 후미에 피재자의 두부가 충돌되어 사망한 재해.";
                    textStep.text = "굴삭기 등 차량계 건설기계를 사용하여 작업하는 때에는 작업반경내 휀스를 설치하는 등 근로자의 출입을 통제하거나, 유도자를 배치하여 정해진 신호에 따라 작업토록 하여야 함.";
                    break;
                }
            case 2:
                {
                    textImgTitle.text = "-굴삭기로 폐목 투하작업 중 피재자를 가격";
                    textInfo.text = "굴착법면 상부에서 굴삭기를 사용하여 약 7m아래 폐목야적장으로 폐목. 투하작업을 진행하던 중, 폐목야적장을 근처로 이동 중이던 피재자를 가격하여 사망한 재해.";
                    textStep.text = "야적장에 굴삭기로 폐목 투하작업을 진행하는 때에는 작업구간에 휀스 또는 울을 설치하여 근로자의 출입을 통제하거나, 감시인을 배치하여 기타 근로자의 출입을 통제하여야 함.";
                    break;
                }
            case 3:
                {
                    textImgTitle.text = "-굴삭기의 붐대 선회 중 버켓이 탈락";
                    textInfo.text = "터파기 구간의 바닥면 잔토제거를 위해 굴삭기 붐대 단부를 브레이커에서 버켓으로 교체한 후 붐대를 선회하던 중 버켓이 붐대의 연결부(퀵커플러)에서 탈락되면서, 하부에서 바닥면 고르기 작업을 진행하고있던 피재자를 가격(사망)한 재해.";
                    textStep.text = "굴삭기를 사용하여 작업하는 때에는 사전에 붐대와 버켓의연결부(퀵커플러) 체결상태 등을 확인하고, 버켓의 탈락방지를 위해 안전핀을 설치하여야 하며, 작업반경 내에 기타 근로자의 접근을 통제하여야 함.";
                    break;
                }
            case 4:
                {
                    textImgTitle.text = "-청소 중 후진하는 굴삭기 바퀴에 협착";
                    textInfo.text = "관매설 및 되메우기 완료 후 피재자가 청소작업을 진행하던 중 인근에서 작업을 마치고 후진하는 굴삭기의 타이어에 협착되어 사망한 재해.";
                    textStep.text = "굴삭기 등 차량계 건설기계를 사용하여 작업하는 때에는 유도자를 배치하는 등 작업 및 이동시에 근로자와의 접촉방지조치를 하여야함.";
                    break;
                }
            case 5:
                {
                    textImgTitle.text = "-굴삭기 버켓으로 콘크리트 타설작업 중 버켓 탈락";
                    textInfo.text = "굴삭기 버켓에 콘크리트를 담아 맨홀 거푸집에 타설하던 중 버켓이 퀵커플러(버켓 부착장치)에서 이탈(낙하)하면서 하부에 있던 피재자를 가격하여 사망한 재해.";
                    textStep.text = "굴삭기 버켓을 부착장치에 체결하는 때에는 체결상태를 확인하고 안전핀을 체결하여야 하며, 백호우 등 차량계 건설기계의 주용도 외의 사용을 지양하고 작업반경 내에는 기타 근로자의 출입을 통제하여야 함.";
                    break;
                }
            case 6:
                {
                    textImgTitle.text = "-지하층 되메우기 작업 중 후진하던 굴삭기에 충돌";
                    textInfo.text = "굴삭기 및 덤프트럭을 사용하여 지하2층 되메우기 및 토사평탄작업을 진행하는 과정에서 후진하는 굴삭기의 운전원이 업무협의 중이던 피재자를 발견하지 못하고 진행하면서 충돌(사망)한 재해.";
                    textStep.text = "굴삭기 등 차량계 건설기계를 사용하는 작업을 진행하는 때에는 작업반경내에 경계휀스를 설치하여 출입을 통제하거나 유도자를 배치하고유도자의 일정신호에 따라 작업을 진행하여야함.";
                    break;
                }
            case 7:
                {
                    textImgTitle.text = "-굴착사면의 연약지반이 붕괴되면서 굴삭기 매몰";
                    textInfo.text = "굴착사면 위의 연약지반 이토가 슬라이딩 되면서 사면 하부에서 굴착작업 중이던 굴삭기 2대가 매몰되어 굴삭기 운전원 1명이 사망한재해.";
                    textStep.text = "우수 등으로 이토내 함수량이 증가됨에 따라 유동성이 커져 굴착사면의 슬라이딩으로 인한 붕괴재해의 위험이 커지므로, 굴착작업전에 균열의 유무, 함부 변화, 배수상태 등 이상유무를 사전점검하고, 이상이 있는 경우 흙막이 지보공(Sheet Pile 등), 보호막 등으로 보강조치를 하여야 함.";
                    break;
                }
            case 8:
                {
                    textImgTitle.text = "-토공작업 중 토사붕괴로 굴삭기 매몰";
                    textInfo.text = "굴삭기 운전원인 피재자가 굴착사면 하단에서 토사상차작업 및 배수로 정비작업을 진행하던 중 상단부 토사가 붕괴되면서 백호가 매몰되어 피재자가 사망한 재해.";
                    textStep.text = "굴착 사면은 안식각(보통흙 습지의 경우 1:1～1:1.5) 이내가 되도록 조정하여야 하며, 차수상태가 불량한 때에는 지반의 액상화가 진행되지 않도록 함수 및 용수 등을 검토하고 배수를 철저히 하여야 함.";
                    break;
                }
            case 9:
                {
                    textImgTitle.text = "-굴삭기 사용하여 H-형강 인양중 낙하";
                    textInfo.text = "교각 기초 흙막이 지보공 가시설(띠장 및 지보재)의 해체 및 양중 작업을 위하여 백호우를 이용하여 띠장재인 H-형강을 인양중 낙하하며 피재자를 타격하여 사망한 재해.";
                    textStep.text = "차량계 건설기계 사용작업시는 건설기계의 주 용도로만 사용하여야 하며, 양중작업은 크레인을 사용하여 작업 실시하고, 양중 작업시 대상물의 낙하, 회전 등으로 인한 위험 방지를 위하여 2점지지 실시와 결속 및 체결을 견고히 하고(Choker방식 등) 양중작업 실시.";
                    break;
                }
        }
    }

}
