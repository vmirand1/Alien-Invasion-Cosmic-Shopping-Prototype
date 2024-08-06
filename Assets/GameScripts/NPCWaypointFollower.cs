using UnityEngine;

public class NPCWaypointFollower : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 3.0f;
    private int currentWaypointIndex = 0;

    void Start()
    {
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[currentWaypointIndex].position;
        }
    }

    void Update()
    {
        if (waypoints.Length == 0)
            return;

        MoveTowardsWaypoint();
    }

    void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        Vector3 move = direction.normalized * speed * Time.deltaTime;

        if (move.magnitude >= direction.magnitude)
        {
            transform.position = targetWaypoint.position;
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
        else
        {
            transform.position += move;
        }
    }
}
