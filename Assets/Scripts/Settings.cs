using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Settings")]
public class Settings : ScriptableObject {
    public float LengthSecs;
    public float PollingIntervalSecs;
    public float AccurateGainPercent;
    public float TolerancePercent;
    public float InaccurateCostPercent;
    public float StartingHealthPercent;
}
