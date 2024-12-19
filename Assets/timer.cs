using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timer : StateMachineBehaviour
{
    private float elapsedTime; // Tracks elapsed time

    // Called when the state is first entered
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        elapsedTime = 0f; // Reset the timer
        Debug.Log($"Animation {stateInfo.shortNameHash} started.");
    }

    // Called every frame the animation is playing
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        elapsedTime += Time.deltaTime; // Increment the timer
        //Debug.Log($"Animation {stateInfo.shortNameHash} playing for {elapsedTime:F2} seconds.");
    }

    // Called when the state exits
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log($"Animation {stateInfo.shortNameHash} ended after {elapsedTime:F2} seconds.");
    }
}
