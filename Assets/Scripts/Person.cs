using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Person : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    public float speed = 1;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        Vector2 force = Random.insideUnitCircle.normalized;
        rigidBody.AddForce(force * speed, ForceMode2D.Impulse);
    }

    void Update()
    {
    }
}
