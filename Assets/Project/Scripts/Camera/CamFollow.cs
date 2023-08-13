using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private Transform[] pos;
    [SerializeField] private int id;
    [SerializeField] private Vector3 speed = Vector3.zero;
    private RaycastHit hit;
    private float rotSpeed = 100;
    private float rotation;
    [SerializeField] private Transform player;
    private PlayerController playerController;
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    private void LateUpdate()
    {
        transform.LookAt(head);
        if(!Physics.Linecast(head.position, pos[id].position))
        {
            transform.position = Vector3.SmoothDamp(transform.position, pos[id].position, ref speed, 0.4f);
            Debug.DrawLine(transform.position, head.position, Color.red);
        }else if (Physics.Linecast(head.position, pos[id].position, out hit))
        {
            transform.position = Vector3.SmoothDamp(transform.position, hit.point, ref speed, 0.4f);
            Debug.DrawLine(transform.position, head.position, Color.red);
        }
    }

    void Update()
    {
        AdjustCamera();

        if (Mathf.Abs(playerController.movY) == 0)
        {
            RotationCamera(head);
        }
        else
        {
            RotationCamera(player);
        }
        
    }

    private void AdjustCamera()
    {
        if (Input.GetButtonDown("CameraAdjust"))
        {
            if (id < pos.Length - 1)
            {
                id++;
            }
            else
            {
                id = 0;
            }
        }
    }

    private void RotationCamera(Transform obj)
    {
        rotation = Input.GetAxis("CameraRot") * rotSpeed * Time.deltaTime;
        obj.Rotate(0, rotation, 0);
    }
}
