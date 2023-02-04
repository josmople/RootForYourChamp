using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State {
    public string Text;
}

public class FightingState : State { }

public class EndState : State { }

public class States  {
    public int StartingIdx;
    public List<(State, float)> StateTargetVolumes;
}


public class Game {
    public int CurrentStateIdx;
    public States States;
    public float StateTimeLeft;
    public float LastPolledTimeSecs;
    public float HealthPercent;

    public State CurrentState {
        get {
            return States.StateTargetVolumes[CurrentStateIdx].Item1;
        }
    }

    public float CurrentTargetVolume {
        get {
            return States.StateTargetVolumes[CurrentStateIdx].Item2;
        }
    }

    public static Game FromSettings (Settings settings, States states, float nowSecs) {
        return new Game {
            CurrentStateIdx = states.StartingIdx,
            States = states,
            StateTimeLeft = settings.LengthSecs,
            LastPolledTimeSecs = nowSecs,
            HealthPercent = settings.StartingHealthPercent,
        };
    }
}
