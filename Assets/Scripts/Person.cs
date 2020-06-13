using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Person : MonoBehaviour

{
    public enum InfectionState
    {
        Susceptible,
        Infected,
        Removed
    }

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    public float speed = 1;
    public Sprite infectedPersonSprite;
    public Sprite personSprite;
    public InfectionState infectionState;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Vector2 force = Random.insideUnitCircle.normalized;
        rigidBody.AddForce(force * speed, ForceMode2D.Impulse);

        if(Random.Range(0f, 1f) < 0.1)
        {
            MakeInfected();
        }
        else
        {
            MakeSusceptible();
        }
    }

    void Update()
    {
    }

    public void MakeSusceptible()
    {
        infectionState = InfectionState.Susceptible;
        spriteRenderer.sprite = personSprite;
    }

    public void MakeInfected()
    {
        infectionState = InfectionState.Infected;
        spriteRenderer.sprite = infectedPersonSprite;
    }
}

