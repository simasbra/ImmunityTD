using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Assets.Scripts;

public class EnemyPathTests
{
    private GameObject enemyGO;
    private EnemyPath enemyPath;

    [SetUp]
    public void SetUp()
    {
        // Create a new scene for the test
        if (!SceneManager.GetSceneByName("TestScene").IsValid())
        {
            SceneManager.CreateScene("TestScene");
        }

        // Set up the Enemy GameObject
        enemyGO = new GameObject("Enemy");
        enemyPath = enemyGO.AddComponent<EnemyPath>();

        // Mock waypoints and adjust their positions according to the class logic
        enemyPath.Waypoints = new Transform[2];
        for (int i = 0; i < enemyPath.Waypoints.Length; i++)
        {
            GameObject wpGO = new GameObject($"WP{i}");
            wpGO.transform.position = new Vector3(960 + (i * 100), 540, 0);  // Adjusting positions based on the class logic
            enemyPath.Waypoints[i] = wpGO.transform;
        }

        // Set the enemy's initial position
        enemyGO.transform.position = new Vector2((enemyPath.Waypoints[0].position.x - 960) / 100, (enemyPath.Waypoints[0].position.y - 540) / 100);
    }

    [Test]
    public void MoveAlongPath_MovesTowardsFirstWaypoint()
    {
        // Set a specific Speed for predictable movement
        enemyPath.SetSpeed(5f);

        // Simulate a short period of movement
        float simulationTime = 0.1f;  // Simulate 0.1 seconds of movement
        for (float t = 0; t < simulationTime; t += Time.fixedDeltaTime)
        {
            enemyPath.Update();
        }

        // Calculate the adjusted position of the second waypoint
        Vector2 secondWaypointAdjustedPosition = new Vector2((enemyPath.Waypoints[1].position.x - 960) / 100, (enemyPath.Waypoints[1].position.y - 540) / 100);

        // Check if the enemy has moved closer to the second waypoint's adjusted position
        float distanceToNextWaypoint = Vector2.Distance(enemyGO.transform.position, secondWaypointAdjustedPosition);
        Assert.Less(distanceToNextWaypoint, 1f);  // Assuming the waypoints were initially more than 1 unit apart after adjustment
    }

    [UnityTest]
    public IEnumerator Enemy_Destroyed_After_Reaching_Last_Waypoint()
    {
        // Set the enemy Speed high enough to ensure it reaches the last waypoint in a single frame
        enemyPath.SetSpeed(10000f);

        // Move the enemy
        enemyPath.Update();

        // Wait for the next frame
        yield return null;

        // Check if the enemy GameObject has been destroyed
        Assert.IsTrue(enemyGO == null, "Enemy GameObject should be destroyed after reaching the last waypoint.");
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up after each test
        GameObject.DestroyImmediate(enemyGO);
    }
}
