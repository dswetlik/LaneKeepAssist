using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HapticFeedback
{
    public class ExecuteState : BaseState
    {

        protected StateMachine _stateMachine;

        public ExecuteState(StateMachine stateMachine) : base("Execute", stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public override void Enter()
        {
            base.Enter();


        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();


        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }

        public override void Exit()
        {
            base.Exit();
        }
   
    }
}
