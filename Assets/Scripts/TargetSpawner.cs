using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Target[] targetPrefabs;    // 6. Polymorphism

    [Header("Spawn Settings")]
    [SerializeField] private Vector3 areaSize = new Vector3(12f, 3f, 4f);

    [SerializeField] private float minSpacing = 1.8f;

    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private int maxTargets = 6;

    private List<Target> _activeTargets = new List<Target>();
    private float _spawnTimer = 0f;


    void Start()
    {
        
    }

    void Update()
    {
        _activeTargets.RemoveAll(target => target == null);
        _spawnTimer -= Time.deltaTime;
        if(_spawnTimer <= 0f && _activeTargets.Count < maxTargets)
        {
            if (TrySpawnRandomTarget())
            {
                _spawnTimer = spawnInterval;
            }
        }
    }

    private bool TrySpawnRandomTarget()
    {
        Target prefab = targetPrefabs[Random.Range(0, targetPrefabs.Length)];

        if (prefab == null) return false;

        Vector3 position = GetRandomPointInArea();

        if (IsTooCloseToOtherTargets(position)) return false;


        Target newTarget = Instantiate(prefab, position, Quaternion.identity);
        _activeTargets.Add(newTarget);

        return true;

    }

    private bool IsTooCloseToOtherTargets(Vector3 position)
    {
        foreach (Target target in _activeTargets)
        {
            if (target != null && Vector3.Distance(target.transform.position, position) < minSpacing) return true;
        }
        return false;
    }

    private Vector3 GetRandomPointInArea()
    {
        Vector3 localOffset = new Vector3(
                Random.Range(-0.5f, 0.5f) * areaSize.x,
                Random.Range(-0.5f, 0.5f) * areaSize.y,
                Random.Range(-0.5f, 0.5f) * areaSize.z
            );

        return transform.position + localOffset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, areaSize);
    }

}
