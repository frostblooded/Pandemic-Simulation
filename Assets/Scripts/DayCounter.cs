using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCounter : MonoBehaviour
{
    public Text text;

    void Update()
    {
        text.text = "Day: " + Mathf.FloorToInt(Time.time);
    }
}
