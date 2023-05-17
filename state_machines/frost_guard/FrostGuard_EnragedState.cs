using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostGuard_EnragedState : StateMachineBehaviour
{
    public float speed;
    public float attackRange;
    public float abilityRange;
    private int switch_attack;
    private int switch_attack_2;
    private Vector3 previousPosition;
    private Vector3 lastPosition;

    Transform player;
    Rigidbody2D rb;
    BossController bossController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        bossController = animator.GetComponent<BossController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossController.TargetedQuaternion(player.position);

        // move toward player
        if (!bossController.hitWall) {
            // move toward player
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, Time.fixedDeltaTime * speed);
            rb.MovePosition(newPos);
        } else {
            rb.velocity = new Vector2(rb.transform.right.x * 10f, 20f);
        }

        // attack
        if (Vector2.Distance(player.position, rb.position) <= attackRange) {
            if (switch_attack == 0) {
                animator.SetTrigger("triggerMelee");
                switch_attack = 1;
            } else if (switch_attack == 1) {
                animator.SetTrigger("triggerAbility2");
                switch_attack = 0;
            }
        } else if (Vector2.Distance(player.position, rb.position) > attackRange + 8
        && Vector2.Distance(player.position, rb.position) < abilityRange) {
            if (switch_attack_2 == 0) {
                animator.SetTrigger("triggerDash");
                switch_attack_2 = 1;
            } else if (switch_attack_2 == 1) {
                animator.SetTrigger("triggerAbility");
                switch_attack_2 = 0;
            }
        }
    }
    
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("triggerAbility");
        animator.ResetTrigger("triggerAbility2");
        animator.ResetTrigger("triggerDash");
        animator.ResetTrigger("triggerMelee");
    }
}
