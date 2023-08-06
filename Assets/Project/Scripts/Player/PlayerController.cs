using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float rotSpeed = 100f;

    [SerializeField] private Transform ikTarget;
    [SerializeField] private bool activeIK;


    private bool dying = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(dying)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("StandUp");
                dying = false;
            }
        }
        else
        {
            float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            float rotation = Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;

            transform.Rotate(0, rotation, 0);

            animator.SetBool("isRun", move != 0);

            if(Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Jumping");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard") && !dying)
        {
            animator.SetTrigger("Dying");
            dying = true;
        }
    }


    private void OnAnimatorIK(int layerIndex)
    {
        if(activeIK) {
            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(ikTarget.position);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, ikTarget.position);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, ikTarget.position);
        }
    }
}
