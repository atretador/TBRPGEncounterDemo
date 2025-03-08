using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class Encounter : MonoBehaviour
{
    public Party enemyParty;
    public List<Transform> enemyPositions = new List<Transform>();
    public Party party;
    public List<Transform> partyPositions = new List<Transform>();
    public bool Triggered = false;

    // Update is called once per frame
    void Update()
    {
        if(!Triggered)
        {
            return;
        }

        //wait till everyone is in position to start attacking
        if(party.entities.Concat(enemyParty.entities).Any(x => x.isReady == false))
        {
            return;
        }

        foreach (Entity entity in party.entities)
        {
            entity.ShowEntityInfo(true);
            if (entity.IsReadyToAttack)
            {
                // here we just pick a random entity from the list of aggro'd entities, but you could implement something like a threat system or unity type priority
                var target = enemyParty.entities[UnityEngine.Random.Range(0, enemyParty.entities.Count)];
                entity.Attack(target);
            }
        }
    }

    public void TriggerEncounter(Party otherParty)
    {
        Triggered = true;

        Debug.Log("Enemies spotted!");
        enemyParty = otherParty;
        if(party.entities.Count > partyPositions.Count)
        {
            Debug.LogError("Not enough positions for our party!");
            return;
        }
        if(enemyParty.entities.Count > enemyPositions.Count)
        {
            Debug.LogError("Not enough positions for the enemy party!");
            return;
        }

        foreach (Entity entity in party.entities.Concat(enemyParty.entities))
        {
            entity.ResetStatus();
            entity.ShowEntityInfo(true);
            entity.OnEncounter = true;
            entity.StopMovement();
            entity.SwitchCollider(true); //enable combat colliders so we can click on them
        }

        //move encounter party members to their positions
        for(int i = 0; i < party.entities.Count; i++)
        {
            party.entities[i].StartMoveToTransformPosition(partyPositions[i]);
        }

        //move enemy party members to their positions
        for(int i = 0; i < enemyParty.entities.Count; i++)
        {
            enemyParty.entities[i].StartMoveToTransformPosition(enemyPositions[i]);
        }
    }
}
