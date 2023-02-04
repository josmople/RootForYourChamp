using System;
using System.Collections.Generic;
using UnityEngine;
using Odin = Sirenix.OdinInspector;

public interface Var<A> {
    A Value {
        get;
        set;
    }

    A InitialValue {
        get;
    }
}

public abstract class UnityVar<A> : Odin::SerializedScriptableObject, Var<A> {
    [SerializeField]
    private A value;

    [SerializeField]
    private A initialValue;

    public virtual void OnEnable () {
        Value = InitialValue;
    }

    public virtual A InitialValue {
        get { return initialValue; }
    }

    public virtual A Value {
        get { return value; }
        set { this.value = value; }
    }
}
