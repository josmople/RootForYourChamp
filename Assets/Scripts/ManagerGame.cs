using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMP = TMPro;
using UI = UnityEngine.UI;

public class ManagerGame : MonoBehaviour {
    public ManagerSound SoundManager;
    public Settings Settings;
    public StatesVar States;
    public UI::Image Volume;
    public UI::Image Health;
    public TMP::TMP_Text StateText;
    public Animator Animator;
    public AudioDetection audioDetector;
    public AudioClip MusicSound;
    public AudioClip CrowdSound;
    public List<AudioClip> BackgroundSounds;

    public ResizingUI hpBar;
    public ResizingUI volumeBar;

    public GameObject humanWin;
    public GameObject robotWin;
    public GameObject quit;
    

    private Game _currentGame;
    private Inputs _inputs;

    void Start() {
        _inputs = new Inputs();
        _inputs.Enable();

        var states = States.Value ?? new States {
            StartingIdx = 2,
            // When skipping calibration during testing
            StateTargetVolumes = Settings.States
                .Select(state => (state, Random.Range(0f, 100f)))
                .ToList(),
        };

        _currentGame = Game.FromSettings(Settings, states, Time.time);
        Volume.transform.localScale = new Vector3(1f, 0.5f, 1f);
        
        Health.transform.localScale = new Vector3(1f, 0.5f, 1f);
        NewState(_currentGame.CurrentState);
        SoundManager.PlayMusic(MusicSound);
        SoundManager.PlayCrowd(CrowdSound);

        humanWin.SetActive(false);
        robotWin.SetActive(false);
        quit.SetActive(false);
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
                    robotWin.SetActive(true);
                    quit.SetActive(true);
                }

                NewState(_currentGame.CurrentState);


            } else if (_currentGame.HealthPercent >= 100f) {
                _currentGame.CurrentStateIdx++;

                if (_currentGame.CurrentState is EndState) {
                    humanWin.SetActive(true);
                    quit.SetActive(true);
                }

                NewState(_currentGame.CurrentState);
            }

            _currentGame.LastPolledTimeSecs = Time.time;
            volumeBar.interpolation = volume / 100f;
            //Volume.transform.localScale = new Vector3(1f, volume / 100f, 1f);
            hpBar.interpolation = _currentGame.HealthPercent / 100f;
            //Health.transform.localScale = new Vector3(_currentGame.HealthPercent / 100f, 1f, 1f);
        }
    }

    private void NewState (State state) {
        StateText.text = state.Text;
        _currentGame.HealthPercent = Settings.StartingHealthPercent;
        Animator.Play(state.Text);
    }


    public float volumeSmoothing = 0.1f;
    public float volumeCached = 0;
    public float volumeMultiplier = 100;
    public float volumeExpAdjustmennt = 1.2f;

    private float GetVolumePercent () {
        if (_inputs._.Low.ReadValue<float>() != 0f) {
            return 20f;
        } else if (_inputs._.Medium.ReadValue<float>() != 0f) {
            return 50f;
        } else if (_inputs._.High.ReadValue<float>() != 0f) {
            return 80f;
        }

        var target = Mathf.Pow(audioDetector.FindVolume(), volumeExpAdjustmennt) * volumeMultiplier;
        volumeCached = Mathf.Lerp(volumeCached, target, volumeSmoothing);
        return Mathf.Clamp(volumeCached, 0, 100);
    }
}
