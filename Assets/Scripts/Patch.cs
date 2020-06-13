using System;
using UnityEngine;

public class patch : MonoBehaviour
{
    public int peopleCount = 16;
    public int rowsCount = 4;

    public float patchHeight = 3;
    public float patchWidth = 3;

    public GameObject personPrefab;
    public Transform peopleHolder;

    void Start()
    {
        float peoplePerRow = peopleCount / rowsCount;
        float distBetweenX = patchWidth / (peoplePerRow + 1);
        float distBetweenY = -patchHeight / (rowsCount + 1);
        int patientZeroIndex = UnityEngine.Random.Range(0, peopleCount - 1);
        int currentPersonIndex = 0;

        Debug.Log("Infected person is " + patientZeroIndex);
        
        for(int i = 0; i < rowsCount; i++)
        {
            for(int j = 0; j < peoplePerRow; j++)
            {
                Vector3 position = new Vector3(distBetweenX * j - patchWidth * 0.4f, distBetweenY * i - 0.4f, 0);
                Person newPerson = Instantiate(personPrefab, position, Quaternion.identity).GetComponent<Person>();
                newPerson.transform.SetParent(peopleHolder, false);

                if (currentPersonIndex == patientZeroIndex)
                {
                    newPerson.MakeInfected();
                }

                currentPersonIndex++;
            }
        }
    }

    void Update()
    {
        
    }
}
