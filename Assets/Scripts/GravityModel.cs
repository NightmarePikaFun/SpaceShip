using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModel : MonoBehaviour
{

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

    public Vector3[] GetGraviteForseAllPlanet(GameObject spaceShip)
    {
        Vector3[] forcesToPlanet = new Vector3[planets.Length];
        float[] forces = new float[planets.Length];
        for(int i = 0; i <forces.Length;i++)
        {
            forces[i] = GravityConst * (planets[i].GetComponent<Planet>().GetMass()*spaceShip.GetComponent<Ship>().GetMass())/Mathf.Pow(GetRadius(planets[i],spaceShip),2);
            forcesToPlanet[i] = (planets[i].transform.position-spaceShip.transform.position).normalized * forces[i];
        }
        //TODO: Дописать направление гравитационных сил для каждой планеты
        return forcesToPlanet;
    }

    private float GetRadius(GameObject planet, GameObject spaceShip)
    {
        return Vector3.Distance(planet.transform.position, spaceShip.transform.position);
    }
}
