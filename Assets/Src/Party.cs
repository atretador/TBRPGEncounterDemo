using System;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{
    public List<Entity> entities = new List<Entity>();
    // if we belong to a set encounter
    public Encounter myEncounter;
}
