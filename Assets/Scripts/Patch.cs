using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Patch : MonoBehaviour
{
    public int peopleCount = 16;
    public int rowsCount = 4;

    public GameObject personPrefab;

    void Start()
    {
        float peoplePerRow = peopleCount / rowsCount;
        
        for(int i = 0; i < rowsCount; i++)
        {
            for(int j = 0; j < peoplePerRow; j++)
            {
                Instantiate(personPrefab, new Vector3(0.75f * j - 1.1f, -0.75f * i - 0.4f, 0), Quaternion.identity);
            }
        }
    }

    void Update()
    {
        
    }
}
