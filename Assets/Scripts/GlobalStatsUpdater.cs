using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GlobalStatsUpdater : MonoBehaviour
{
    public Text statsText;

    void Update()
    {
        UpdateStats();
    }

    void UpdateStats()
    {
        List<Person> people = FindObjectsOfType<Person>().ToList();
        int susceptibleCount = people.FindAll(p => p.infectionState == Person.InfectionState.Susceptible).Count;
        int infectedCount = people.FindAll(p => p.infectionState == Person.InfectionState.Infected).Count;
        int removedCount = people.FindAll(p => p.infectionState == Person.InfectionState.Removed).Count;

        statsText.text = "Susceptible: " + susceptibleCount +
                       "\nInfected: " + infectedCount +
                       "\nRemoved: " + removedCount;
    }
}
