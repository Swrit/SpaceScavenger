using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    public static PlayArea Instance { private set; get; }
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;


    [SerializeField] private BoxCollider2D enemyMovementZone;
    [SerializeField] private BoxCollider2D playerMovementZone;
    [SerializeField] private BoxCollider2D activeZone;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = ObjectPoolManager.Instance.RequestObjectAt(playerPrefab, playerSpawnPoint.position);
        player.GetComponent<ShipBehaviour>()?.SetClampBox(playerMovementZone);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsOutsideActiveZone(Collider2D collider)
    {
        return (!activeZone.bounds.Intersects(collider.bounds));
    }

    public bool IsInsideEnemyMoveZone(Collider2D collider)
    {
        return (!enemyMovementZone.bounds.Intersects(collider.bounds));
    }

    public Vector3 RandomEnemyGoal()
    {
        Vector3 randomPoint = new Vector3();
        randomPoint.x = UnityEngine.Random.Range(enemyMovementZone.bounds.min.x, enemyMovementZone.bounds.max.x);
        randomPoint.y = UnityEngine.Random.Range(enemyMovementZone.bounds.min.y, enemyMovementZone.bounds.max.y);

        return randomPoint;
    }

}
