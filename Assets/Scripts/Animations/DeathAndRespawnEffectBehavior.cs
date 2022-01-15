using Characters.Herochar;
using UnityEngine;

namespace Animations
{
    public class DeathAndRespawnEffectBehavior : StateMachineBehaviour
    {
        public HerocharController heroController;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        // override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        // {
        // }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        // {
        //
        // }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.gameObject.SetActive(false);
            heroController.gameObject.SetActive(true);
            if (heroController.IsDeath)
            {
                heroController.IsDeath = false;
                heroController.DeathEvent.Invoke();
            }
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}

    }

}
