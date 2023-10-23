using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GlanceableUILogic : MonoBehaviour
{
    public GameObject SimpleUiObject;
    public GameObject DetailedUiObject;
    public GameObject HoloBox;
    public GameObject HoloBoxPrefab;
    public GameObject Tabletop;
    public GameObject TabletopPrefab;

    [Header("SimpleGlanceableObjects")]
    public TextMeshProUGUI SGText;
    [Header("DetailedGlanceableObjects")]
    public TextMeshProUGUI DGText;
    public Image DGPathImage;

    // Start is called before the first frame update
    void Start()
    {
        //EnableSimpleGlanceable();
        DisableAllGlanceable();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHoloBox(GameObject target)
    {
        if (HoloBox != null) Destroy(HoloBox);
        HoloBox = target;
        //EnableSimpleGlanceable();
    }

    public void SetTabletop(GameObject target)
    {
        if (Tabletop != null) Destroy(Tabletop);
        Tabletop = target;
    }

    public void EnableSimpleGlanceable()
    {
        GameMaster.instance.GlanceableLogic.Tabletop.GetComponent<EvaluationBoxLogic>().AddInterfaceSwitch();

        SimpleUiObject.SetActive(true);
        DetailedUiObject.SetActive(false);
        if (HoloBox != null)
        {
            HoloBox.transform.GetChild(0).gameObject.SetActive(false);
            HoloBox.GetComponent<HoloBoxLogic>().SwitchTab(1);
        }
        GameMaster.instance.SwitchSelectedButton(null);
        GameMaster.instance.ToggleIsHandTrigger(false);

        if (Tabletop != null)
        {
            ActiveMissionData am = Tabletop.GetComponent<EvaluationBoxLogic>().ActiveMission;
            SGText.text = am.Template.MissionTitle + ", " + am.MissionMaterialText;
        }

    }

    public void EnableDetailedGlanceable()
    {
        GameMaster.instance.GlanceableLogic.Tabletop.GetComponent<EvaluationBoxLogic>().AddInterfaceSwitch();

        SimpleUiObject.SetActive(false);
        DetailedUiObject.SetActive(true);
        if (HoloBox != null) { 
            HoloBox.transform.GetChild(0).gameObject.SetActive(false);
            HoloBox.GetComponent<HoloBoxLogic>().SwitchTab(1);
        }
        GameMaster.instance.SwitchSelectedButton(null);
        GameMaster.instance.ToggleIsHandTrigger(false);

        if (Tabletop != null)
        {
            ActiveMissionData am = Tabletop.GetComponent<EvaluationBoxLogic>().ActiveMission;

            string fullText = "";
            fullText += "Object Size: " + am.Template.ObjectSizeText + "\n";
            fullText += "Material: " + am.MissionMaterialText + "\n";
            if(am.Template.MissionType == MissionTypes.Milling)
            {
                fullText += "Carving Depth: " + am.Template.CarvingDepthText + "\n";
            }
            else
            {
                fullText += "Object Diameter: " + am.Template.CarvingDepthText + "\n";
            }


            fullText += "Manufacturing Tool: " + am.Template.RequiredTool;

            DGText.text = fullText;
            DGPathImage.sprite = am.Template.MissionBlueprint;

        }
    }

    public void EnableHoloBox()
    {
        GameMaster.instance.GlanceableLogic.Tabletop.GetComponent<EvaluationBoxLogic>().AddInterfaceSwitch();

        SimpleUiObject.SetActive(false);
        DetailedUiObject.SetActive(false);
        if (HoloBox != null)
        {
            HoloBox.transform.GetChild(0).gameObject.SetActive(true);
            HoloBox.GetComponent<HoloBoxLogic>().Init(Tabletop.GetComponent<EvaluationBoxLogic>().ActiveMission);
        }

        GameMaster.instance.SwitchSelectedButton(null);
        GameMaster.instance.ToggleIsHandTrigger(false);
    }

    public void DisableAllGlanceable()
    {
        SimpleUiObject.SetActive(false);
        DetailedUiObject.SetActive(false);
        if (HoloBox != null)
        {
            HoloBox.transform.GetChild(0).gameObject.SetActive(false);
            HoloBox.GetComponent<HoloBoxLogic>().SwitchTab(1);
        }
        GameMaster.instance.SwitchSelectedButton(null);
        GameMaster.instance.ToggleIsHandTrigger(false);

    }
}
