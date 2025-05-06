using Enemy.StateMachine;
using System;
using System.Collections.Generic;

namespace Infrastructure.States
{
    public class GameStateMachine
    {
        private Dictionary<Type, IEnemyState> _states;
        private IEnemyState _activeState;

        public void Initialize(Dictionary<Type, IEnemyState> states)
        {
            _states = states;
        }

        public void Enter<TState>() where TState : class, IEnemyState
        {
            IEnemyState state = ChangeState<TState>();
            state.Enter();
        }

        public void Update()
        {
            _activeState.Update();
        }

        private TState ChangeState<TState>() where TState : class, IEnemyState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IEnemyState =>
            _states[typeof(TState)] as TState;

    }
}
