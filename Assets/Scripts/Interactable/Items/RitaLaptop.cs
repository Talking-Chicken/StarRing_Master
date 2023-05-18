using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using MoreMountains.Feedbacks;
using UnityEngine.UI;

public class RitaLaptop : InteractObj, IInteractable
{
    [SerializeField, BoxGroup("Relative Objs")] private GameObject wall;
    [SerializeField, BoxGroup("Feedbacks")] private MMF_Player cameraFBIN, cameraFBOUT;
    [SerializeField, BoxGroup("Sign in")] private int passwordCount = 4;
    [SerializeField, BoxGroup("Sign in")] private List<GameObject> passwordFills;
    [SerializeField, BoxGroup("Sign in")] private GameObject popupWindow, laptopCanvas, downloadBar;
    [SerializeField, BoxGroup("Sign in")] private RectTransform downloadBarFill;
    [SerializeField, BoxGroup("Sign in")] private float timeToDownload;
    private float currentTime = 0.0f;
    private List<int> password = new List<int>();
    private bool hasWindowShowed = false;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (password.Count >= 4 && currentTime < timeToDownload) {
            if (downloadBar.gameObject.activeSelf == false) {
                downloadBar.gameObject.SetActive(true);
            }
            currentTime += Time.deltaTime;
            downloadBarFill.localScale = new Vector3(Mathf.Lerp(0, 1, currentTime/timeToDownload), 1, 1);
        }

        if (currentTime >= timeToDownload) {
            if (!hasWindowShowed) {
                popupWindow.SetActive(true);
                hasWindowShowed = true;
            }
        }
    }

    /// interact with rita's laptop,
    /// it transit camera to see the laptop
    public override bool Interact() {
        base.Interact();
        wall.layer = 0;
        downloadBarFill.localScale = new Vector3(0,0,0);
        currentTime = 0.0f;
        hasWindowShowed = false;
        laptopCanvas.SetActive(true);
        cameraFBIN.PlayFeedbacks();
        return true;
    }

    public void EndInteract() {
        wall.layer = 24;
        laptopCanvas.SetActive(false);
        popupWindow.SetActive(false);
        cameraFBOUT.PlayFeedbacks();
        Player.ChangeState(Player.stateExplore);
    }

    public void EnterPassword(int num) {
        if (password.Count < passwordCount) {
            password.Add(num);
            UpdatePasswordUI();
        }
    }

    public void DeletePassword() {
        if (password.Count > 0) {
            password.RemoveAt(password.Count-1);
            UpdatePasswordUI();
        }
    }

    public void UpdatePasswordUI() {
        for (int i = 0; i < passwordFills.Count; i++) {
            if (password.Count > i)
                passwordFills[i].SetActive(true);
            else
                passwordFills[i].SetActive(false);
        }
    }
}
