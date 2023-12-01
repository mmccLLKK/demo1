using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInit : MonoBehaviour
{
    private AIFSM _aifsm;

    public TriggerMsg patrolTrigger;
    public TriggerMsg trackTrigger;

    private void Awake()
    {
        _aifsm = GetComponent<AIFSM>();
        Role role = GetComponent<Role>();
        List<(AIStateType, StateMachineBase)> state = new List<(AIStateType, StateMachineBase)>();
        state.Add((AIStateType.patrol, new PatrolStateMachine()));
        state.Add((AIStateType.track, new TrackStateMachine()));
        state.Add((AIStateType.strike, new StrikeStateMachine()));

        foreach (var valueTuple in state)
        {
            var aiStateMachineBase = valueTuple.Item2 as AIStateMachineBase;
            aiStateMachineBase.aifsm = _aifsm;
            aiStateMachineBase.role = role;
        }

        List<(AIStateType, AIStateType)> trans = new List<(AIStateType, AIStateType)>();
        trans.Add((AIStateType.patrol, AIStateType.track));
        trans.Add((AIStateType.track, AIStateType.strike));
        trans.Add((AIStateType.track, AIStateType.patrol));
        trans.Add((AIStateType.strike, AIStateType.track));

        _aifsm.SetStatesInfo(state, trans);

        _aifsm.SetState(AIStateType.patrol, true);

        patrolTrigger.onEntry = enemy =>
        {
            var roleInput = enemy.GetComponent<RoleInput>();
            if (roleInput == null)
            {
                return;
            }

            if (_aifsm.IsInState(AIStateType.patrol))
            {
                _aifsm.SetState(AIStateType.track);
                var stateMachineBase = _aifsm.GetStateInst(AIStateType.track) as TrackStateMachine;
                if (stateMachineBase != null)
                {
                    stateMachineBase.enemy = enemy;
                }
            }
        };
        trackTrigger.onEntry = enemy => { _aifsm.SetState(AIStateType.strike); };
        trackTrigger.onStay = enemy =>
        {
            if (_aifsm.IsInState(AIStateType.strike))
            {
                return;
            }

            _aifsm.SetState(AIStateType.strike);
        };
    }
}