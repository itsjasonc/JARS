using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Human : MonoBehaviour
{
    public Transform[] wayPoints;
    private float speed = 3;

    private NavMeshAgent navigate;
    private int destination;
    private Animator animator;

    // Use this for initialization
    void Start ()
    {
        navigate = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navigate.speed = speed;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (!navigate.pathPending && navigate.remainingDistance < 0.5f)
        {
            Move();
            animator.SetBool("Running", true);
        }
        DestroyOnPress();
    }

    void Move()
    {
        if (wayPoints.Length == 0)
        {
            return;
        }
        navigate.destination = wayPoints[destination].position;
        destination = (destination + 1) % wayPoints.Length;
    }

    //For testing
    void DestroyOnPress()
    {
        if(Input.GetKeyDown(KeyCode.Comma))
        {
            Destroy(gameObject);
        }
    }
}
