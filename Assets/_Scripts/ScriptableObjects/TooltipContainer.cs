using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LoadingScreen/ToolTipManager", fileName = "ToolTipManager")]
public class ToolTipManager : ScriptableObject
{
    [field: SerializeField] public string[] toolTipsArray { private set; get; }
}
