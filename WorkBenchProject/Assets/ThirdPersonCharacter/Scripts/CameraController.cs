using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
   
    public float camaraMouseSpeed = 1;
    public float cameraControllerSpeed = 0.5f;
    public float followSpeed = 9;
    public float minAngle = -35f;
    public float maxAngle = 35;

    private float verticalCamera;
    private float horizontalCamera;

    private InputHandler inputHandler;

    private float turnSmoothing = .1f;
    private float smoothX;
    private float smoothY;
    private float smoothXvelocity;
    private float smoothYvelocity;
    private float lookAngle;
    private float tiltAngle;
    
    [HideInInspector]
    public Transform pivot;
    [HideInInspector]
    public Transform camTrans;

    void Start()
    {
        camTrans = Camera.main.transform;
        pivot = camTrans.parent;
        inputHandler = target.GetComponent<InputHandler>();
    }

    void FixedUpdate()
    {
        horizontalCamera = inputHandler.mosueInput.x;
        verticalCamera = inputHandler.mosueInput.y;

        float horizontalCameraController = inputHandler.rightAxis.x;
        float verticalCameraController = inputHandler.rightAxis.y;
        float targetSpeed = camaraMouseSpeed;

        if ((verticalCameraController != 0) || (horizontalCameraController != 0))
        {
            verticalCamera = verticalCameraController;
            horizontalCamera = horizontalCameraController;
            targetSpeed = cameraControllerSpeed;
        }

        FollowTarget(Time.deltaTime);
        HanldeRotation(Time.deltaTime, verticalCamera, horizontalCamera, targetSpeed);
    }

    private void FollowTarget(float d)
    {
        float speed = d * followSpeed;
        Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, speed);
        transform.position = targetPosition;
    }

    private void HanldeRotation(float d, float vertical, float horizontal, float targetSpeed) 
    {
        if (turnSmoothing > 0)
        {
            smoothX = Mathf.SmoothDamp(smoothX, horizontal, ref smoothXvelocity, turnSmoothing);
            smoothY = Mathf.SmoothDamp(smoothY, vertical, ref smoothYvelocity, turnSmoothing);
        }
        else
        {
            smoothX = horizontal;
            smoothY = vertical;
        }

        lookAngle += smoothX * targetSpeed;
        transform.rotation = Quaternion.Euler(0, lookAngle, 0);
        tiltAngle -= smoothY * targetSpeed;
        tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);
        pivot.localRotation = Quaternion.Euler(tiltAngle,0,0);
    }
}
