using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCStateBase
{
    public abstract void EnterState(NPC npc);
    public abstract void UpdateState(NPC npc);
    public abstract void LeaveState(NPC npc);
}
