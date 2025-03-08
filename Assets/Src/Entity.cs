using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public BasicEntityInfoDisplay basicEntityInfoDisplay;
    public string Name;
    public Party Party;
    public int HealthPoints;
    public int currentHeathPoints;
    public int AttackPower;
    public float MovementSpeed;
    public bool isReady = true;
    public bool isMoving = false;
    public float AttackCooldown;
    public float LastAttackTime;
    public bool IsReadyToAttack => Time.time - LastAttackTime >= AttackCooldown;
    public bool OnEncounter;

    // colliders
    public Collider2D triggerCollider;
    public Collider2D combatCollider;

    void Start()
    {
        OnEncounter = false;
        isReady = true;
        isMoving = false;
        LastAttackTime = Time.time + AttackCooldown;
        currentHeathPoints = HealthPoints;
        ShowEntityInfo(false);
        SwitchCollider(false);
    }

    private Coroutine currentMoveCoroutine = null;

    public void SwitchCollider(bool inCombat)
    {
        triggerCollider.enabled = !inCombat;
        combatCollider.enabled = inCombat;
    }

    public void Attack(Entity target)
    {
        if(!IsReadyToAttack)
        {
            Debug.Log("Attack on cooldown");
            return;
        }
        LastAttackTime = Time.time;
        target.TakeDamage(AttackPower);
    }

    public void TakeDamage(int damage)
    {
        currentHeathPoints -= damage;
        basicEntityInfoDisplay.UpdateHeathBar((float)currentHeathPoints / HealthPoints);
        if (currentHeathPoints <= 0)
        {
            Party.entities.Remove(this); // so we dont throw null refs
            // implement death logic here - rn we just blow up the object
            Destroy(gameObject);
        }
    }

    public void ResetStatus()
    {
        currentHeathPoints = HealthPoints;
    }

    public void StopMovement()
    {
        if (currentMoveCoroutine != null)
        {
            StopCoroutine(currentMoveCoroutine);
        }

        isMoving = false;
        isReady = true;
    }

    public void StartMoveToTransformPosition(Transform target)
    {
        StartNewMovement(target.position);
    }

    public void StartMoveToVector3Position(Vector3 target)
    {
        StartNewMovement(target);
    }

    private void StartNewMovement(Vector3 target)
    {
        StopMovement();
        currentMoveCoroutine = StartCoroutine(MoveToPositionCoroutine(target));
    }

    private IEnumerator MoveToPositionCoroutine(Vector3 target)
    {
        isMoving = true;
        isReady = false;
        target = new Vector3(target.x, target.y, 0);

        while (transform.position != target && isMoving)
        {
            float step = MovementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            yield return null;
        }

        Debug.Log("Entity reached the target position.");
        isReady = true;
        isMoving = false;
        LastAttackTime = Time.time; // we are done moving, so we reset the attack timer
        currentMoveCoroutine = null;
    }

    public void ShowEntityInfo(bool state)
    {
        if(basicEntityInfoDisplay == null)
        {
            return;
        }

        basicEntityInfoDisplay.gameObject.SetActive(state);
        if(state)
        {
            basicEntityInfoDisplay.entityName.text = Name;
            basicEntityInfoDisplay.UpdateHeathBar((float)currentHeathPoints / HealthPoints);
            basicEntityInfoDisplay.UpdateActionBar(Mathf.Min((Time.time - LastAttackTime) / AttackCooldown, 1f));
        }
    }
}