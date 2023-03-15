using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOption : Button
{
    [SerializeField] private int index;
    private SelectionMenu selectionMenu;
    private float currentTime = 0.0f;
    //getters & setters
    public int Index {get=>index;}

    protected override void Start()
    {
        base.Start();
        selectionMenu = GetComponentInParent<SelectionMenu>();
        if (selectionMenu == null)
            Debug.Log("selection menu is null");
    }

    public void ChangeToThisOption() {
        if (selectionMenu.CurrentOptionIndex > Index) {

        }
    }

    // IEnumerator RotateToFacingCamera() {
    //     float rotateTime = 0.5f;
    //     currentTime += Time.unscaledDeltaTime;
    //     transform
    // }
}
