using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class EvaluationBoxLogic : MonoBehaviour
{
    TutorialUILogic TutorialLogic;
    GlanceableUILogic GlanceableLogic;

    [Header("Interface groups")]
    public GameObject InterfaceHighlightObject;
    public GameObject ButtonUIObject;
    public GameObject[] GetMissionButtons;
    public GameObject InactiveMissionUIObject;
    public GameObject ActiveMissionUIObject;
    public GameObject AbandonMissionButton;
    public GameObject Tools3DObjects;
    public GameObject MaterialsMilling3DObjects;
    public GameObject MaterialsTurning3DObjects;
    public GameObject SolutionsMilling3DObjects;
    public GameObject SoltutionsTurning3DObjects;
    public GameObject EvaluationCompleteInterface;

    [Header("ScriptableData")]
    public MissionData[] DemoMissionScriptableData;
    public MissionData[] EvalMissionScriptableData;

    [Header("Variables")]
    public ActiveMissionData ActiveMission;
    [SerializeField] private TabletopStates state;

    [Header("object Selections")]
    public GameObject SelectedTool;
    public ToolIndexes SelectedToolType;
    public GameObject SelectedMaterial;
    public MaterialIndexes SelectedMaterialType;
    public GameObject SelectedProduct;
    public MissionIndexes SelectedProductType;

    [Header("Check Solution")]
    public TextMeshProUGUI SolutionTextObject;
    public TextMeshProUGUI InactiveMissionTextObject;

    [Header("Player Metrics")]
    public int CompletedDemos;
    public float[] CompletionTimes;
    public int[] MissionErrors;
    public int[] InterfaceSwitches;
    public int EyeInteractionCounter;
    public int HandInteractionCounter;
    public bool IsEvalValid;
    public int ParticipantID;

    private int prevTutValue;
    private int demoMissionIndex;
    private string baseSolutionTextValue;
    private ActiveMissionData[] EvaluationMissionSet;
    private int EvaluationMissionIndex;
    private float timer;
    private string baseInactiveTextValue;

    #region unity Functions
    // Start is called before the first frame update
    void Start()
    {
        TutorialLogic = GameMaster.instance.TutorialUILogicObject;
        GlanceableLogic = GameMaster.instance.GlanceableLogic;
        demoMissionIndex = 0;
        baseSolutionTextValue = SolutionTextObject.text;
        baseInactiveTextValue = InactiveMissionTextObject.text;

        CompletedDemos = 0;

        EvaluationMissionSet = new ActiveMissionData[10];
        EvaluationMissionIndex = 0;

        CompletionTimes = new float[10];
        MissionErrors = new int[10];
        InterfaceSwitches = new int[10];
        ParticipantID = -1;

        ResetTabletop();

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(prevTutValue != TutorialLogic.TutoriaIndex)
        {
            ResetTabletop();
        }

        prevTutValue = TutorialLogic.TutoriaIndex;
    }

    #endregion

    #region Tabletop States
    public void ResetTabletop()
    {
        if (TutorialLogic.TutoriaIndex != 0)
        {
            state = TabletopStates.Inactive;
            InterfaceHighlightObject.SetActive(true);

            GlanceableLogic.DisableAllGlanceable();
            ButtonUIObject.SetActive(false);
            foreach (GameObject go in GetMissionButtons) go.SetActive(false);
            InactiveMissionUIObject.SetActive(false);
            ActiveMissionUIObject.SetActive(false);
            AbandonMissionButton.SetActive(false);
            EvaluationCompleteInterface.SetActive(false);

            Tools3DObjects.SetActive(false);
            MaterialsMilling3DObjects.SetActive(false);
            MaterialsTurning3DObjects.SetActive(false);
            SolutionsMilling3DObjects.SetActive(false);
            SoltutionsTurning3DObjects.SetActive(false);
}
        else
        {
            InterfaceHighlightObject.SetActive(false);

            if (ActiveMission.Template == null)
            {
                state = TabletopStates.NoMission;

                GlanceableLogic.DisableAllGlanceable();
                ButtonUIObject.SetActive(true);
                foreach (GameObject go in GetMissionButtons) go.SetActive(true);
                InactiveMissionUIObject.SetActive(true);
                ActiveMissionUIObject.SetActive(false);
                AbandonMissionButton.SetActive(false);
                EvaluationCompleteInterface.SetActive(false);

                Tools3DObjects.SetActive(false);
                MaterialsMilling3DObjects.SetActive(false);
                MaterialsTurning3DObjects.SetActive(false);
                SolutionsMilling3DObjects.SetActive(false);
                SoltutionsTurning3DObjects.SetActive(false);

                if (ParticipantID < 0)
                {
                    InactiveMissionTextObject.text = baseInactiveTextValue;
                }
                else
                {
                    InactiveMissionTextObject.text = baseInactiveTextValue + "\n Your ParticipantID is: <b>" + ParticipantID + "</b>";
                }
            }
            else
            {
                GlanceableLogic.EnableSimpleGlanceable();
                //2D objects
                ButtonUIObject.SetActive(true);
                foreach (GameObject go in GetMissionButtons) go.SetActive(false);
                InactiveMissionUIObject.SetActive(false);

                if (ActiveMission.IsSingleMission)
                {
                    state = TabletopStates.SingleMission;
                    EvaluationCompleteInterface.SetActive(false);
                    ActiveMissionUIObject.SetActive(true);
                    AbandonMissionButton.SetActive(true);

                }
                else
                {
                    state = TabletopStates.Evaluation;
                    if (EvaluationMissionIndex < 10)
                    {
                        EvaluationCompleteInterface.SetActive(false);
                        ActiveMissionUIObject.SetActive(true);
                    }
                    else
                    {
                        EvaluationCompleteInterface.SetActive(true);
                        ActiveMissionUIObject.SetActive(false);
                    }
                    AbandonMissionButton.SetActive(false);
                }
                    
                //3D objects
                Tools3DObjects.SetActive(true);
                if (ActiveMission.Template.MissionType == MissionTypes.Milling)
                {
                    MaterialsMilling3DObjects.SetActive(true);
                    SolutionsMilling3DObjects.SetActive(true);

                    MaterialsTurning3DObjects.SetActive(false);
                    SoltutionsTurning3DObjects.SetActive(false);
                }
                else
                {
                    MaterialsMilling3DObjects.SetActive(false);
                    SolutionsMilling3DObjects.SetActive(false);

                    MaterialsTurning3DObjects.SetActive(true);
                    SoltutionsTurning3DObjects.SetActive(true);
                }

            }
         
        }
    }
    

    public void RepositionTabletop()
    {
        TutorialLogic.ShowTutorial(3);
    }

    public void RepositionHolobox()
    {
        TutorialLogic.ShowTutorial(4);
    }

    #endregion

    #region Mision Generator
    public void GenerateDemoMission()
    {
        int missionNumber = demoMissionIndex % DemoMissionScriptableData.Length;

        ActiveMission = CreateActiveMission(DemoMissionScriptableData[missionNumber], true);

        demoMissionIndex++;

        ResetTabletop();

    }

    public void GenerateEvalMissionSet()
    {
        //First 4 missions linear
        for(int i=0; i < 4; i++)
        {
            EvaluationMissionSet[i] = CreateActiveMission(EvalMissionScriptableData[i], false);
        }

        //Next 4 random but one of each mission
        //add
        for (int i = 4; i < 8; i++)
        {
            EvaluationMissionSet[i] = CreateActiveMission(EvalMissionScriptableData[i-4], false);
        }
        //shuffle
        for (int i=4; i < 8; i++)
        {
            ActiveMissionData temp = EvaluationMissionSet[i];
            int randomIndex = Random.Range(i, 8);
            EvaluationMissionSet[i] = EvaluationMissionSet[randomIndex];
            EvaluationMissionSet[randomIndex] = temp;
        }

        //Final 2 fully random
        for(int i = 8; i < 10; i++)
        {
            EvaluationMissionSet[i] = CreateActiveMission(EvalMissionScriptableData[Random.Range(0, 4)], false);
        }

        EvaluationMissionIndex = -1;

        //Reset metrics
        CompletionTimes = new float[10];
        MissionErrors = new int[10];
        InterfaceSwitches = new int[10];
        for (int i=0; i < 10; i++)
        {
            CompletionTimes[i] = 0;
            MissionErrors[i] = 0;
            InterfaceSwitches[i] = 0;
        }
        EyeInteractionCounter = 0;
        HandInteractionCounter = 0;
        IsEvalValid = false;
        ParticipantID = -1;

    NextEvalStep();

    }

    public void NextEvalStep()
    {
        EvaluationMissionIndex++;
        if (EvaluationMissionIndex >=0 && EvaluationMissionIndex < 10)
        {
            timer = 0;

            DeselectMaterial();
            DeselectProduct();
            DeselectTool();
            ActiveMission = EvaluationMissionSet[EvaluationMissionIndex];

        }
        else
        {
            //ActiveMission.Template = null;
        }
        ResetTabletop();
    }

    public void SubmitEval(bool IsRecorded)
    {
        IsEvalValid = IsRecorded;

        //Read CSV
        SaveResults();

        ActiveMission.Template = null;
        ResetTabletop();
    }

    public ActiveMissionData CreateActiveMission(MissionData template, bool isSingleMission)
    {
        ActiveMissionData am = new ActiveMissionData();
        am.Template = template;
        am.IsSingleMission = isSingleMission;

        int matNo = Random.Range(1, 5);
        if (matNo == 1)
        {
            am.MissionMaterial = MaterialIndexes.Aluminum;
        }
        else if (matNo == 2)
        {
            am.MissionMaterial = MaterialIndexes.Copper;
        }
        else if (matNo == 3)
        {
            am.MissionMaterial = MaterialIndexes.Stainless;
        }
        else
        {
            am.MissionMaterial = MaterialIndexes.Steel;
        }

        return am;

    }

    #endregion

    #region Item Selections

    public void SetTool (GameObject Object, ToolIndexes Index)
    {
        if (SelectedTool != null) DeselectTool();
        SelectedTool = Object;
        SelectedToolType = Index;
    }

    public void SetMaterial(GameObject Object, MaterialIndexes Index)
    {
        if (SelectedMaterial != null) DeselectMaterial();
        SelectedMaterial = Object;
        SelectedMaterialType = Index;
    }

    public void SetProduct(GameObject Object, MissionIndexes Index)
    {
        if (SelectedProduct != null) DeselectProduct();
        SelectedProduct = Object;
        SelectedProductType = Index;
    }

    public void DeselectTool()
    {
        if (SelectedTool!=null) SelectedTool.GetComponent<ManufacturingSelectionLogic>().Deselect();
        SelectedTool = null;
    }

    public void DeselectMaterial()
    {
        if (SelectedMaterial != null)  SelectedMaterial.GetComponent<ManufacturingSelectionLogic>().Deselect();
        SelectedMaterial = null;
    }

    public void DeselectProduct()
    {
        if (SelectedProduct != null) SelectedProduct.GetComponent<ManufacturingSelectionLogic>().Deselect();
        SelectedProduct = null;
    }

    #endregion

    #region Mission Solution

    public void CheckSolution()
    {
        string errormsg = "";
        if (SelectedTool == null) errormsg = "No Tool selected";
        else if (SelectedMaterial == null) errormsg = "No Material selected";
        else if (SelectedProduct == null) errormsg = "No Finished product selected";
        else if (SelectedToolType != ActiveMission.Template.RequiredTools[0]) errormsg = "Wrong Tool selected";
        else if (SelectedMaterialType != ActiveMission.MissionMaterial) errormsg = "Wrong Material selected";
        else if (SelectedProductType != ActiveMission.Template.BaseMissionIndex) errormsg = "Wrong Finished product selected";

        if(errormsg == "")
        {
            if (ActiveMission.IsSingleMission)
            {
                SolutionTextObject.text = baseSolutionTextValue;
                ActiveMission.Template = null;
                CompletedDemos++;
                ResetTabletop();
            }
            else
            {
                //TODO if evaluation sequence = true
                SolutionTextObject.text = baseSolutionTextValue;
                LogMissionTimer();
                NextEvalStep();
            }
        }
        else
        {
            AddError();
            SolutionTextObject.text = baseSolutionTextValue + "\n <b><color=red>" + errormsg + "</color><b>";

        }
    }

    public void AbandonMission()
    {
        ActiveMission.Template = null;
        ResetTabletop();
    }

    #endregion

    #region Player Metrics
    public void AddError()
    {
        if(EvaluationMissionIndex >= 0 && EvaluationMissionIndex < 10 && !ActiveMission.IsSingleMission)
        {
            MissionErrors[EvaluationMissionIndex]++;
        }
    }

    public void LogMissionTimer()
    {
        if (EvaluationMissionIndex >= 0 && EvaluationMissionIndex < 10 && !ActiveMission.IsSingleMission)
        {
            CompletionTimes[EvaluationMissionIndex]= timer;
        }
    }

    public void AddInterfaceSwitch()
    {
        if (EvaluationMissionIndex >= 0 && EvaluationMissionIndex < 10 && !ActiveMission.IsSingleMission)
        {
            InterfaceSwitches[EvaluationMissionIndex]++;
        }
    }

    #endregion

    #region File manager

    public List<string> LoadResults()
    {
        string fileName = "Results.csv";

        if (!Directory.Exists(Application.persistentDataPath + "/Saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");
        }

        string path = Application.persistentDataPath + "/Saves/" + fileName;

        List<string> ret = new List<string>();

        if (File.Exists(path))
        {
            //read file
            StreamReader stream = new StreamReader(path);


            while (!stream.EndOfStream)
            {
                ret.Add(stream.ReadLine());
            }

            stream.Close();
            return ret;

        }
        else
        {
            string firstLine = "Participant ID,Experiment success,No of Tutorials complete,"+
                "Complete Time 1sec,Complete Time 2sec,Complete Time 3sec,Complete Time 4sec,Complete Time 5sec," +
                "Complete Time 6sec,Complete Time 7sec,Complete Time 8sec,Complete Time 9sec,Complete Time 10sec," +
                "Interface Switches 1,Interface Switches 2,Interface Switches 3,Interface Switches 4,Interface Switches 5,"+
                "Interface Switches 6,Interface Switches 7,Interface Switches 8,Interface Switches 9,Interface Switches 10,"+
                "Errors 1,Errors 2,Errors 3,Errors 4,Errors 5,Errors 6,Errors 7,Errors 8,Errors 9,Errors 10,"+
                "No of Eye Interacts,No of Hand Interacts";
            ret.Add(firstLine);
            return ret;
        }
    }

    public void SaveResults()
    {
        List<string> curFile = LoadResults();

        ParticipantID = curFile.Count;

        //serialise new entry
        string newEntry = "";
        newEntry += ParticipantID + "," + IsEvalValid + "," + CompletedDemos + ",";
        for (int i = 0; i < CompletionTimes.Length; i++)
        {
            newEntry += CompletionTimes[i] + ",";
        }
        for (int i = 0; i < InterfaceSwitches.Length; i++)
        {
            newEntry += InterfaceSwitches[i] + ",";
        }
        for (int i = 0; i < MissionErrors.Length; i++)
        {
            newEntry += MissionErrors[i] + ",";
        }
        newEntry += EyeInteractionCounter + ",";
        newEntry += HandInteractionCounter;

        curFile.Add(newEntry);

        //Save file
        string fileName = "Results.csv";

        if (!Directory.Exists(Application.persistentDataPath + "/Saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");
        }

        string path = Application.persistentDataPath + "/Saves/" + fileName;

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        StreamWriter stream = new StreamWriter(path);

        foreach (string line in curFile)
        {
            stream.WriteLine(line);
        }

        stream.Close();
    }

    #endregion
}

public enum TabletopStates
{
    Inactive,
    NoMission,
    SingleMission,
    Evaluation
}

[System.Serializable]
public class ActiveMissionData
{
    public MissionData Template;
    public bool IsSingleMission;
    public MaterialIndexes MissionMaterial;
    public string MissionMaterialText
    {
        get
        {
            if (MissionMaterial == MaterialIndexes.Aluminum) return "Aluminum";
            else if (MissionMaterial == MaterialIndexes.Copper) return "Copper";
            else if (MissionMaterial == MaterialIndexes.Stainless) return "Stainless";
            else if (MissionMaterial == MaterialIndexes.Steel) return "Steel";
            else return "";
        }
    }
}
