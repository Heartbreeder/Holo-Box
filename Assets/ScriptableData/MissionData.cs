using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="MissionDataTemplate")]
public class MissionData : ScriptableObject
{
    public string MissionTitle;
    public MissionTypes MissionType;
    public MissionIndexes BaseMissionIndex;
    public string ObjectSizeText;
    public string CarvingDepthText;
    public ToolIndexes[] RequiredTools;
    public Sprite MissionBlueprint;

    public string RequiredTool
    {
        get
        {
            if (RequiredTools.Length < 1) return "";

            if (RequiredTools[0] == ToolIndexes.Cartilage) return "Cartilage Tool";
            else if (RequiredTools[0] == ToolIndexes.Completion) return "Completion Tool";
            else if (RequiredTools[0] == ToolIndexes.Cutting) return "Cutting Tool";
            else if (RequiredTools[0] == ToolIndexes.Tuberous10mm) return "Tuberous Tool Φ10";
            else if (RequiredTools[0] == ToolIndexes.Tuberous3mm) return "Tuberous Tool Φ3";
            else return "";
        }
    }

    public string GetRequiredToolText(int index)
    {
        if (RequiredTools.Length >= index || index < 0) return null;

        switch (RequiredTools[index])
        {
            case ToolIndexes.Cartilage:
                return "Cartilage Tool";
            case ToolIndexes.Completion:
                return "Completion Tool";
            case ToolIndexes.Cutting:
                return "Cutting Tool";
            case ToolIndexes.Tuberous10mm:
                return "Tuberous Tool Φ10";
            case ToolIndexes.Tuberous3mm:
                return "Tuberous Tool Φ3";
        }
        return null;
    }

    public static string GetMaterialText(MaterialIndexes mat)
    {
        switch (mat)
        {
            case MaterialIndexes.Aluminum:
                return "Aluminum";
            case MaterialIndexes.Copper:
                return "Copper";
            case MaterialIndexes.Stainless:
                return "Stainless Steel";
            case MaterialIndexes.Steel:
                return "Steel";
        }
        return null;
    }

}

public enum MissionIndexes
{
    M1,
    M3,
    M9,
    M14,
    M33,
    M37,
    M44,
    M46
}

public enum ToolIndexes
{
    Cartilage,
    Completion,
    Cutting,
    Tuberous3mm,
    Tuberous10mm
}

public enum MaterialIndexes
{
    Copper,
    Aluminum,
    Steel,
    Stainless
}

public enum MissionTypes
{
    Milling,
    Turning
}