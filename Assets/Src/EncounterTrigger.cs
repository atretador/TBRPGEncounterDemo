using UnityEngine;

public class EncounterTrigger : MonoBehaviour
{
    private Entity entity;
    public Encounter encounter;

    void Awake()
    {
        entity = GetComponent<Entity>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == gameObject)
        {
            return;
        }

        Entity otherEntity = other.GetComponent<Entity>();
        if (otherEntity != null && otherEntity.Party != entity.Party)
        {
            Debug.Log("Encounter Triggered!");
            encounter.TriggerEncounter(otherEntity.Party);
        }
    }
}
