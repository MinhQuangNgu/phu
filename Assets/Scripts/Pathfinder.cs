using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    EnemySpawner enemySpawner;
    WayConfigSO waveConfig;
    List<Transform> waypoints;
    int waypointIndex = 0;

    float minusSpeed = 0f;
    float minusTime = 0f;

    void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Start()
    {
        waveConfig = enemySpawner.GetCurrentWave();
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].position;
    }


    void Update()
    {
        FollowPath();
        minusTime -= Time.deltaTime;
        if (minusTime <= 0f)
        {
            minusTime = 0f;
            minusSpeed = 0f;
        }
    }
    void FollowPath()
    {
        if (waypointIndex < waypoints.Count)
        {
            Vector3 targetPosition = waypoints[waypointIndex].position;
            float delta = (waveConfig.GetMoveSpeed() - minusSpeed) * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta);
            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ChangeSpeed(float minusSpeed, float minusTimer)
    {
        this.minusSpeed = minusSpeed;
        minusTime = minusTimer;
    }
}
