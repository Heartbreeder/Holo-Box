using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoloBoxLogic : MonoBehaviour
{
    [SerializeField] GameObject[] MissionTabObjects;
    [SerializeField] GameObject[] ToolTabObjects;
    [SerializeField] GameObject[] MaterialTabObjects;

    [SerializeField] GameObject MissionTextField;
    [SerializeField] GameObject MissionObjectParent;

    [Header("MissionObjects")]
    [SerializeField] GameObject M1Obj;
    [SerializeField] GameObject M3Obj;
    [SerializeField] GameObject M9Obj;
    [SerializeField] GameObject M14Obj;
    [SerializeField] GameObject M33Obj;
    [SerializeField] GameObject M37Obj;
    [SerializeField] GameObject M44Obj;
    [SerializeField] GameObject M46Obj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        SwitchTab(1);
    }


    public void CloseHoloBox()
    {
        GameMaster.instance.GlanceableLogic.EnableSimpleGlanceable();
    }

    public void SwitchTab(int option)
    {
        if(option == 1)
        {
            foreach (GameObject obj in MissionTabObjects) obj.SetActive(true);
            foreach (GameObject obj in ToolTabObjects) obj.SetActive(false);
            foreach (GameObject obj in MaterialTabObjects) obj.SetActive(false);
        }
        else if (option == 2)
        {
            foreach (GameObject obj in MissionTabObjects) obj.SetActive(false);
            foreach (GameObject obj in ToolTabObjects) obj.SetActive(true);
            foreach (GameObject obj in MaterialTabObjects) obj.SetActive(false);
        }
        else
        {
            foreach (GameObject obj in MissionTabObjects) obj.SetActive(false);
            foreach (GameObject obj in ToolTabObjects) obj.SetActive(false);
            foreach (GameObject obj in MaterialTabObjects) obj.SetActive(true);
        }
    }

    public void Init(ActiveMissionData am)
    {
        if (am == null) return;

        string fullText = "";
        fullText += "Object Size: " + am.Template.ObjectSizeText + "\n";
        fullText += "Material: " + am.MissionMaterialText + "\n";
        if (am.Template.MissionType == MissionTypes.Milling)
        {
            fullText += "Carving Depth: " + am.Template.CarvingDepthText + "\n";
        }
        else
        {
            fullText += "Object Diameter: " + am.Template.CarvingDepthText + "\n";
        }


        fullText += "Manufacturing Tool: " + am.Template.RequiredTool;

        MissionTextField.GetComponent<TextMeshProUGUI>().text = fullText;
        MissionObjectParent.SetActive(true);

        M1Obj.SetActive(false);
        M3Obj.SetActive(false);
        M9Obj.SetActive(false);
        M14Obj.SetActive(false);
        M33Obj.SetActive(false);
        M37Obj.SetActive(false);
        M44Obj.SetActive(false);
        M46Obj.SetActive(false);

        if (am.Template.BaseMissionIndex == MissionIndexes.M1)
        {
            M1Obj.SetActive(true);
        }else if (am.Template.BaseMissionIndex == MissionIndexes.M3)
        {
            M3Obj.SetActive(true);
        }
        else if(am.Template.BaseMissionIndex == MissionIndexes.M9)
        {
            M9Obj.SetActive(true);
        }else if (am.Template.BaseMissionIndex == MissionIndexes.M14)
        {
            M14Obj.SetActive(true);
        }
        else if (am.Template.BaseMissionIndex == MissionIndexes.M33)
        {
            M33Obj.SetActive(true);
        }
        else if (am.Template.BaseMissionIndex == MissionIndexes.M37)
        {
            M37Obj.SetActive(true);
        }
        else if (am.Template.BaseMissionIndex == MissionIndexes.M44)
        {
            M44Obj.SetActive(true);
        }
        else if (am.Template.BaseMissionIndex == MissionIndexes.M46)
        {
            M46Obj.SetActive(true);
        }
    }

}

