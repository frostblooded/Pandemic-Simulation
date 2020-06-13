using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private float lastInfection;

    public float speed = 1;
    public float infectionDistance = 1;
    public float infectionProbability = 0.2f;
    public float infectionCooldown = 1;
    public Sprite infectedPersonSprite;
    public Sprite personSprite;
    public InfectionState infectionState;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Vector2 force = Random.insideUnitCircle.normalized;
        rigidBody.AddForce(force * speed, ForceMode2D.Impulse);
        MakeSusceptible();
        lastInfection = Time.time + infectionCooldown;
    }

    void Update()
    {
        if(CanInfect())
        {
            InfectNearby();
        }
    }

    public void InfectNearby()
    {
        Person[] people = FindObjectsOfType<Person>();

        // Get the people that aren't infected, are in infection range, and pass
        // the infection probability check
        List<Person> peopleToInfect = people.ToList()
                                            .FindAll(p => p.infectionState != InfectionState.Infected)
                                            .FindAll(p => Vector3.Distance(p.transform.position, this.transform.position) < infectionDistance)
                                            .FindAll(p => Random.Range(0f, 1f) < infectionProbability);

        foreach(Person person in peopleToInfect)
        {
            person.MakeInfected();
        }

        lastInfection = Time.time + Random.Range(0f, 1f);
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

    private bool CanInfect()
    {
        return infectionState == InfectionState.Infected
            && lastInfection + infectionCooldown < Time.time;
    }
}
