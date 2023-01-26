using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    [SerializeField] string _name;
    protected BaseMachine _bStateMachine;

    public BaseState(string name, BaseMachine stateMachine)
    {
        this._name = name;
        this._bStateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }

    public string GetName() { return _name; }

}
