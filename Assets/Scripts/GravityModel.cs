using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModel : MonoBehaviour
{

    public float shipMulitpayerSpeed;

    GameObject[] planets;
    [SerializeField]
    private GameObject sun;
    [SerializeField]
    private float GravityConst; //6.67408f * Mathf.Pow(10, -11);
    [SerializeField]
    private float multiplayer, planetSpeedMultiplayer;

    // Start is called before the first frame update
    void Start()
    {
        planets = GameObject.FindGameObjectsWithTag("Planet");
        for(int i = 0; i< planets.Length;i++)
        {
            planets[i].GetComponent<Planet>().SetSpeedMultiplayer(multiplayer);
        }
        Debug.Log(planets.Length);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlanetMove();
    }

    void PlanetMove()
    {
        Vector3[] forcesToPlanet = new Vector3[planets.Length];
        float[] forces = new float[planets.Length];
        for (int i = 0; i<planets.Length;i++)
        {
            if (planets[i].name == "Sun")
                continue;
            forces = new float[planets.Length];
            for (int j = 0; j< planets.Length;j+=100)
            {
                Vector3 distance = (sun.transform.position-planets[i].transform.position).normalized;
                forces[i] = GravityConst * (planets[i].GetComponent<Planet>().GetMass() * sun.GetComponent<Planet>().GetMass()) / Mathf.Pow(planets[i].GetComponent<Planet>().GetRangeToSun(), 2);
                float sunSpeed = GravityConst * sun.GetComponent<Planet>().GetMass() / (planets[i].GetComponent<Planet>().GetRangeToSun()* planetSpeedMultiplayer);
                planets[i].GetComponent<Planet>().AddForce(distance*forces[i], sunSpeed);
            }
            
        }
    }

    //гравитация для корабля
    public Vector3 GetNearbyPlanetGravityForces(GameObject spaceShip)
    {
        Vector3 forcesToPlanet;
        float forces;
        float[] distance = new float[planets.Length];
        int id = 0;
        float minDistance = float.MaxValue;
        for(int i = 0; i < distance.Length; i++)
        {
            distance[i] = GetRadius(planets[i], spaceShip);
            if(minDistance>distance[i])
            {
                minDistance = distance[i];
                id = i;
            }
        }
        forces = shipMulitpayerSpeed * (planets[id].GetComponent<Planet>().GetMass()* spaceShip.GetComponent<Ship>().GetMass())/(distance[id]);
        //Debug.Log(forces);
        forcesToPlanet = (planets[id].transform.position - spaceShip.transform.position).normalized * forces;
        /*for(int i = 0; i <forces.Length;i++)
        {
            forces[i] = GravityConst * (planets[i].GetComponent<Planet>().GetMass()*spaceShip.GetComponent<Ship>().GetMass())/distance[i];
            forcesToPlanet[i] = (planets[i].transform.position-spaceShip.transform.position).normalized * forces[i];
            Debug.Log("Force "+i+": "+forces[i]);
        } */
        //TODO: Дописать направление гравитационных сил для каждой планеты
        //Debug.Log("Force to ship: " + forcesToPlanet.Length);
        return forcesToPlanet;
    }

    private float GetRadius(GameObject planet, GameObject spaceShip)
    {
        return Vector3.Distance(planet.transform.position, spaceShip.transform.position)*1f;
    }
}
