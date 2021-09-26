using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{

    private Camera mainCamera;
    private bool isDragging;
    private Rigidbody2D currentBallRigitbody;
    private SpringJoint2D currentBallSprinJoint;

    [SerializeField] private float detachDelay = 0.5f;
    [SerializeField] private float respownDelay;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRigitbody == null)
        {
            return;
        }

        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            isDragging = true;
            currentBallRigitbody.isKinematic = true;
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
            currentBallRigitbody.position = worldPosition;            
        }
        else
        {
            if (isDragging)
            {
                LaunchBall();
            }
            isDragging = false;
            
            return;
        }
        
    }

    private void LaunchBall()
    {
        currentBallRigitbody.isKinematic = false;
        currentBallRigitbody = null;
        Invoke(nameof(DetachBall), detachDelay);
    }

    private void DetachBall()
    {
        currentBallSprinJoint.enabled = false;
        currentBallSprinJoint = null;
        Invoke(nameof(SpawnNewBall), respownDelay);
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);
        currentBallRigitbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSprinJoint = ballInstance.GetComponent<SpringJoint2D>();
        currentBallSprinJoint.connectedBody = pivot;
    }

}
