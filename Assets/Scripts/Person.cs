using System;
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

    public enum MovementState
    {
        Normal,
        Travel
    }

    private Rigidbody2D rigidBody;
    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;
    private float lastInfection;
    private float infectedTime;

    public float speed = 1;
    public float infectionDistance = 1;
    public float infectionProbability = 0.2f;
    public float infectionCooldown = 1;
    public float timeToHealthy = 5;

    public float travelCheckCooldown = 1;
    public float lastTravelCheck;
    public float travelProbability = 0.01f;

    [HideInInspector]
    public patch patch;

    public Sprite personSprite;
    public Sprite infectedPersonSprite;
    public Sprite removedPersonSprite;

    public InfectionState infectionState;
    public MovementState movementState;
    public Vector3 traveledToPosition;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();

        AddStartingForce();
        movementState = MovementState.Normal;

        MakeSusceptible();
        lastTravelCheck = Time.timeSinceLevelLoad + travelCheckCooldown;
    }

    void NormalUpdate()
    {
        if(CanInfect())
        {
            InfectNearby();
        }

        if(infectionState == InfectionState.Infected
            && infectedTime + timeToHealthy < Time.timeSinceLevelLoad)
        {
            MakeRemoved();
        }

        if (lastTravelCheck + travelCheckCooldown < Time.timeSinceLevelLoad)
        {
            if(UnityEngine.Random.Range(0f, 1f) < travelProbability)
            {
                StartTravel();
            }

            lastTravelCheck = Time.timeSinceLevelLoad;
        }
    }

    void TravelUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, traveledToPosition, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, traveledToPosition) < 1f)
        {
            EndTravel();
        }
    }

    void Update()
    {
        if(movementState == MovementState.Normal)
        {
            NormalUpdate();
        }
    }

    private void FixedUpdate()
    {
        if(movementState == MovementState.Travel)
        {
            TravelUpdate();
        }
    }

    public void InfectNearby()
    {
        Person[] people = FindObjectsOfType<Person>();

        List<Person> peopleToInfect = people.ToList()
                                            .FindAll(p => p.infectionState == InfectionState.Susceptible)
                                            .FindAll(p => Vector3.Distance(p.transform.position, this.transform.position) < infectionDistance)
                                            .FindAll(p => UnityEngine.Random.Range(0f, 1f) < infectionProbability);
        peopleToInfect.ForEach(p => p.MakeInfected());
        lastInfection = Time.timeSinceLevelLoad + UnityEngine.Random.Range(0f, 1f);
    }

    private void AddStartingForce()
    {
        Vector2 force = UnityEngine.Random.insideUnitCircle.normalized;
        rigidBody.AddForce(force * speed, ForceMode2D.Impulse);
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
        infectedTime = Time.timeSinceLevelLoad;

        // Let's not infect immediately
        lastInfection = Time.timeSinceLevelLoad + infectionCooldown;
    }

    public void MakeRemoved()
    {
        infectionState = InfectionState.Removed;
        spriteRenderer.sprite = removedPersonSprite;
    }

    private bool CanInfect()
    {
        return infectionState == InfectionState.Infected
            && lastInfection + infectionCooldown < Time.timeSinceLevelLoad;
    }

    private void StartTravel()
    {
        List<patch> otherPatches = FindObjectsOfType<patch>().ToList().FindAll(p => p != this.patch);
        
        if(otherPatches.Count == 0)
        {
            return;
        }

        traveledToPosition = otherPatches[UnityEngine.Random.Range(0, otherPatches.Count - 1)].center.position;
        Debug.Log("Travel from patch " + this.patch + " to position " + traveledToPosition);

        movementState = MovementState.Travel;
        rigidBody.velocity = Vector2.zero;
        circleCollider.enabled = false;
    }

    private void EndTravel()
    {
        circleCollider.enabled = true;
        movementState = MovementState.Normal;
        AddStartingForce();
    }
}
