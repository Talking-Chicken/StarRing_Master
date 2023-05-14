using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.InventoryEngine;
using TMPro;

public class InventorySlotPlus : InventorySlot
{
    public TextMeshProUGUI QuantityTextPro;
    public TextMeshProUGUI ItemNameTextPro;

    public override void DrawIcon(InventoryItem item, int index)
    {
        if (ParentInventoryDisplay != null)
        {				
            if (!InventoryItem.IsNull(item))
            {
                ItemNameTextPro.text = item.ItemName;    
            }
        }

        base.DrawIcon(item, index);
    }

    public override void DisableIconAndQuantity()
    {
        ItemNameTextPro.text = "";
        base.DisableIconAndQuantity();
    }

    public override void SetQuantity(int quantity)
    {
        if (quantity > 0)
        {
            QuantityTextPro.gameObject.SetActive(true);
            QuantityTextPro.text = "0"+quantity.ToString();	
        }
        else
        {
            QuantityTextPro.gameObject.SetActive(true);
            QuantityTextPro.text = "00";	
        }
    }
}
