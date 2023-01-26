using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMachine : MonoBehaviour
{

    BaseState currentState;

    void Start()
    {
        currentState = GetInitialState();

    }

    void Update()
    {
        currentState?.UpdateLogic();
    }

    void LateUpdate()
    {
        currentState?.UpdatePhysics();
    }

    public void ChangeState(BaseState newState)
    {
        currentState?.Exit();

        currentState = newState;
        currentState?.Enter();
    }

    protected virtual BaseState GetInitialState() { return null; }


    //Debug
    private void OnGUI()
    {
        string content = currentState != null ? currentState.GetName() : "unnamed";
        GUILayout.Label($"<color='black'><size='20'{content}</size></color>");
    }

}
