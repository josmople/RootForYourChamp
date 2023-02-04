using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMP = TMPro;
using UI = UnityEngine.UI;

public class ManagerGame : MonoBehaviour {
    public Settings Settings;
    public StatesVar States;
    public UI::Image Volume;
    public UI::Image Health;
    public TMP::TMP_Text StateText;

    private Game _currentGame;
    private Inputs _inputs;

    void Start() {
        _inputs = new Inputs();
        _inputs.Enable();

        _currentGame = Game.FromSettings(Settings, States.Value, Time.time);
        Volume.transform.localScale = new Vector3(1f, 0.5f, 1f);
        Health.transform.localScale = new Vector3(1f, 0.5f, 1f);
        NewState(_currentGame.CurrentState);
    }

    void Update() {
        // Check player accuracy
        if (_currentGame.CurrentState is FightingState && _currentGame.LastPolledTimeSecs + Settings.PollingIntervalSecs < Time.time) {
            var lowTarget = _currentGame.CurrentTargetVolume - Settings.TolerancePercent;
            var highTarget = _currentGame.CurrentTargetVolume + Settings.TolerancePercent;

            var volume = GetVolumePercent();
            if (lowTarget <= volume && volume <= highTarget) {
                _currentGame.HealthPercent += Settings.AccurateGainPercent;
            } else {
                _currentGame.HealthPercent -= Settings.InaccurateCostPercent;
            }

            if (_currentGame.HealthPercent <= 0) {
                _currentGame.CurrentStateIdx--;

                if (_currentGame.CurrentState is EndState) {
                    //
                }

                NewState(_currentGame.CurrentState);


            } else if (_currentGame.HealthPercent >= 100f) {
                _currentGame.CurrentStateIdx++;

                if (_currentGame.CurrentState is EndState) {
                    //
                }

                NewState(_currentGame.CurrentState);
            }

            _currentGame.LastPolledTimeSecs = Time.time;
            Volume.transform.localScale = new Vector3(1f, volume / 100f, 1f);
            Health.transform.localScale = new Vector3(1f, _currentGame.HealthPercent / 100f, 1f);
        }
    }

    private void NewState (State state) {
        StateText.text = state.Text;
        _currentGame.HealthPercent = Settings.StartingHealthPercent;

        // Play animation
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
