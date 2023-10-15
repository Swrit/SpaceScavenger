using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalvageHook : WeaponBase, I_ObjectReset
{
    [SerializeField] private float maxRange = 200f;
    [SerializeField] private float extendingSpeed = 320f;
    [SerializeField] private float contractingSpeed = 320f;

    private float rangeRemains;
    [SerializeField] private float contractionThreshold = 1f;

    [SerializeField] private SpriteRenderer chainSprite;
    //[SerializeField] private Transform playZone;
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private ShipControllerPlayer shipControllerPlayer;

    private enum HookState
    {
        idle,
        extending,
        contracting,
        dragging,
    }

    private HookState currentState;
    private Grabbable grabbedObject;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        shipControllerPlayer = FindObjectOfType<ShipControllerPlayer>();

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case HookState.idle:
                break;
            case HookState.extending:
                if (rangeRemains == 0)
                {
                    SwitchState(HookState.contracting);
                    break;
                }

                float moveY = Mathf.Min(rangeRemains, extendingSpeed * Time.deltaTime);
                transform.Translate(new Vector3(0, moveY, 0));
                rangeRemains -= moveY;
                CheckForGrabbables();
                break;

            case HookState.contracting:
                if (Vector3.Distance(transform.position, shootPoint.position) < contractionThreshold)
                {
                    SwitchState(HookState.idle);
                    break;
                }
                transform.position = Vector3.MoveTowards(transform.position, shootPoint.position, contractingSpeed * Time.deltaTime);
                break;

            case HookState.dragging:
                if (grabbedObject.transform.parent != this.transform)
                {
                    SwitchState(HookState.contracting);
                    break;
                }
                if (Vector3.Distance(transform.position, shootPoint.position) < contractionThreshold)
                {
                    //grabbedObject.PickUp(shipControllerPlayer);
                    SwitchState(HookState.idle);
                    break;
                }
                transform.position = Vector3.MoveTowards(transform.position, shootPoint.position, contractingSpeed * grabbedObject.GetWeight() * Time.deltaTime);
                break;
        }
        UpdateChain();
    }


    private void SwitchState(HookState newState)
    {
        EndState(currentState);
        currentState = newState;
        StartState(currentState);
    }

    private void StartState(HookState hookState)
    {
        switch (hookState)
        {
            case HookState.idle:
                transform.position = shootPoint.position;
                transform.SetParent(shootPoint, true);
                break;
            case HookState.extending:
                rangeRemains = maxRange;
                break;
        }
    }

    private void EndState(HookState hookState)
    {
        switch (hookState)
        {
            case HookState.idle:
                transform.SetParent(null, true);
                break;
            case HookState.dragging:
                if (grabbedObject.transform.parent == this.transform) grabbedObject.transform.SetParent(null, true);
                grabbedObject.Ungrab();
                grabbedObject = null;
                break;
        }
    }

    private void UpdateChain()
    {
        float length = Vector2.Distance(transform.position, shootPoint.position);
        if (float.IsNaN(length)) length = 0f;
        chainSprite.size = new Vector2(chainSprite.size.x, length);
        chainSprite.transform.up = Vector3.Normalize(transform.position - shootPoint.position);
    }

    private void CheckForGrabbables()
    {
        List<Collider2D> collisions = new List<Collider2D>();
        ContactFilter2D cf2d = new ContactFilter2D().NoFilter();
        Physics2D.OverlapCollider(boxCollider, cf2d, collisions);

        foreach (Collider2D collision in collisions)
        {
            Grabbable grab = collision.GetComponent<Grabbable>();
            if (grab == null) continue;
            else
            {
                Grab(grab);
                return;
            }
        }
    }

    private void Grab(Grabbable grab)
    {
        grabbedObject = grab;
        grab.transform.SetParent(this.transform, true);
        grab.Grab();
        SwitchState(HookState.dragging);
    }

    public override void WeaponStart()
    {
        if (currentState == HookState.idle) SwitchState(HookState.extending);
    }

    public override void WeaponStop()
    {
        if (currentState == HookState.extending) SwitchState(HookState.contracting);
        if (currentState == HookState.dragging) SwitchState(HookState.contracting);
    }

    public void ResetObject()
    {
        SwitchState(HookState.idle);

    }
}
