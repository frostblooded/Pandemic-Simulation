using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class patch : MonoBehaviour
{
    public int peopleCount = 16;
    public int rowsCount = 4;

    public float patchHeight = 3;
    public float patchWidth = 3;

    public GameObject personPrefab;
    public Transform peopleHolder;
    public Text statsText;

    public Transform center;

    private List<Person> people;

    void Start()
    {
        float peoplePerRow = peopleCount / rowsCount;
        float distBetweenX = patchWidth / (peoplePerRow + 1);
        float distBetweenY = -patchHeight / (rowsCount + 1);
        int patientZeroIndex = UnityEngine.Random.Range(0, peopleCount - 1);
        int currentPersonIndex = 0;
        people = new List<Person>();

        for(int i = 0; i < rowsCount; i++)
        {
            for(int j = 0; j < peoplePerRow; j++)
            {
                Vector3 position = new Vector3(distBetweenX * j - patchWidth * 0.4f, distBetweenY * i - 0.4f, 0);
                Person newPerson = Instantiate(personPrefab, position, Quaternion.identity).GetComponent<Person>();
                newPerson.transform.SetParent(peopleHolder, false);
                newPerson.patch = this;

                if (currentPersonIndex == patientZeroIndex)
                {
                    newPerson.MakeInfected();
                }

                people.Add(newPerson);
                currentPersonIndex++;
            }
        }
    }

    void Update()
    {
        UpdateStats();
    }

    void UpdateStats()
    {
        int susceptibleCount = people.FindAll(p => p.infectionState == Person.InfectionState.Susceptible).Count;
        int infectedCount = people.FindAll(p => p.infectionState == Person.InfectionState.Infected).Count;
        int removedCount = people.FindAll(p => p.infectionState == Person.InfectionState.Removed).Count;

        statsText.text = "Susceptible: " + susceptibleCount +
                       "\nInfected: " + infectedCount +
                       "\nRemoved: " + removedCount;
    }
}
