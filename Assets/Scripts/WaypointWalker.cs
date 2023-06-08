using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WaypointWalker : MonoBehaviour
{
    public const float ReachThreshold = 0.1f;
    public GameObject ChaseTarget;

    private NavMeshAgent agent { get; set; }
    private List<Tuple<Vector3, Action>> waypointList { get; set; }
    private MovingActor Actor { get; set; }
    private int listIndex { get; set; }

    public bool going { get; private set; }
    public bool isChasing { get; private set; }
    public bool IsLooped;

    public float Speed 
    { 
        get => agent.speed;
        set
        {
            agent.speed = value;
        }
    }

    public void OnEnable()
    {
        try
        {
            Actor.isApplyingMovement = false;
            Actor.isImportingMovement = true;
            agent.enabled = true;
        }
        catch { };
    }

    public void OnDisable()
    {
        try
        {
            Actor.isApplyingMovement = true;
            Actor.isImportingMovement = false;
            agent.enabled = false;
        }
        catch { };
    }

    void Awake()
    {
        going = false;
        waypointList = new List<Tuple<Vector3, Action>>();

        Actor = GetComponent<MovingActor>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        Speed = Actor.Speed;
    }

    public void AddWaypoint(Vector3 point, Action action)
    {
        waypointList.Add(new Tuple<Vector3, Action>(point, action));
    }

    public bool ResetWaypoints()
    {
        if(going == false)
        {
            waypointList.Clear();
            Stop();
        }

        return !going;
    }

    public bool StartWalking()
    {
        if (going || waypointList == null)
            return false;

        going = true;

        MoveToCurrentPoint();

        return true;
    }

    private void MoveToCurrentPoint()
    {
        Vector3 point = waypointList[listIndex].Item1;
        agent.SetDestination(point);
        going = true;
    }

    IEnumerator ChasingTimer(int seconds)
    {
        yield return new WaitForSeconds(seconds);

        isChasing = false;
        agent.velocity = new Vector2();
        Actor.Movement = new Vector2();
        Stop();
    }

    public void ChaseEntity(GameObject target = null, int seconds = -1)
    {
        isChasing = true;
        going = true;
        if (target != null)
            ChaseTarget = target;

        if (seconds != -1)
            StartCoroutine(ChasingTimer(seconds));

        agent.SetDestination(ChaseTarget.transform.position);
    }

    private void CheckPosition()
    {
        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.velocity = new Vector2();
            Actor.Movement = new Vector2();
            agent.ResetPath();

            if (isChasing)
            {
                StopChasing();
            }
            else
            {
                waypointList[listIndex]?.Item2?.Invoke();
                listIndex = (listIndex + 1) % waypointList.Count;
            }

            if (listIndex == 0 && IsLooped == false)
                Stop();
            else
                MoveToCurrentPoint();
        }
    }

    public void Pause()
    {
        going = false;
        agent.isStopped = true;
    }

    public void Resume()
    {
        going = true;
        agent.isStopped = false;
    }

    public void Stop()
    {
        Pause();
        listIndex = 0;
        agent.ResetPath();
    }

    private void StopChasing()
    {
        isChasing = false;
        StopCoroutine("ChasingTimer");
    }

    private void SyncroniseWalkerMovement()
    {
        Actor.Movement = agent.velocity.normalized;
    }

    void Update()
    {
        if (going)
        {
            SyncroniseWalkerMovement();
            CheckPosition();
            if (isChasing)
                agent.SetDestination(ChaseTarget.transform.position);
        }
    }
}
