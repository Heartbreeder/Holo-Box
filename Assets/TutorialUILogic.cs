using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialUILogic : MonoBehaviour
{
    //Index 0 = No tutorial, 1= first tutorial...
    public int TutoriaIndex;
    public ControllerHolobox Control;
    public GameObject[] Tutorials;

    public TextMeshProUGUI TabletopText;
    public TextMeshProUGUI HoloboxText;
    public string SpawnErrorMsg = "<color=red><b>Please spawn an interface first.</b></color>";

    private string TabletopOriginalText;
    private string HoloboxOriginalText;

    private bool IsFirstTimeTutorial;

    // Start is called before the first frame update
    void Start()
    {
        TutoriaIndex = 1;
        ShowTutorial(TutoriaIndex);
        IsFirstTimeTutorial = true;

        TabletopOriginalText = TabletopText.text;
        HoloboxOriginalText = HoloboxText.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTutorial(int index)
    {
        int i = 1;
        foreach(GameObject go in Tutorials)
        {
            TutoriaIndex = 0;
            if(index > 0 && index <= Tutorials.Length && index == i)
            {
                go.SetActive(true);
                TutoriaIndex = i;
                //Additional settings
                if(TutoriaIndex == 3)
                {
                    Control.SpawnTabletop = true;
                    TabletopText.text = TabletopOriginalText;
                }
                else if (TutoriaIndex == 4)
                {
                    Control.SpawnHolobox = true;
                    HoloboxText.text = HoloboxOriginalText;
                }
                break;

            }
            else
            {
                go.SetActive(false);
            }
            i++;
        }


    }

    public void DisableSpawnHolobox()
    {
        if (gameObject.GetComponent<GlanceableUILogic>().HoloBox == null)
        {
            HoloboxText.text = HoloboxOriginalText + "\n" + SpawnErrorMsg;
            return;
        }

        Control.SpawnHolobox = false;

        if (IsFirstTimeTutorial)
        {
            IsFirstTimeTutorial = false;
        }
        ShowTutorial(0);

    }

    public void DisableSpawnTabletop()
    {
        if (gameObject.GetComponent<GlanceableUILogic>().Tabletop == null)
        {
            TabletopText.text = TabletopOriginalText + "\n" + SpawnErrorMsg;
            return;
        }

        Control.SpawnTabletop = false;

        if (IsFirstTimeTutorial)
        {
            ShowTutorial(4);
        }
        else
        {
            ShowTutorial(0);
        }
    }
}
