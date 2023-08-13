using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float rotSpeed = 100f;

    [SerializeField] private Transform rightHand;

    [SerializeField] private Transform ikTarget;
    [SerializeField] private bool activeIK;


    private bool dying = false;

    private bool isHanging = false;
    private bool hangingTrigger = true;
    private Transform rootTarget;

    public float movX;
    public float movY;


    [SerializeField] private Transform item;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rootTarget = null;
        hangingTrigger = true;
    }

    private void FixedUpdate()
    {
        if (!rootTarget) return;
        if (isHanging && hangingTrigger)
        {
            transform.position = new Vector3(transform.position.x, rootTarget.position.y, rootTarget.position.z);
            transform.rotation = Quaternion.Euler(transform.rotation.x, rootTarget.rotation.y, transform.rotation.z);
            hangingTrigger = false;
        }

    }

    void Update()
    {
        movY = Input.GetAxis("Vertical");
        movX = Input.GetAxis("Horizontal");
        animator.SetFloat("X", movX, 0.1f, Time.deltaTime);
        animator.SetFloat("Y", movY, 0.1f, Time.deltaTime);

        if (dying)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("StandUp");
                dying = false;
            }
            animator.SetBool("RightShimmy", false);
            animator.SetBool("LeftShimmy", false);
            animator.SetBool("isRun", false);
        }
        else
        {
            float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            float rotation = Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
            if (isHanging)
            {
                animator.SetBool("RightShimmy", rotation > 0);
                animator.SetBool("LeftShimmy", rotation < 0);

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    StartCoroutine("GoingUp");
                }
            }
            else
            {
                //transform.Rotate(0, rotation, 0);

                animator.SetBool("isRun", move != 0);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animator.SetTrigger("Jumping");
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                animator.SetTrigger("PickUp");
            }
        }
    }

    private IEnumerator GoingUp()
    {
        animator.SetTrigger("GoingUp");
        yield return new WaitForSeconds(3.24f);
        isHanging = false;
        hangingTrigger = true;
        rb.isKinematic = false;
    }

    public void Hanging(Transform target)
    {
        if (isHanging) return;
        animator.SetTrigger("Hanging");
        isHanging = true;
        rb.isKinematic = true;
        rootTarget = target;

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
        if (activeIK)
        {
            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(ikTarget.position);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, ikTarget.position);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, ikTarget.position);
        }

        animator.SetLookAtWeight(animator.GetFloat("IK_Val"));
        animator.SetLookAtPosition(item.position);

        if (animator.GetFloat("IK_Val") > 0.9f)
        {
            item.parent = rightHand;
            item.localPosition = new Vector3(-0.046f, 0.102f, 0.043f);
        }

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, animator.GetFloat("IK_Val"));
        animator.SetIKPosition(AvatarIKGoal.RightHand, item.position);
    }
}