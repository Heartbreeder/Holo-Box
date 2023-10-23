using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class GlanceButtonUI : MonoBehaviour
{

    public GameObject HighlightObject;
    public Image ButtonBackground;
    public Image ButtonFilling;

    public float triggerTime = 2.0f;

    public MyEvent OnClickEvent;

    private bool isSelected;
    private bool isBeingClicked;
    private float triggerCount;
    private Image fillButtonImage;
    // Start is called before the first frame update
    void Start()
    {
        isSelected = false;
        isBeingClicked = false;
        DeselectButton();
        triggerCount = 0;
        if(ButtonFilling!=null) fillButtonImage = ButtonFilling.GetComponent<Image>();
        if (ButtonFilling != null) fillButtonImage.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (isBeingClicked)
        {
            triggerCount += Time.deltaTime;

            if (triggerCount >= triggerTime)
            {
                if (ButtonFilling != null)  fillButtonImage.fillAmount = 0;
                ClickButton();
            }
            else
            {
                if (ButtonFilling != null) fillButtonImage.fillAmount = triggerCount / triggerTime;
            }
        }
        else
        {
            if (ButtonFilling != null) fillButtonImage.fillAmount = 0;
        }

        
    }

    public void SelectButton()
    {
        DelayedClickStop();
        isSelected = true;
        if (HighlightObject != null) HighlightObject.SetActive(true);
    }

    public void DeselectButton()
    {
        DelayedClickStop();
        isSelected = false;
        if(HighlightObject !=null) HighlightObject.SetActive(false);
    }

    public void ClickButton()
    {
        if (GameMaster.instance.isHandTriggered)
        {
            if(GameMaster.instance.GlanceableLogic.Tabletop !=null) GameMaster.instance.GlanceableLogic.Tabletop.GetComponent<EvaluationBoxLogic>().HandInteractionCounter++;
        }
        else
        {
            if (GameMaster.instance.GlanceableLogic.Tabletop != null) GameMaster.instance.GlanceableLogic.Tabletop.GetComponent<EvaluationBoxLogic>().EyeInteractionCounter++;
        }

        DeselectButton();
        isBeingClicked = false;
        GameMaster.instance.ToggleIsHandTrigger(false);
        OnClickEvent.Invoke();
    }

    public void DelayedClick()
    {
        isBeingClicked = true;
        triggerCount = 0;
    }

    public void DelayedClickStop()
    {
        isBeingClicked = false;
        triggerCount = 0;
    }

    public void DoNothing()
    {

    }

    
    public void OnTriggerEnter(Collider other)
    {
        GameMaster gm = GameMaster.instance;



        //Eye Tracking cursor selects button IF no hand trigger is selecting a button
        if (other.tag== "Cursor" && gm.isHandTriggered == false)
        {
            if (gm.SelectedButton == this)
                return;
            gm.SwitchSelectedButton(this);
            gm.ToggleIsHandTrigger(false);

        }
        //Hand cursor selects and clicks a button. Also toggles eye tracking cursor off.
        else if (other.tag == "HandCursor")
        {
            gm.SwitchSelectedButton(this);
            gm.ToggleIsHandTrigger(true);
            gm.SelectedButton.DelayedClick();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        GameMaster gm = GameMaster.instance;
        //If Eye tracking cursor exits button do nothing

        //If Hand tracking exits, stop clicking. Also toggle eye tracking cursor on.
        if (other.tag == "HandCursor")
        {
            gm.SelectedButton.DelayedClickStop();
            gm.ToggleIsHandTrigger(false);

        }
    }


}

[Serializable]
public class MyEvent : UnityEvent { }
