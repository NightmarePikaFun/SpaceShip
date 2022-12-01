using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private bool canMove;
    [SerializeField]
    private float mass;
    [SerializeField]
    private float rangeToSun;
    [SerializeField]
    private float rotateAroundCoef;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float offsetSin, offsetCos;
    

    private Rigidbody rb;
    private float currentPos;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RotateAround();
        if(canMove)
            Move();
    }

    void RotateAround()
    {
        //rb.AddTorque(new Vector3(0,impulseSpeed*364,0));
        this.transform.Rotate( new Vector3(0, speed* rotateAroundCoef, 0));
    }
    
    void Move()
    {
        Vector3 route = new Vector3(0, 0, 0);
        route.x += Mathf.Sin(currentPos) * rangeToSun * offsetSin;
        route.z += Mathf.Cos(currentPos) * rangeToSun * offsetCos;
        transform.position = route;

        currentPos += Mathf.PI * speed * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter "+other.gameObject.tag);
        if (other.gameObject.tag == "SpaceShip")
            Destroy(other.gameObject);
    }

    public float GetMass()
    {
        return mass;
    }
}
