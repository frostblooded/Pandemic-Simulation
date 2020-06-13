using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstInfector : MonoBehaviour
{
    public float timeBeforeFirstInfection = 2;

    void Update()
    {
        if(Time.timeSinceLevelLoad > timeBeforeFirstInfection)
        {
            Person[] people = FindObjectsOfType<Person>();
            int infectedIndex = Random.Range(0, people.Length - 1);
            people[infectedIndex].MakeInfected();
            Destroy(this);
        }
    }
}
