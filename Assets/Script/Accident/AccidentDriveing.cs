using UnityEngine;
using System.Collections;
using UnityEngine.VR;


public enum GEAR
{
    N,
    D,
    R,
}
public class AccidentDriveing : MonoBehaviour
{
    public float parking_break = 0;

    public Rigidbody body;
    public AudioSource soundEngine;
    public Transform centerOfMass;
    public WheelCollider[] wheels;
    public float steer_max = 20.0f;
    public float motor_max = 10.0f;
    public float brake_max = 200.0f;
    private float volume;
    private bool b_Once = false;
    
    private bool b_AccelIV;
    private bool b_gearIV;
    private float MySpeed;
    private float MyRPM;

    public AudioSource diesel;

    [HideInInspector] public float steer = 0;
    [HideInInspector] public float motor = 0;
    [HideInInspector] public float brake = 0;
    [HideInInspector] public GEAR gear = GEAR.N;

    public bool isDone;

    private float pastData = 0f;

    private float Forward
    {
        get
        {
            switch (gear)
            {
                case GEAR.D:
                    return 1.0f;
                case GEAR.R:
                    return -1.0f;
            }
            return 0;
        }
    }

    void Start()
    {
        body.centerOfMass = centerOfMass.localPosition;
        b_AccelIV = false;
        b_gearIV = false;
        MySpeed = 20.0f;
        MyRPM = 80.0f;
    }

    public void UpdateButtonsVR()
    {
        if (ArduinoData.Instance.arduinoOk.Equals(true))
        {
            if (PlayerPrefs.GetInt("Gear") == 1)
            {
                gear = GEAR.D;
                b_gearIV = true;
            }
            else if (PlayerPrefs.GetInt("Gear") == 0)
            {
                gear = GEAR.N;
                b_AccelIV = false;
                b_gearIV = false;
            }
            else if (PlayerPrefs.GetInt("Gear") == 2) //후진모드
            {
                gear = GEAR.R;
                b_gearIV = true;
            }
        }

        //가속 패달
        if (AccidentDriveScene.instance.steering.GetAcceleraterPedal() > 0f)
        {
            motor = Mathf.Clamp(AccidentDriveScene.instance.steering.GetAcceleraterPedal(), 0, 1);
            b_AccelIV = true;
        }
        else
        {
            b_AccelIV = false;
        }

        // 브레이크
        if (AccidentDriveScene.instance.steering.GetBrakePedal() > 0f)
        {
            brake = Mathf.Clamp(AccidentDriveScene.instance.steering.GetBrakePedal() + 0.5f, 0, 1);

            if (MySpeed > 20.0f)
            {
                MySpeed = MySpeed - 80.0f * Time.deltaTime;
            }
        }
        else
        {
            if (ArduinoData.Instance.arduinoOk && ArduinoData.Instance.safepin)
            {
                if (isLever_Press_btn == false)
                {
                    parking_break = 0.1f;
                }
                else
                {
                    parking_break = 1f;
                    brake = 0;
                }
            }
            else
            {
                parking_break = 0;
                wheels[0].motorTorque = 0;
                wheels[1].motorTorque = 0;
                wheels[2].motorTorque = 0;
                wheels[3].motorTorque = 0;
            }
        }

        float cluch = Steering.Instance.GetCluchPedal();
        if (cluch == 1)
        {
            if (cluch != pastData)
            {
                Lever_Press_btn();
            }
        }
        pastData = cluch;
    }

    public bool isLever_Press_btn = false;

    public void Lever_Press_btn()
    {
        if (isLever_Press_btn)
        {
            isLever_Press_btn = !isLever_Press_btn;
        }
        else
        {
            isLever_Press_btn = !isLever_Press_btn;
        }
    }

