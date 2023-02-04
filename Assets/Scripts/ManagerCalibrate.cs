using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMP = TMPro;
using UI = UnityEngine.UI;

public class ManagerCalibrate : MonoBehaviour {
    public StatesVar StatesVar;
    public Settings Settings;
    public UI::Image Volume;
    public TMP::TMP_Text StateText;

    private int _calibrateStateIdx = 0;
    private Inputs _inputs;
    private States _states;

    void Start() {
        _inputs = new Inputs();
        _inputs.Enable();

        _states = new States () {
            StartingIdx = Settings.StartingIdx,
            StateTargetVolumes = new List<(State, float)>(),
        };
    }

    void Update() {
        var volume = GetVolumePercent();
        Volume.transform.localScale = new Vector3(volume / 100f, 1f, 1f);

        while (_calibrateStateIdx < Settings.States.Count && Settings.States[_calibrateStateIdx] is EndState) {
            _states.StateTargetVolumes.Add((Settings.States[_calibrateStateIdx], 0f));
            _calibrateStateIdx++;
        }

        // Done
        if (_calibrateStateIdx == Settings.States.Count) {
            StatesVar.Value = _states;
            SceneManager.LoadScene("Game");

        } else if (_calibrateStateIdx < Settings.States.Count && _inputs._.Confirm.WasReleasedThisFrame()) {
            // TODO:
            _states.StateTargetVolumes.Add((Settings.States[_calibrateStateIdx], volume));
            _calibrateStateIdx++;
        }

        if (_calibrateStateIdx < Settings.States.Count) {
            StateText.text = Settings.States[_calibrateStateIdx].Text;
        }
    }

    private float GetVolumePercent () {
        if (_inputs._.Low.ReadValue<float>() != 0f) {
            return 20f;
        } else if (_inputs._.Medium.ReadValue<float>() != 0f) {
            return 50f;
        } else if (_inputs._.High.ReadValue<float>() != 0f) {
            return 80f;
        }

        return 0.0f;
    }
}
