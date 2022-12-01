using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CalcFullShipMass();
        shipBody = this.GetComponent<Rigidbody>();
        curPos = prevPos = transform.position;
    }

    #region TestText
    [SerializeField]
    private Text fuelText, impulseSpeedText, impulseText, speedText;
    #endregion


    [SerializeField]
    private float impulse;
    [SerializeField, Range(0,1.0f)]
    private float impulseGrow;
    [SerializeField]
    private float spaceShipMass;
    [SerializeField]
    private float fuelMass;
    [SerializeField, Range(0, 1.0f)]
    private float fuelLostSpeed;

    private Rigidbody shipBody;
    private float impulseSpeed;
    private float fullShipMass;
    private Vector3 curPos, prevPos;
    // Update is called once per frame
    void FixedUpdate()
    {
        curPos = transform.position;
        SpeedCalc();
        DisplayStats();
        if(Input.GetKey(KeyCode.F))
        {
            impulse += impulseGrow;
        }
        if(Input.GetKey(KeyCode.G))
        {
            impulse -= impulseGrow;
        }
        if(Input.GetKey(KeyCode.T))
        {
            impulse = 0;
        }
        if(Input.GetKey(KeyCode.Q))
        {
            shipBody.AddTorque(new Vector3(0,-1,0));
        }
        if(Input.GetKey(KeyCode.E))
        {
            shipBody.AddTorque(new Vector3(0, 1, 0));
        }
        float speedX, speedZ, angle = this.transform.rotation.y;
        speedX = -impulseSpeed * Mathf.Cos(angle);
        speedZ = -impulseSpeed * Mathf.Sin(angle);
        //shipBody.AddForce(new Vector3(-speedX,0,speedZ ));
        shipBody.AddForce(new Vector3(0, 0, -impulseSpeed));
        prevPos = curPos;    
    }

    void DisplayStats()
    {
        fuelText.text = "Fuel: " + fuelMass;
        impulseSpeedText.text = "Impulse Speed: " + Rounding(impulseSpeed);
        impulseText.text = "Impulse: " + impulse;
        float[] curSpeed = GetSpeed();
        speedText.text = "Current speed -> x: " + Rounding(curSpeed[0]) + " z: " + Rounding(curSpeed[1]);
    }


    float Rounding(float input)
    {
        return input - input % 0.00001f;
    }

    void SpeedCalc()
    {
        impulseSpeed = impulse * Mathf.Log(fullShipMass / spaceShipMass);
        if (impulse != 0)
        {
            fuelMass = fuelMass - fuelLostSpeed;  
        }
        CalcFullShipMass();
    }

    void CalcFullShipMass()
    {
        fullShipMass = spaceShipMass + fuelMass;
    }

    float[] GetSpeed()
    {
        return new float[] {curPos.x-prevPos.x, curPos.z-prevPos.z };
    }

    float GiveForceFromOtherPlanet()
    {
        return 0;
    }

    public float GetMass()
    {
        return fullShipMass;
    }

    //TODO: Рассчитать скорость коробля относительно сил притяжения планет.

    //TODO: Нормальный поворот и управление, а то это говно какое-то
}
