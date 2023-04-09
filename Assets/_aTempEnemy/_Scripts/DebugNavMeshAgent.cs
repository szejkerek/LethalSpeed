using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DebugNavMeshAgent : MonoBehaviour
{
    [SerializeField] private bool showGizmos = true;

    NavMeshAgent agent;
    bool inPlayMode = false;

    private void Awake()
    {
        inPlayMode = true;
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnDrawGizmos()
    {
        if (!inPlayMode || !showGizmos)
            return;

        //Velocity line
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + agent.velocity);

        //Desired velocity line
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + agent.desiredVelocity);

        //Path
        Gizmos.color = Color.black;
        NavMeshPath agentPath = agent.path;
        Vector3 lastCorner = transform.position;
        foreach (Vector3 corner in agentPath.corners)
        {
            Gizmos.DrawLine(lastCorner, corner);
            Gizmos.DrawSphere(corner, 0.2f);
            lastCorner = corner;
        }
    }
}
