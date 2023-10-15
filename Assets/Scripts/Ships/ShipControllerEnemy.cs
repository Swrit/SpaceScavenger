using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControllerEnemy : MonoBehaviour, I_ShipControllerInterface, I_ObjectReset
{
    public event EventHandler<int> OnWeaponStart;
    public event EventHandler<int> OnWeaponStop;

    private Vector2 inputVector = Vector2.zero;
    private BoxCollider2D boxCollider;
    private Health health;
    private int weaponSlotsAmount;

    private enum EnemyActionType
    {
        setTimer,
        wait,
        waitForGoal,
        waitForPlayArea,
        startWeapon,
        stopWeapon,
        startAllWeapons,
        stopAllWeapons,
        setTrackPlayerHorizontal,
        setRandomGoal,
        moveDown,
        setVulnerability,
        stopMovement,
    }

    [Serializable]
    private struct EnemyAction
    {
        public EnemyActionType type;
        public float floatParam;
        public int intParam;
        public bool boolParam;
    }

    [SerializeField] private List<EnemyAction> pattern;
    private int currentAction = 0;
    private float actionTimer = 0;
    private Vector3 goalPosition = new Vector3();
    private float goalThreshold = 8f;
    private bool trackPlayerHorizontal = false;



    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        health = GetComponent<Health>();
        ResetObject();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessAction(pattern[currentAction]);
    }

    public Vector2 GetMovementVector()
    {
        Vector2 input = inputVector;
        if (trackPlayerHorizontal)
        {
            Vector3 playerDir = GameManager.Instance.GetPlayerPosition() - transform.position;
            input.x = Mathf.Sign(playerDir.x);
            input.Normalize();
        }
        return input;
    }

    private void ProcessAction(EnemyAction enemyAction)
    {
        switch (enemyAction.type)
        {
            case EnemyActionType.setTimer:
                actionTimer = enemyAction.floatParam;
                NextAction();
                break;
            case EnemyActionType.wait:
                if (actionTimer < 0) NextAction();
                else actionTimer -= Time.deltaTime;
                break;
            case EnemyActionType.waitForGoal:
                if (Vector3.Distance(transform.position, goalPosition) <= goalThreshold) NextAction();
                break;
            case EnemyActionType.waitForPlayArea:
                if (PlayArea.Instance.IsInsideEnemyMoveZone(boxCollider)) NextAction();
                break;
            case EnemyActionType.startWeapon:
                OnWeaponStart?.Invoke(this, enemyAction.intParam);
                NextAction();
                break;
            case EnemyActionType.stopWeapon:
                OnWeaponStop?.Invoke(this, enemyAction.intParam);
                NextAction();
                break;
            case EnemyActionType.startAllWeapons:
                for (int i = 0; i<weaponSlotsAmount; i++)
                {
                    OnWeaponStart?.Invoke(this, i);
                }
                NextAction();
                break;
            case EnemyActionType.stopAllWeapons:
                for (int i = 0; i < weaponSlotsAmount; i++)
                {
                    OnWeaponStop?.Invoke(this, i);
                }
                NextAction();
                break;
            case EnemyActionType.setTrackPlayerHorizontal:
                trackPlayerHorizontal = enemyAction.boolParam;
                NextAction();
                break;
            case EnemyActionType.setRandomGoal:
                goalPosition = PlayArea.Instance.RandomEnemyGoal();
                inputVector = (goalPosition - transform.position).normalized;
                NextAction();
                break;
            case EnemyActionType.moveDown:
                inputVector = Vector2.down;
                NextAction();
                break;
            case EnemyActionType.setVulnerability:
                health.SetVulnerability(enemyAction.boolParam);
                NextAction();
                break;
            case EnemyActionType.stopMovement:
                inputVector = Vector2.zero;
                NextAction();
                break;
        }
    }

    private void NextAction()
    {
        currentAction++;
        if (currentAction == pattern.Count) currentAction--;
    }

    public void ResetObject()
    {
        inputVector = Vector2.zero;
        weaponSlotsAmount = GetComponent<ShipBehaviour>().HowManyWeapons();
        currentAction = 0;
        actionTimer = 0;
        goalPosition = new Vector3();
    }
}
