using UnityEngine;
using System.Collections.Generic;

public class ResourceClusterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] int numberOfClusters = 5;
    [SerializeField] float spawnRadius = 20f;
    [SerializeField] float minDistanceFromPlayer = 5f;
    
    [Header("Cluster Settings")]
    [SerializeField] int minResourcesPerCluster = 3;
    [SerializeField] int maxResourcesPerCluster = 8;
    [SerializeField] float clusterRadius = 3f;
    [SerializeField] float enemyChance = 0.3f; // 30% chance for enemy instead of resource
    
    [Header("Prefabs")]
    [SerializeField] GameObject[] resourcePrefabs;
    [SerializeField] GameObject[] enemyPrefabs;
    
    void Start()
    {
        SpawnResourceClusters();
    }
    
    void SpawnResourceClusters()
    {
        for (int i = 0; i < numberOfClusters; i++)
        {
            Vector2 clusterCenter = FindValidClusterPosition();
            int resourceCount = Random.Range(minResourcesPerCluster, maxResourcesPerCluster + 1);
            
            SpawnCluster(clusterCenter, resourceCount);
        }
    }
    
    Vector2 FindValidClusterPosition()
    {
        Vector2 playerPos = Player.Instance != null ? Player.Instance.transform.position : Vector2.zero;
        Vector2 clusterPos;
        int attempts = 0;
        
        do
        {
            // Random position within spawn radius
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float randomDistance = Random.Range(minDistanceFromPlayer, spawnRadius);
            clusterPos = playerPos + randomDirection * randomDistance;
            
            attempts++;
            if (attempts > 50) break; // Avoid infinite loop
            
        } while (Vector2.Distance(clusterPos, playerPos) < minDistanceFromPlayer);
        
        return clusterPos;
    }
    
    void SpawnCluster(Vector2 center, int count)
    {
        List<Vector2> spawnPositions = GenerateClusterPositions(center, count);
        
        foreach (Vector2 position in spawnPositions)
        {
            if (Random.value < enemyChance)
            {
                SpawnEnemy(position);
            }
            else
            {
                SpawnResource(position);
            }
        }
    }
    
    List<Vector2> GenerateClusterPositions(Vector2 center, int count)
    {
        List<Vector2> positions = new List<Vector2>();
        
        // Always place one at the center
        positions.Add(center);
        
        // Place the rest around it
        for (int i = 1; i < count; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * clusterRadius;
            Vector2 position = center + randomOffset;
            
            // Make sure positions don't overlap too much
            bool validPosition = true;
            foreach (Vector2 existingPos in positions)
            {
                if (Vector2.Distance(position, existingPos) < 0.8f)
                {
                    validPosition = false;
                    break;
                }
            }
            
            if (validPosition)
            {
                positions.Add(position);
            }
            else
            {
                // Try again with different offset
                i--;
            }
        }
        
        return positions;
    }
    
    void SpawnResource(Vector2 position)
    {
        if (resourcePrefabs.Length > 0)
        {
            GameObject resourcePrefab = resourcePrefabs[Random.Range(0, resourcePrefabs.Length)];
            Instantiate(resourcePrefab, position, Quaternion.identity);
        }
    }
    
    void SpawnEnemy(Vector2 position)
    {
        if (enemyPrefabs.Length > 0)
        {
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(enemyPrefab, position, Quaternion.identity);
        }
    }
    
    // Call this to spawn more clusters during gameplay
    public void SpawnMoreClusters(int additionalClusters)
    {
        for (int i = 0; i < additionalClusters; i++)
        {
            Vector2 clusterCenter = FindValidClusterPosition();
            int resourceCount = Random.Range(minResourcesPerCluster, maxResourcesPerCluster + 1);
            SpawnCluster(clusterCenter, resourceCount);
        }
    }
}