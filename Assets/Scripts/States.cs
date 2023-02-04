using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Odin = Sirenix.OdinInspector;

[CreateAssetMenu(menuName="States")]
public class States : Odin::SerializedScriptableObject {
    public int StartingIdx;
    public List<(State, float)> StateTargetVolumes;
}
