using System.Collections;
using System.Collections.Generic;
using Odin = Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName="Settings")]
public class Settings : Odin::SerializedScriptableObject {
    public float LengthSecs;
    public float PollingIntervalSecs;
    public float AccurateGainPercent;
    public float TolerancePercent;
    public float InaccurateCostPercent;
    public float StartingHealthPercent;
    public float MusicMaxVolume;
    public float SoundMaxVolume;
    public int StartingIdx;
    public List<State> States;
}