    public void UpdateButtons()
    {
        if (!b_Once)
        {
            b_Once = true;
        }

        //드라이브 모드
        if (PlayerPrefs.GetInt("Gear") == 1)
        {
            if (!LogManager.Instance.Exist("GEAR : " + gear.ToString()))
            {
                LogManager.Instance.Save("GEAR : " + gear.ToString());
            }

            gear = GEAR.D;
            b_gearIV = true;
        }
        else if (PlayerPrefs.GetInt("Gear") == 0)
        {
            if (body.gameObject.name != "Excavator_caterpiller" && !LogManager.Instance.Exist("GEAR : " + gear.ToString()))
            {
                LogManager.Instance.Save("GEAR : " + gear.ToString());
            }
            gear = GEAR.N;
            b_AccelIV = false;
            b_gearIV = false;
        }
        else if (PlayerPrefs.GetInt("Gear") == 2) //후진모드
        {
            if (!LogManager.Instance.Exist("GEAR : " + gear.ToString()))
            {
                LogManager.Instance.Save("GEAR : " + gear.ToString());
            }
            gear = GEAR.R;
            b_gearIV = true;
        }
        
        //핸들
        if (Input.GetAxisRaw("Handle") > 0.1f || Input.GetAxisRaw("Handle") < -0.1f)
        {
            steer = Mathf.Clamp(Input.GetAxisRaw("Handle"), -1, 1);
            GameEvent.notify(GameEvent.TAG.DRIVE_HANDLE);
        }

        //가속 패달
        if (Input.GetAxisRaw("Accelerate") > -0.9f)
        {
            motor = Mathf.Clamp(Input.GetAxis("Accelerate"), 0, 1);
            b_AccelIV = true;

            if (motor > 0 && 0.2f > motor)
            {
                LogManager.Instance.Save("SPEED : 4 ");
            }
            else if (motor >= 0.2f && 0.4f > motor)
            {
                LogManager.Instance.Save("SPEED : 3 ");
            }
            else if (motor >= 0.4f && 0.6f > motor)
            {
                LogManager.Instance.Save("SPEED : 2 ");
            }
            else if (motor >= 0.6f && 0.8f > motor)
            {
                LogManager.Instance.Save("SPEED : 1 ");
            }
            else
            {
                LogManager.Instance.Save("SPEED : 0 ");
            }
        }
        else
        {
            b_AccelIV = false;
        }

        // 브레이크
        if (Input.GetAxisRaw("Break") > -0.9f)
        {
            brake = -1 * Mathf.Clamp(((Input.GetAxis("Break") + 1) * 0.5f), 0, -1);

            if (MySpeed > 20.0f)
            {
                MySpeed = MySpeed - 80.0f * Time.deltaTime;
            }
        }
        else
        {
            if (ArduinoData.Instance.arduinoOk && ArduinoData.Instance.safepin)
            {

                if (isLever_Press_btn == false)
                {
                    parking_break = 0.1f;
                }
                else
                {
                    parking_break = 1f;
                    brake = 0;
                }
            }
            else
            {
                parking_break = 0;
                wheels[0].motorTorque = 0;
                wheels[1].motorTorque = 0;
                wheels[2].motorTorque = 0;
                wheels[3].motorTorque = 0;
            }
        }
    }


    void SetSpeedIV()
    {
        if (b_gearIV)        //기어가 들어가면 바늘이 떨리게
        {
            //   Debug.Log("1111");
            if (b_AccelIV)       //악셀을 밟았을때 속도계
            {
                if (MySpeed < Random.Range(-90.0f, -100.0f))
                {
                    //  스피드
                    MySpeed = MySpeed + Random.Range(10.0f, 25.0f) * Time.deltaTime;
                }
                else
                {
                    MySpeed = MySpeed - 40.0f * Time.deltaTime;
                }
            }
            else if (MySpeed > 17.0f)
            {
                MySpeed = MySpeed - 40.0f * Time.deltaTime;
            }
            else
            {
                MySpeed = MySpeed + 30.0f * Time.deltaTime;
            }
        }
    }

    void Update()
    {
        UpdateButtonsVR();

        float _forward = Forward;

        if (isDone)
        {
            wheels[0].brakeTorque = brake_max * 1;
            wheels[1].brakeTorque = brake_max * 1;
            wheels[2].brakeTorque = brake_max * 1;
            wheels[3].brakeTorque = brake_max * 1;

            return;
        }

        if (GetComponent<ViveAccident>().brake)
        {
            brake = 1;
        }
        wheels[0].motorTorque = motor_max * motor * _forward * parking_break;
        wheels[1].motorTorque = motor_max * motor * _forward * parking_break;
        wheels[2].motorTorque = motor_max * motor * _forward * parking_break;
        wheels[3].motorTorque = motor_max * motor * _forward * parking_break;
        wheels[0].brakeTorque = brake_max * brake;
        wheels[1].brakeTorque = brake_max * brake;
        wheels[2].brakeTorque = brake_max * brake;
        wheels[3].brakeTorque = brake_max * brake;


        wheels[0].steerAngle = steer_max * steer;
        wheels[1].steerAngle = steer_max * steer;

        float _velocity = body.velocity.magnitude;
        _velocity = Mathf.Clamp(_velocity, 0.0f, 13.0f);
        body.velocity = body.velocity.normalized * _velocity;

        if (_velocity >= 0.1f)
            volume = Mathf.Clamp01(_velocity * 0.3f);

        SetSpeedIV();  //계기판
    }


    GEAR GetAxisCaterpillerInput(string inputName)
    {
        // 0 은 중립 , -1은 후진, 1은 전진으로 구분

        if (Input.GetAxisRaw(inputName) > 0.3f)
        {
            return GEAR.D;
        }
        else if (Input.GetAxisRaw(inputName) < -0.3f)
        {
            return GEAR.R;
        }
        else
        {
            return GEAR.N;
        }
    }
}
