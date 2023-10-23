using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    public GlanceableUILogic GlanceableLogic;
    public GlanceButtonUI SelectedButton;
    public bool isHandTriggered;
    public GameObject MainCamera;
    public CapsuleCanvasMovement CapsuleMovementScript;
    public Vector3 EyeFixationPoint;
    public TutorialUILogic TutorialUILogicObject;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public void SwitchSelectedButton(GlanceButtonUI newButton)
    {
        if (newButton == SelectedButton)
            return;

        if (SelectedButton != null)
        {
            SelectedButton.DeselectButton();
        }
        SelectedButton = newButton;
        if (SelectedButton !=null)
            SelectedButton.SelectButton();
    }

    public void ToggleIsHandTrigger(bool value)
    {
        isHandTriggered = value;
    }

}
