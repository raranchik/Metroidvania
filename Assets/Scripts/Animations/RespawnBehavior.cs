using Characters.Herochar;
using UnityEngine;

namespace Animations
{
    public class RespawnBehavior : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GameObject gameObject = animator.gameObject;
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            CapsuleCollider2D col = gameObject.GetComponent<CapsuleCollider2D>();
            col.enabled = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        // {
        //
        // }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GameObject gameObject = animator.gameObject;

            HerocharController heroController = gameObject.GetComponent<HerocharController>();
            heroController.IsDeath = heroController.IsRespawn = false;

            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            CapsuleCollider2D col = gameObject.GetComponent<CapsuleCollider2D>();
            col.enabled = true;
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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
