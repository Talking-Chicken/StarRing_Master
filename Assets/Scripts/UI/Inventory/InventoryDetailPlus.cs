using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.InventoryEngine;
using TMPro;
using MoreMountains.Tools;

public class InventoryDetailPlus : InventoryDetails
{
    [Header("Components Extensions")]
    [MMInformation("Here you need to bind the extension panel components.",MMInformationAttribute.InformationType.Info,false)]
    public TextMeshProUGUI TitlePro;
    public TextMeshProUGUI ShortDescriptionPro;
    public TextMeshProUGUI DescriptionPro;
    public TextMeshProUGUI QuantityPro;

    protected override IEnumerator FillDetailFields(InventoryItem item, float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);
        if (TitlePro!=null) { TitlePro.text = item.ItemName ; }
        if (ShortDescriptionPro!=null) { ShortDescriptionPro.text = item.ShortDescription;}
        if (DescriptionPro!=null) { DescriptionPro.text = item.Description;}
        if (QuantityPro!=null) { QuantityPro.text = item.Quantity.ToString();}
        if (Icon!=null) { Icon.sprite = item.Icon;}
        
        if (HideOnEmptySlot && !Hidden && (item.Quantity == 0))
        {
            StartCoroutine(MMFade.FadeCanvasGroup(_canvasGroup,_fadeDelay,0f));
            Hidden=true;
        }
    }

    protected override IEnumerator FillDetailFieldsWithDefaults(float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);
        if (TitlePro!=null) { TitlePro.text = DefaultTitle ;}
        if (ShortDescriptionPro!=null) { ShortDescriptionPro.text = DefaultShortDescription;}
        if (DescriptionPro!=null) { DescriptionPro.text = DefaultDescription;}
        if (QuantityPro!=null) { QuantityPro.text = DefaultQuantity;}
        if (Icon!=null) { Icon.sprite = DefaultIcon;}
    }
}
