using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum InteractableType {OBJ, NPC, EXM}

/// this is the parent class for all interactable objects (including NPCs and constructions)
public class Interactable : MonoBehaviour
{
    [SerializeField, BoxGroup("Cursor Settings")] private InteractableType type = InteractableType.OBJ;
    [SerializeField, BoxGroup("Properties")] private bool isInteractable = true;
    [SerializeField, BoxGroup("Properties")] private List<Material> outlineMats;

    // getters & setters
    public InteractableType Type {get=>type;}
    public bool IsInteractable {get=>isInteractable; set=>isInteractable=value;}
    public List<Material> OutlineMats {get=>outlineMats; protected set =>outlineMats=value;}
    
    protected virtual void Start() {}

    
    protected virtual void Update() {}


}
