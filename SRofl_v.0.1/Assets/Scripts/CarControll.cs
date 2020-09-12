using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CarControll : MonoBehaviour
{

    public AudioMixer Mixer;

    [SerializeField]
    private int Acceleration;

    [SerializeField]
    private int MaxSpeed;

    [SerializeField]
    private int MotorTorque;

    public WheelJoint2D[] Wheels = new WheelJoint2D[2];
    JointMotor2D Motor;

    private int gearId;

    [SerializeField]
    RectTransform arrowRPM;

    [SerializeField]
    RectTransform arrow2;

    private float MaxSpeedM;
    public float rpm;
    public float rpms;
    private float gearBoxTime;
    private float gearBoxCd;

    // public void ChangeVolume (float volume)
    // {
    //     Mixer.audioMixer.SetFloat("EngineProfil", rpm);
    // }

    void Start ()
    {
        
        rpm = 0;
        int gearId = 1;
        // rpm = Motor.motorSpeed;
        // int Acceleration = 30;
        // int MaxSpeed = 1000;
        // int MotorTorque = 245;
    }
        // определяет угол наклона стрелки показа оборотов двигателя
        float GetRPMAngle(float rpm)
        { 
            return 0f - 220f * (rpm / (gearNumbers[gearId]*250));
        }

        float GetRPMAngle2(float rpms)
        { 
            if (gearId == 0)
            {
                return -11f - 112f * (rpms/200);
            }
            else 
            {
                return -11f - 112f * (rpms / (MaxSpeed / (gearNumbers[gearId])));
            }
        }

        float kek()
        {
            if (gearId == 0)
            {
                Mixer.SetFloat("EngineGrouPitch", 0.7f + (rpms/200));
                Mixer.SetFloat("EngineGroup", 1 + rpms * 2 / 200);
            }
            else
            {
                Mixer.SetFloat("EngineGrouPitch", 0.7f + (rpms/(MaxSpeed/gearNumbers[gearId]/3.835f)));
                Mixer.SetFloat("EngineGroup", 1 + rpms * 2 / (MaxSpeed / (gearNumbers[gearId]/3.835f)));
            }
        }
        
        // float rpms = (Motor.motorSpeed + Acceleration) + Time.deltaTime;

        // время переключения (чтобы блокировать газ на короткий промежуток )
        float gearBox = 0.0f;

        float[] gearNumbers = new float[] {0f, 3.626f, 2.2f, 1.541f, 1.213f, 1f, 0.767f};
    
     	void FixedUpdate ()
     	{
            
            // rpm = Mathf.Clamp(rpm, 0f, 2000f);

            // rpms = Mathf.Clamp(rpms, 0f, 17000f);

            //Если хотим переключить передачу вверх и это не последняя (верхняя)
            if (Input.GetKeyDown(KeyCode.UpArrow) && (gearId < 6))
            {
                gearId++;   
                gearBox=Time.time + gearBoxCd;
                print (gearId);
                print (gearNumbers[gearId]);
            }   
                
                // Если хотим переключить передачу вниз и это не нейтральная (нижняя)
                else if (Input.GetKeyDown(KeyCode.DownArrow) && (gearId > 0))
                {
                    gearId--;   
                    gearBox=Time.time + gearBoxCd;
                    print (gearId);
                }
     		
            float MaxSpeedM = MaxSpeed / (gearNumbers[gearId]/3.835f);

            if (Input.GetKey(KeyCode.S) && Motor.motorSpeed < MaxSpeedM)
     		{
     			// Motor.motorSpeed = Mathf.Clamp(Motor.motorSpeed, 0f, 2000f);
                rpms = (Motor.motorSpeed + Acceleration) + Time.deltaTime;
                Motor.motorSpeed = rpms;
     			Motor.maxMotorTorque = MotorTorque * (gearNumbers[gearId]);
     			Wheels[0].motor = Motor;
     			Wheels[1].motor = Motor;
     		}

     		else if (Input.GetKey(KeyCode.W) && Motor.motorSpeed > -MaxSpeedM)
     		{
                // Motor.motorSpeed = Mathf.Clamp(Motor.motorSpeed, 0f, 20000f);
                rpms = (Motor.motorSpeed - Acceleration) + Time.deltaTime;
                Motor.motorSpeed = rpms;
     			Motor.maxMotorTorque = MotorTorque * (gearNumbers[gearId]);
     			Wheels[0].motor = Motor;
     			Wheels[1].motor = Motor;
     		}

     		else                      // тормоз
     		{
     			if (Motor.motorSpeed > 0)  // Назад
     			{
     				rpms = (Motor.motorSpeed - Acceleration) - Time.deltaTime;
                    Motor.motorSpeed = rpms;  // = Math.pow(k*x,n)
     				Wheels[0].motor = Motor;
     				Wheels[1].motor = Motor;	
     			}
     			else if (Motor.motorSpeed < 0) // Вперед
     			{
     				rpms = (Motor.motorSpeed + Acceleration) - Time.deltaTime;
                    Motor.motorSpeed = rpms;
     				Wheels[0].motor = Motor;
     				Wheels[1].motor = Motor;
     			}

     		}

            rpm = (Mathf.Abs(Motor.motorSpeed)/20);

            rpms = (Mathf.Abs(rpms)/4.45f);

            kek();
            
            arrowRPM.rotation = Quaternion.Euler(0.0f, 0.0f, GetRPMAngle(rpm));

            arrow2.rotation = Quaternion.Euler(0.0f, 0.0f, GetRPMAngle2(rpms));
        }
}