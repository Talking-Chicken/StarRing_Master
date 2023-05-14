using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.InventoryEngine;
using TMPro;

public class InventoryDisplayPlus : InventoryDisplay
{
    public InventorySlotPlus SlotPrefabPlus;
    protected InventorySlotPlus _slotPrefabPlus = null;
    public TMP_FontAsset TMPFont;


    protected override void InitializeSlotPrefab()
    {
        base.InitializeSlotPrefab();
        if (SlotPrefabPlus != null) {
            _slotPrefabPlus = SlotPrefabPlus;
        }
    }
}
