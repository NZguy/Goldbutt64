using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShooter : Enemy
{
    public GameObject target;
    public float distance;
    public float rotSpeed = 100f;
    public float followRange = 15f;
    public float eyeLevel = 10f;

    public NavMeshAgent agent;


    public override void Update()
    {

        transform.LookAt(new Vector3 (target.transform.position.x, eyeLevel, target.transform.position.z));
        distance = Vector3.Distance(this.transform.position, target.transform.position);

        if(distance < followRange)
        {
            if (agent.isStopped == false)
            {
                agent.isStopped = true;
            }

            Vector3 targetPos = Vector3.MoveTowards(transform.position, player.transform.position, 2 * Time.deltaTime);
            transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        }
        else if (agent.isStopped || (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0 && agent.remainingDistance != Mathf.Infinity) )
        {
            agent.isStopped = false;
            agent.SetDestination(GetRandomLocation());

        }
    }

    private Vector3 GetRandomLocation()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        // Pick the first indice of a random triangle in the nav mesh
        int t = Random.Range(0, navMeshData.indices.Length - 3);

        // Select a random point on it
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;
    }

}

