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
    private float infectedTime;

    public float speed = 1;
    public float infectionDistance = 1;
    public float infectionProbability = 0.2f;
    public float infectionCooldown = 1;
    public float timeToHealthy = 5;

    public Sprite personSprite;
    public Sprite infectedPersonSprite;
    public Sprite removedPersonSprite;

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

        if(infectionState == InfectionState.Infected
            && infectedTime + timeToHealthy < Time.time)
        {
            MakeRemoved();
        }
    }

    public void InfectNearby()
    {
        Person[] people = FindObjectsOfType<Person>();

        List<Person> peopleToInfect = people.ToList()
                                            .FindAll(p => p.infectionState == InfectionState.Susceptible)
                                            .FindAll(p => Vector3.Distance(p.transform.position, this.transform.position) < infectionDistance)
                                            .FindAll(p => Random.Range(0f, 1f) < infectionProbability);
        peopleToInfect.ForEach(p => p.MakeInfected());
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
        infectedTime = Time.time;
    }

    public void MakeRemoved()
    {
        infectionState = InfectionState.Removed;
        spriteRenderer.sprite = removedPersonSprite;
    }

    private bool CanInfect()
    {
        return infectionState == InfectionState.Infected
            && lastInfection + infectionCooldown < Time.time;
    }
}
