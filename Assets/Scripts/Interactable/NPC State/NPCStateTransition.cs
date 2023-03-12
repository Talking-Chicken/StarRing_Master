using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateTransition : NPCStateBase
{
    public override void EnterState(NPC npc) {
        npc.IsInteractable = false;
    }
    public override void UpdateState(NPC npc) {}
    public override void LeaveState(NPC npc) {
        npc.previousState = this;
    }
}
