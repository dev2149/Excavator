using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class AccidentDiging : MonoBehaviour
{
    public static bool isRpmGood;
    public bool isRpmSetting;
    public AccidentDigBucket DigingBucket;

    public Transform RightJoystick;
    public Transform leftJoystick;
    public DigingWarning leversound;        //레버 사운드음
    public DigingWarning caution;           // 경고
    public UILabel preMessage;

    //UI 조이스틱 관련
    private Color color_lever = new Color(0.639f, 0.01f, 0.01f);

    private UIDigLeftControl mUIDigLeftCtl;
    private UIDigRightControl mUIDigRightCtl;

    public DigingShake digingShake;
    public DigingWarning digingWarning;

    public Transform RPM_IV;                //알피엠 게이지
    private int RPM_State; // 알피엠 상태

    //시간

    public class JOYSTICKPOS
    {
        public float left_H;
        public float left_V;
        public float Rihgt_H;
        public float Right_V;
    }

    private JOYSTICKPOS ShakePos;

    [System.Serializable]
    public class AXIS
    {
        public Transform node;
        public float speed;
        public float min;
        public float max;

        [HideInInspector]
        public float angle;
        public float angleR;
        public float angleL;
    }
    public AXIS body;
    public AXIS upper;
    public AXIS fore;
    public AXIS bucket;

    public bool keyboard = true;
    bool isDigingShake;
    float digingShakeTime = 0;

    public AudioSource diesel;
    private float correction;

    public AudioClip shakesound;
    private float timeSpan = 0.0f;
    int joy1;
    int joy2;

    void Start()
    {
        ShakePos = new JOYSTICKPOS();

        correction = 0.5f;
        
        //굴삭기 초기 위치값 변경
        body.angle = 0.0f;
        upper.angle = -7.0f;
        fore.angle = -30.0f;
        bucket.angle = -90f;
        RPM_State = 0;

        mUIDigLeftCtl = leftJoystick.GetComponent<UIDigLeftControl>();
        mUIDigRightCtl = RightJoystick.GetComponent<UIDigRightControl>();
    }


    public void Rpm(int value)
    {
        body.speed = body.speed + 0.5f * value;
        upper.speed = upper.speed + 0.5f * value;
        fore.speed = fore.speed + 0.5f * value;
        bucket.speed = bucket.speed + 0.5f * value;
    }

    void Update()
    {
        // 평탄일때는 또 어떻게 해야할지.
        //조이스틱 연결
        joy1 = GetJoysticks.Instance.GetJoystickNumber(JoystickType.Left);
        joy2 = GetJoysticks.Instance.GetJoystickNumber(JoystickType.Right);
        if (GetJoysticks.Instance.GetJoystickNumber(JoystickType.Right) != -100 && GetJoysticks.Instance.GetJoystickNumber(JoystickType.Left) != -100)
        {
            // 3rd 왼쪽 -> 좌/우 body
            if (Input.GetAxisRaw(joy1 + "_Joystick_Horizontal") > 0.3f || Input.GetAxisRaw(joy1 + "_Joystick_Horizontal") < -0.3f)
            {
                if (AccidentDigBucket.shoveling)
                {
                    body.angle += body.speed * Time.deltaTime * Input.GetAxisRaw(joy1 + "_Joystick_Horizontal") * 0.01f;

                    leftJoystick.GetComponent<UIDigLeftControl>().widget.localPosition += new Vector3(body.speed * Time.deltaTime * Input.GetAxisRaw(joy1 + "_Joystick_Horizontal") * 0.2f, 0, 0);

                    if (Input.GetAxisRaw(joy1 + "_Joystick_Horizontal") > 0.6f || Input.GetAxisRaw(joy1 + "_Joystick_Horizontal") < -0.6f)
                    {
                        caution.On();

                        preMessage.text = "버킷이 땅속에 있습니다. 케빈을 좌/우로 움직이는것은 위험합니다.";
                    }

                    leversound.On();
                    RPM_State = 2;
                }
                else
                {
                    if (timeSpan < 45F)  // 천천히 속도 붙게..
                    {
                        timeSpan += Time.deltaTime * 8F;
                    }
                    else
                    {
                        timeSpan = 50F;
                    }

                    body.angle += body.speed * Time.deltaTime * Input.GetAxisRaw(joy1 + "_Joystick_Horizontal") * correction * timeSpan;
                    leftJoystick.GetComponent<UIDigLeftControl>().widget.localPosition += new Vector3(body.speed * Input.GetAxisRaw(joy1 + "_Joystick_Horizontal") * 1.8f * 25, 0, 0);
                    leversound.On();
                    RPM_State = 1;
                }

            }
            else
            {
                if (timeSpan > 10F)
                {
                    timeSpan = 0F;
                    NGUITools.PlaySound(shakesound);
                }
            }

            //4th 왼쪽 -> 위/아래 fore  
            if (Input.GetAxisRaw(joy1 + "_Joystick_Vertical") > 0.3f || Input.GetAxisRaw(joy1 + "_Joystick_Vertical") < -0.3f)
            {
                fore.angle += fore.speed * Time.deltaTime * Input.GetAxisRaw(joy1 + "_Joystick_Vertical") * correction;

                float speed = fore.speed;

                if (AccidentDigBucket.shoveling)
                {
                    RPM_State = 2;

                    if (DigingBucket.forespeed)
                    {
                        speed = 0.0f;
                    }
                    else
                    {
                        speed = 40.0f;
                    }
                }

                leftJoystick.GetComponent<UIDigLeftControl>().widget.localPosition += new Vector3(0, -speed * Input.GetAxisRaw(joy1 + "_Joystick_Vertical") * 1.6f, 0);
                leversound.On();
            }

        }

        //5th 오른쪽 -> 좌/우 bucket
        if (Input.GetAxisRaw(joy2 + "_Joystick_Horizontal") > 0.3f || Input.GetAxisRaw(joy2 + "_Joystick_Horizontal") < -0.3f)
        {
            bucket.angle += bucket.speed * Time.deltaTime * -Input.GetAxisRaw(joy2 + "_Joystick_Horizontal") * correction;
            RightJoystick.GetComponent<UIDigRightControl>().widget.localPosition += new Vector3(-bucket.speed * Input.GetAxisRaw(joy2 + "_Joystick_Horizontal") * 1.6f, 0, 0);
            leversound.On();
        }


        //6th 오른쪽 -> 위/아래 upper
        if (Input.GetAxisRaw(joy2 + "_Joystick_Vertical") > 0.3f || Input.GetAxisRaw(joy2 + "_Joystick_Vertical") < -0.3f)
        {
            upper.angle += upper.speed * Time.deltaTime * -Input.GetAxisRaw(joy2 + "_Joystick_Vertical") * correction;

            RightJoystick.GetComponent<UIDigRightControl>().widget.localPosition += new Vector3(0, upper.speed * Input.GetAxisRaw(joy2 + "_Joystick_Vertical") * 1.6f, 0);
            leversound.On();
        }

        //UI 조이스틱 색 변화
        UpdateRightWidgetState();
        UpdateLeftWidgetState();

        upper.angle = Mathf.Clamp(upper.angle, upper.min, upper.max);
        fore.angle = Mathf.Clamp(fore.angle, fore.min, fore.max);
        bucket.angle = Mathf.Clamp(bucket.angle, bucket.min, bucket.max);

        body.node.localRotation = Quaternion.Euler(0.0f, 0.0f, body.angle);
        upper.node.localRotation = Quaternion.Euler(upper.angle, 0.0f, 0.0f);
        fore.node.localRotation = Quaternion.Euler(fore.angle, 0.0f, 0.0f);
        bucket.node.localRotation = Quaternion.Euler(bucket.angle, 0.0f, 0.0f);

        if (Option.Instance.Sound)
        {
            diesel.volume = Mathf.Lerp(0.0f, 0.1f, 120f);
            diesel.GetComponent<AudioSource>().loop = true;
        }
        else
        {
            diesel.GetComponent<AudioSource>().volume = 0;
        }

        if (Input.anyKey == false)
        {
            RPM_State = 0;
        }
    }

    void uppermax(int socre)
    {
        switch (socre)
        {
            case 0:
                break;
            case 1: //한번
                upper.max = 13;
                break;
            case 2: //두번
                upper.max = 14;
                break;
            case 3: //세번
                upper.max = 15;
                break;
            case 4: //네번째 평탄할때.
                break;
        }
    }

    void FixedUpdate()
    {
        if (isDigingShake == true)
        {
            digingShakeTime += Time.deltaTime;

            if (digingShakeTime > 1f)
            {
                digingShakeTime = 0;
                isDigingShake = false;
            }
        }
        if (false == AccidentDigBucket.shoveling)
        {
            if (GetJoysticks.Instance.GetJoystickNumber(JoystickType.Right) != -100 && GetJoysticks.Instance.GetJoystickNumber(JoystickType.Left) != -100)
            {
                if (Input.GetAxisRaw(joy1 + "_Joystick_Horizontal") != 0 && Input.GetAxisRaw(joy2 + "_Joystick_Horizontal") != 0)
                {
                    //흔들림 감지
                    if (0.25f <= Mathf.Abs(ShakePos.left_H - Input.GetAxisRaw(joy1 + "_Joystick_Horizontal")) ||
                        0.25f <= Mathf.Abs(ShakePos.left_V - Input.GetAxisRaw(joy1 + "_Joystick_Vertical")) ||
                        0.25f <= Mathf.Abs(ShakePos.Rihgt_H - Input.GetAxisRaw(joy2 + "_Joystick_Horizontal")) ||
                        0.25f <= Mathf.Abs(ShakePos.Right_V - Input.GetAxisRaw(joy2 + "_Joystick_Vertical")))
                    {
                        if (isDigingShake == false)
                        {
                            isDigingShake = true;
                            digingShake.Run();
                        }
                    }
                }
            }
        }
        if (GetJoysticks.Instance.GetJoystickNumber(JoystickType.Right) != -100 && GetJoysticks.Instance.GetJoystickNumber(JoystickType.Left) != -100)
        {
            ShakePos.left_H = Input.GetAxisRaw(joy1 + "_Joystick_Horizontal");
            ShakePos.left_V = Input.GetAxisRaw(joy1 + "_Joystick_Vertical");
            ShakePos.Rihgt_H = Input.GetAxisRaw(joy2 + "_Joystick_Horizontal");
            ShakePos.Right_V = Input.GetAxisRaw(joy2 + "_Joystick_Vertical");
        }
        RPM_State = 1;

        //알피엠 상태
        if (RPM_State == 0)      //기본
        {
            RPM_IV.localRotation = Quaternion.Euler(52.85935f, Random.Range(-140.0f, -145.0f), 121.458f);
        }
        else if (RPM_State == 1)    //굴삭시
        {
            RPM_IV.localRotation = Quaternion.Euler(52.85935f, Random.Range(-160, -165.0f), 121.458f);
        }
        else if (RPM_State == 2)    //과부하
        {
            RPM_IV.localRotation = Quaternion.Euler(52.85935f, Random.Range(-180.0f, -185.0f), 121.458f);
        }

    }

    void UpdateRightWidgetState()
    {
        if (mUIDigRightCtl.widget.localPosition.y > 30)
        {
            mUIDigRightCtl.Down.GetComponent<TweenColor>().from = color_lever;
            mUIDigRightCtl.Up.GetComponent<TweenColor>().from = Color.white;
            mUIDigRightCtl.Bucket_pull.GetComponent<TweenColor>().from = Color.white;
            mUIDigRightCtl.Bucket_push.GetComponent<TweenColor>().from = Color.white;
        }
        else if (mUIDigRightCtl.widget.localPosition.y < -30)
        {
            mUIDigRightCtl.Down.GetComponent<TweenColor>().from = Color.white;
            mUIDigRightCtl.Up.GetComponent<TweenColor>().from = color_lever;
            mUIDigRightCtl.Bucket_pull.GetComponent<TweenColor>().from = Color.white;
            mUIDigRightCtl.Bucket_push.GetComponent<TweenColor>().from = Color.white;
        }
        else if (mUIDigRightCtl.widget.localPosition.x > 30)
        {
            mUIDigRightCtl.Bucket_pull.GetComponent<TweenColor>().from = color_lever;
            mUIDigRightCtl.Bucket_push.GetComponent<TweenColor>().from = Color.white;
            mUIDigRightCtl.Down.GetComponent<TweenColor>().from = Color.white;
            mUIDigRightCtl.Up.GetComponent<TweenColor>().from = Color.white;
        }
        else if (mUIDigRightCtl.widget.localPosition.x < -30)
        {
            mUIDigRightCtl.Bucket_pull.GetComponent<TweenColor>().from = Color.white;
            mUIDigRightCtl.Bucket_push.GetComponent<TweenColor>().from = color_lever;
            mUIDigRightCtl.Down.GetComponent<TweenColor>().from = Color.white;
            mUIDigRightCtl.Up.GetComponent<TweenColor>().from = Color.white;
        }

        if (mUIDigRightCtl.widget.localPosition == new Vector3(0, 0, 0))
        {
            mUIDigRightCtl.Down.GetComponent<TweenColor>().from = Color.white;
            mUIDigRightCtl.Up.GetComponent<TweenColor>().from = Color.white;
            mUIDigRightCtl.Bucket_pull.GetComponent<TweenColor>().from = Color.white;
            mUIDigRightCtl.Bucket_push.GetComponent<TweenColor>().from = Color.white;
            preMessage.text = " ";
        }
    }


    void UpdateLeftWidgetState()
    {
        if (mUIDigLeftCtl.widget.localPosition.y > 1)
        {
            mUIDigLeftCtl.full.GetComponent<TweenColor>().from = color_lever;
            mUIDigLeftCtl.push.GetComponent<TweenColor>().from = Color.white;
            mUIDigLeftCtl.L_turn.GetComponent<TweenColor>().from = Color.white;
            mUIDigLeftCtl.R_turn.GetComponent<TweenColor>().from = Color.white;
        }
        else if (mUIDigLeftCtl.widget.localPosition.y < -30)
        {
            mUIDigLeftCtl.full.GetComponent<TweenColor>().from = Color.white;
            mUIDigLeftCtl.push.GetComponent<TweenColor>().from = color_lever;
            mUIDigLeftCtl.L_turn.GetComponent<TweenColor>().from = Color.white;
            mUIDigLeftCtl.R_turn.GetComponent<TweenColor>().from = Color.white;
        }
        else if (mUIDigLeftCtl.widget.localPosition.x > 30)
        {
            mUIDigLeftCtl.R_turn.GetComponent<TweenColor>().from = color_lever;
            mUIDigLeftCtl.L_turn.GetComponent<TweenColor>().from = Color.white;
            mUIDigLeftCtl.push.GetComponent<TweenColor>().from = Color.white;
            mUIDigLeftCtl.full.GetComponent<TweenColor>().from = Color.white;
        }
        else if (mUIDigLeftCtl.widget.localPosition.x < -30)
        {
            mUIDigLeftCtl.R_turn.GetComponent<TweenColor>().from = Color.white;
            mUIDigLeftCtl.L_turn.GetComponent<TweenColor>().from = color_lever;
            mUIDigLeftCtl.push.GetComponent<TweenColor>().from = Color.white;
            mUIDigLeftCtl.full.GetComponent<TweenColor>().from = Color.white;
        }

        if (mUIDigLeftCtl.widget.localPosition == new Vector3(0, 0, 0))
        {
            mUIDigLeftCtl.full.GetComponent<TweenColor>().from = Color.white;
            mUIDigLeftCtl.push.GetComponent<TweenColor>().from = Color.white;
            mUIDigLeftCtl.R_turn.GetComponent<TweenColor>().from = Color.white;
            mUIDigLeftCtl.L_turn.GetComponent<TweenColor>().from = Color.white;
            preMessage.text = " ";
        }
    }
}
