using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMEnemy : MonoBehaviour {

    // point that enemy have to reach
    public Transform[] points;
    // patrol time
    [Range(0.1f, 5f)] public float FSMDelay;
    // time to wait before move to the next point
    [Range(1f, 10f)] public float idleTime;

    FSM fsmMachine;
    // check if player is chought
    bool isEnd;

    void Start() {
        // StartFSM();
    }

    /*public void StartFSM() {

        // Define states and link actions when enter/exit/stay
        FSMState idleAction = new FSMState {
            enterActions = new FSMAction[] { WaitBeforeMove },
            exitActions = new FSMAction[] { NextPosition }
        };

        FSMState moveAction = new FSMState {
            enterActions = new FSMAction[] { Move }
        };

        FSMState seekAction = new FSMState {
            enterActions = new FSMAction[] { FindPosition },
            stayActions = new FSMAction[] { CheckPosition }
        };

        FSMState catchAction = new FSMState {
            enterActions = new FSMAction[] { EndLevel }
        };

        // Define transitions
        FSMTransition fromIdleToMove = new FSMTransition(CheckTimeToMove);
        FSMTransition fromMoveToIdle = new FSMTransition(CheckRechedPosition);
        FSMTransition fromIdleToCatch = new FSMTransition(CheckPlayer);
        FSMTransition fromIdleToSeek = new FSMTransition(CheckNearObject);
        FSMTransition fromSeekToIdle = new FSMTransition(CheckReachedObject);
        FSMTransition fromSeekToCatch = new FSMTransition(CheckPlayer);
        FSMTransition fromMoveToSeek = new FSMTransition(CheckNearObject);
        FSMTransition fromMoveToCatch = new FSMTransition(CheckPlayer);

        // Link states with transitions
        idleAction.AddTransition(fromIdleToMove, moveAction);
        idleAction.AddTransition(fromIdleToCatch, catchAction);
        idleAction.AddTransition(fromIdleToSeek, seekAction);
        moveAction.AddTransition(fromMoveToIdle, idleAction);
        moveAction.AddTransition(fromMoveToSeek, seekAction);
        moveAction.AddTransition(fromMoveToCatch, catchAction);
        seekAction.AddTransition(fromSeekToIdle, idleAction);
        seekAction.AddTransition(fromSeekToCatch, catchAction);

        // Setup a FSA at initial state
        fsmMachine = new FSM(idleAction);
        // Start monitoring
        StartCoroutine(PatrolFSM());
    }*/

    // Periodic update, run forever
    IEnumerator PatrolFSM() {
        while (!isEnd) {
            fsmMachine.Update();
            yield return new WaitForSeconds(FSMDelay);
        }
    }
}
