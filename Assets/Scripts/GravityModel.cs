using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModel : MonoBehaviour
{

    GameObject[] planets;
    private float GravityConst = 6.67408f * Mathf.Pow(10, -11);


    // Start is called before the first frame update
    void Start()
    {
        planets = GameObject.FindGameObjectsWithTag("Planet");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3[] GetGraviteForseAllPlanet(GameObject spaceShip)
    {
        Vector3[] forcesToPlanet = new Vector3[planets.Length];
        float[] forces = new float[planets.Length];
        for(int i = 0; i <forces.Length;i++)
        {
            forces[i] = GravityConst * (planets[i].GetComponent<Planet>().GetMass()*spaceShip.GetComponent<Ship>().GetMass())/Mathf.Pow(GetRadius(planets[i],spaceShip),2);
        }
        //TODO: Дописать направление гравитационных сил для каждой планеты
        return null;
    }

    private float GetRadius(GameObject planet, GameObject spaceShip)
    {
        return Vector3.Distance(planet.transform.position, spaceShip.transform.position);
    }
}
