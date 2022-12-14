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
        Debug.Log("Move helper: " + moveHelper.transform.position);
        impulseGrow *= spaceShipMass*50;
        fuelMass *= spaceShipMass;
        fuelLostSpeed *= spaceShipMass*spaceShipMass* spaceShipMass;
        //shipBody.AddForce(new Vector3(750 * Time.deltaTime , 0, 0));
    }

    #region TestText
    [SerializeField]
    private Text fuelText, impulseSpeedText, impulseText, speedText;
    #endregion

    [SerializeField]
    private GameObject observer, moveHelper;
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
            this.transform.Rotate(new Vector3(0,-1,0));
        }
        if(Input.GetKey(KeyCode.E))
        {
            this.transform.Rotate(new Vector3(0, 1, 0));
            //shipBody.AddTorque(new Vector3(0, 1, 0));
        }
        float speedX, speedZ, angle = this.transform.rotation.y;
        speedX = -impulseSpeed * Mathf.Cos(angle);
        speedZ = -impulseSpeed * Mathf.Sin(angle);
        //shipBody.AddForce(new Vector3(-speedX,0,speedZ ));
        shipBody.AddForce(DistanceMove()*(-impulseSpeed));
        Gravity();
        prevPos = curPos;    
    }


    void Gravity()
    {
        Vector3 forces = observer.GetComponent<GravityModel>().GetNearbyPlanetGravityForces(this.gameObject);
        shipBody.AddForce(forces);
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
        Debug.Log(Mathf.Log(fullShipMass / spaceShipMass));
        impulseSpeed = impulse * Mathf.Log(fullShipMass / spaceShipMass);
        if (impulse != 0 && fuelMass>0)
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

    public float GetMass()
    {
        return fullShipMass;
    }


    public Vector3 DistanceMove()
    {
        Vector3 retVec = (moveHelper.transform.position - this.transform.position).normalized;
        retVec.y = 0;
        return retVec;
    }
    //TODO: Рассчитать скорость коробля относительно сил притяжения планет.

    //TODO: Нормальный поворот и управление, а то это говно какое-то
}
