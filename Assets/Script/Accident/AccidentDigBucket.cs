using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AccidentDigBucket : MonoBehaviour
{
    private readonly float DROP_ANIMATETIME = (10.0f / 3.0f);
    public static bool shoveling = false;
    public float LimitLine;
    public static bool droped = false;
    public bool stall = false;
    public bool forespeed;
    private bool bDig = false;

    bool isDigFail = false;
    float time;

    public ParticleSystem particeSoil;
    public MegaPointCache soil;
    public MegaCrumple soilcrumple;
    public Transform sourcePosition;
    public Vector3 shovelingRange = new Vector3(55.0f, 120.0f, 5.0f);
    public Vector3 pouringRange = new Vector3(80.0f, 120.0f, 0.0f);
    public Transform point;
    public MegaMorph digging;
    public MegaMorph drop;
    public GameObject reciever;
    public GameObject target;
    public GameObject[] delete;
    public GameObject stone_parent;

    private Transform _transform;
    private Vector3 distance;
    private float diggingTime = 0.0f;
    private float pouringValue = 0.0f;
    private bool dropCheck = false;
    private Vector3 dropPosition;
    private Vector2 dropRange = Vector2.zero;
    private float dropValue = 0.0f;
    private bool forepush = false;

    public AccidentDiging diging;
    public DigingShake digingShake;
    public DigingWarning diesel;
    public DigingWarning shovelingSound;
    public UIDigRightControl control_R;
    public UIDigLeftControl control_L;
    public int int_foul_overload;
    public UILabel DIGstage;
    public Text DIGstageVR;

    private float bucketangle;
    private float foreangle;
    private float dist;

    private GameObject finish;

    private float Angle
    {
        get
        {
            return Vector3.Angle(Vector3.up, _transform.forward);
        }
    }

    private GameObject stone;
    public GameObject cube;
    public GameObject cube_;
    float CUBE_DELAY = 0.3f;
    float cubeDelay;

    float shovelingForeX;
    public int score = 0;
    private bool b_angle;

    public bool isBucketClose;

    bool isDrop;

    bool isOverload;
    
    void Start()
    {
        cubeDelay = CUBE_DELAY;

        shoveling = false;

        _transform = this.transform;

        finish = GameObject.FindWithTag("DIG_FINISH");
    }

    void Update()
    {
        if (isDigFail)
        {
            time += Time.deltaTime;

            if (time > 1f)
            {
                time = 0;
                isDigFail = false;
            }
        }
        delete = GameObject.FindGameObjectsWithTag("Stone");

        UpdateSoil();
        UpdateDropArea();

        //파티클 멈춤 수정
        if (particeSoil.isPlaying)
        {
            if (gameObject.GetComponentInChildren<MegaPointCache>().time > 0.4f)
            {
                particeSoil.Stop();
            }
        }

        if (shoveling)
        {
            diesel.On();
        }
        else
        {
            UpdateSpeed(); //서서히 올라가게 
        }

        // 과부하 경고 메시지 16.02.18

        RaycastHit hit;
        Vector3 down = transform.TransformDirection(Vector3.down) * 10;     //월드 좌표계로 변환
        if (Physics.Raycast(point.position, down, out hit)) // 50%이상 잠기면 
        {
            dist = Vector3.Distance(point.position, hit.collider.transform.position);
            if (dist > 6.0f && dist < 13.0f) //50% 속도저하
            {
                if (!forepush) //암밀기제한 //digging일때만
                {
                    forespeed = true;
                }
            }
            else
            {
                forespeed = false;
            }

            //바닥 끝까지 버켓이 만난경우.
            if (b_angle)
            {
                if ((int)bucketangle != (int)diging.bucket.angle &&
                    (int)foreangle != (int)diging.fore.angle) // 90%이상 잠겨서 암을 움직였다. // || -> && 로 변경
                {
                    diging.bucket.speed = 25.0f;
                    diging.fore.speed = 5.0f;
                    diging.upper.speed = 5.0f;

                    //과부하가 처음인지 확인 
                    if (isOverload == false)
                    {
                        isOverload = true;

                        reciever.SendMessage("OnFail", 8);   // 과부하 전달.
                    }
                }
                else
                {
                    diging.bucket.speed = 45.0f;
                    diging.fore.speed = 10.0f;
                    diging.upper.speed = 10.0f;
                }
            }
            else
            {
                isOverload = false;
            }
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (other.tag == "DIG_DIGGING")
        {
            if (Angle < 100.0f)
            {
                isBucketClose = true;
            }
            else
            {
                isBucketClose = false;

            }
        }
    }

    bool isBucketOn;

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "DIG_BucketOn")
        {
            isBucketOn = true;
        }

        if (other.tag == "DIG_DIGGING")
        {
            forepush = false;
            if (100.0f < Angle)
            {
                diging.bucket.speed = 45.0f;
                diging.fore.speed = 10.0f;
                diging.upper.speed = 10.0f;
                diesel.On();
                
                shoveling = true;
                diggingTime = digging.animtime;
                shovelingForeX = diging.fore.angle;
            }
            else if (Angle < 100.0f)
            {
                diging.bucket.speed = 45.0f;
                diging.fore.speed = 10.0f;
                diging.upper.speed = 0.2f;
                diesel.On();
                shoveling = false;
                _Play();
            }
        }
    }

    void UpdateSpeed()
    {
        float speed = 70.1f + ((1 - soil.time) * 15);

        //흙이 차있는 양에 비례해서 버킷 열리는 속도 증가
        if (GetJoysticks.Instance.GetJoystickNumber(JoystickType.Left) >= 0 && Input.GetAxisRaw(GetJoysticks.Instance.GetJoystickNumber(JoystickType.Left) + "_Joystick_Horizontal") < -0.3f)
        {
            if (diging.bucket.speed < speed)
            {
                diging.bucket.speed += 1.5f;
            }
            else
            {
                diging.bucket.speed = 70.1f;
            }
        }
        else
        {
            if (diging.bucket.speed < speed)
            {
                diging.bucket.speed += 1.5f;
            }
            else
            {
                diging.bucket.speed = 70.1f;
            }
        }
        
        if (diging.fore.speed < 50.1f)
        {
            diging.fore.speed += 1.5f;
        }

        if (diging.upper.speed < 50.1f)
        {
            diging.upper.speed += 1.5f;
        }
    }

    void UpdateSoil()
    {
        if (shoveling)
        {
            digingShake.vibrateRange = 0.03f; // 땅속에서 과중

            if (control_R.play)
            {
                shovelingSound.On();
            }


            if (control_L.play)
            {
                shovelingSound.On();
            }

            if (soil.time <= 0.4f)
            {
                soilcrumple.scale = 1.2f;

                cubeDelay -= Time.deltaTime;

                if (cubeDelay <= -0.5f)
                {
                    Vector3 Base_ = stone_parent.transform.position; //기본 생성위치
                    stone = Instantiate(cube_, new Vector3(Base_.x + Random.Range(-3, 3), Base_.y + (2 - soil.time), Base_.z), Quaternion.identity) as GameObject; // 랜덤하게 생성되는 위치.
                    cubeDelay = CUBE_DELAY;
                    stone.transform.parent = stone_parent.transform; //부모안에 생성
                }
            }

            float foreDistance = 0.1f * (diging.fore.angle - shovelingForeX);
            float invRange = 1.2f / (shovelingRange.y - shovelingRange.x);
            float angleAmount = 1.0f - Mathf.Clamp01((Angle - shovelingRange.x) * invRange);

            distance = _transform.position - sourcePosition.position;
            float distanceAmount = 1.0f - Mathf.Clamp01((distance.y - shovelingRange.z) * (1.0f / shovelingRange.z));

            if (foreDistance > 0.3f)
            {
                soil.time = Mathf.Min(soil.time, 1.0f - (angleAmount * foreDistance)); //암이안움직이면 흙이 안퍼짐.. 그러면 안되는ㄷㅇ..  
            }

            soil.time = Mathf.Min(soil.time, 1.0f - (distanceAmount * angleAmount));
            bDig = true;
            if (digging.animtime < 3.2f)
            {
                digging.SetAnimTime(diggingTime + (1.0f - soil.time) * 0.08f * DROP_ANIMATETIME);
            }
        }
        else if (1.0f > soil.time)
        {
            float invRange = 1.0f / (pouringRange.y - pouringRange.x); //0.025
            float angleAmount = Mathf.Clamp01((Angle - pouringRange.x) * invRange); //항상 0.0f

            float amount = angleAmount * Time.deltaTime;

            if ((1.0f > soil.time) && (0.0f < amount))
            {
                if (false == particeSoil.isPlaying)
                {
                    particeSoil.Play();
                }

                soilcrumple.scale -= 0.0069f;

                cubeDelay -= Time.deltaTime;

                if (cubeDelay <= 0)
                {
                    Instantiate(cube, new Vector3(Random.Range(0.0f, 6.0f), particeSoil.transform.position.y, particeSoil.transform.position.z), Quaternion.identity); // 랜덤하게 생성되는 위치.
                    cubeDelay = CUBE_DELAY;
                }

                for (int i = 0; i < delete.Length; i++)
                    delete[i].GetComponent<Rigidbody>().useGravity = true;
            }
            else
            {
                if (true == particeSoil.isPlaying)
                {
                    isDrop = false;
                    particeSoil.Stop();
                }
            }

            soil.time += amount;

            if (0.0f >= amount)
                return;
            
            RaycastHit hit;
            if (Physics.Raycast(point.position, -Vector3.up, out hit))
            {
                Debug.Log(hit.collider.tag);
                shovelingSound.On();

                if (hit.collider.tag == "DIG_DROP")
                {
                    shovelingSound.On();

                    if (1.0f > soil.time)
                    {
                        pouringValue += amount;

                        //버킷의 높이에 따라 흙 차오르는 에니메이션 딜레이
                        StartCoroutine(DropAnim(amount, transform.position.y * 0.03f));
                    }
                    if (bDig)
                    {
                        bDig = false;
                        if (soil.time > 0.2f)
                        {
                            reciever.SendMessage("OnFail", 16);
                        }
                        GameEvent.notify(GameEvent.TAG.DIG_POUR);
                    }
                }

                if (hit.collider.tag == "DIG_AREA_1")
                {
                    if (0.1f <= soil.time)
                    {
                        if (isDrop == false)
                        {
                            reciever.SendMessage("OnFail", 10);
                        }

                        if (particeSoil.isPlaying)
                        {
                            isDrop = true;
                        }
                    }

                }
            }
        }
        else
        {
            if (particeSoil.isPlaying)
            {
                Invoke("DeleteStone", 3.0f);
                isDrop = false;
                particeSoil.Stop();

                if ((int)soilcrumple.scale <= 0.0f)
                    soilcrumple.scale = 0.0f;
            }
            digingShake.vibrateRange = 0.015f; // 땅속에서 과중
        }

        soil.time = Mathf.Clamp01(soil.time);
    }

    void UpdateDropArea()
    {
        if (false == dropCheck)
            return;

        Vector3 v1 = new Vector3(dropPosition.x, 0.0f, dropPosition.z);
        Vector3 v2 = new Vector3(_transform.position.x, 0.0f, _transform.position.z);
        Vector3 v3 = v2 - v1;
        dropValue = Mathf.Max(v3.magnitude * (1.0f / 15.0f), dropValue);

        drop.SetAnimTime(Mathf.Lerp(dropRange.x, dropRange.y, 1.3f - dropValue));  //자연스럽게 땅이꺼지는 정도 2초 평탄한 땅애니메이션. //1.0f - dropValue	
    }

    void _Play()
    {
        reciever.SendMessage("OnFail", 18);
        float time = 0.5f;
        float amount = 2.0f;
        iTween.ShakeRotation(target, new Vector3(amount, amount, amount), time);
    }

    void DeleteStone()
    {
        for (int i = 0; i < delete.Length; i++)
            Destroy(delete[i]);
    }

    IEnumerator DropAnim(float amount, float dis)
    {
        yield return new WaitForSeconds(dis);
        drop.SetAnimTime(drop.animtime + amount * 0.25f * (10.0f / 3.0f)); //흙이 차오르는 애니메이션 //0.25
    }

}
