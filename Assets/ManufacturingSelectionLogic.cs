using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManufacturingSelectionLogic : MonoBehaviour
{
    public ToolIndexes toolIndex;
    public MaterialIndexes materialIndex;
    public MissionIndexes missionIndex;

    public GameObject SelectionIdentifierObject;

    private void Start()
    {
        Deselect();
    }

    public void SetSelectedTool()
    { 
        GameMaster.instance.GlanceableLogic.Tabletop.GetComponent<EvaluationBoxLogic>().SetTool(this.gameObject, toolIndex);
        SelectionIdentifierObject.SetActive(true);
    }

    public void SetSelectedMaterial()
    {
        GameMaster.instance.GlanceableLogic.Tabletop.GetComponent<EvaluationBoxLogic>().SetMaterial(this.gameObject, materialIndex);
        SelectionIdentifierObject.SetActive(true);
    }

    public void SetSelectedProduct()
    {
        GameMaster.instance.GlanceableLogic.Tabletop.GetComponent<EvaluationBoxLogic>().SetProduct(this.gameObject, missionIndex);
        SelectionIdentifierObject.SetActive(true);
    }

    public void Deselect()
    {
        SelectionIdentifierObject.SetActive(false);
    }

}
