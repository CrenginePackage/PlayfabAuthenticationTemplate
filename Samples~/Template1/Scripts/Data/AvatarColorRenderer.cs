using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarColorRenderer : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer upperColorRenderer;
    [SerializeField] private MeshRenderer[] upper;
    [SerializeField] private MeshRenderer[] lower;


    public void ChangeUpperColor(CustomizeColor _customizeColor)
    {
        if (upperColorRenderer == null)
        {
            for (int i = 0; i < upper.Length; i++)
            {
                //upper[i].material.SetColor("_BaseColor", ConvertColor(_customizeColor));
                //upper[i].material.color = ConvertColor(_customizeColor);
                upper[i].sharedMaterial.color = ConvertColor(_customizeColor);
            }
            return;
        }
        Debug.Log("Change Color: " + _customizeColor);
        upperColorRenderer.material.SetColor("_BaseColor", ConvertColor(_customizeColor));
    }


    private Color ConvertColor(CustomizeColor _customizeColor)
    {
        switch (_customizeColor)
        {
            case CustomizeColor.Yellow:
                return Color.yellow;
            case CustomizeColor.Red:
                return Color.red;
            case CustomizeColor.Blue:
                return Color.blue;
            case CustomizeColor.Green:
                return Color.green;
            case CustomizeColor.White:
                return Color.white;
            default:
                Debug.LogError("Unknown Color. Returning White.");
                goto case CustomizeColor.White;
        }
    }
}
