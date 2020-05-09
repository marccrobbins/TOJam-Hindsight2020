using System;
using Rewired;
using UnityEngine;

public class GameInputController : MonoBehaviour
{
    private const InputActionEventType BUTTON_TYPE = InputActionEventType.ButtonJustReleased;
    private const InputActionEventType AXIS_TYPE = InputActionEventType.AxisActiveOrJustInactive;

    [SerializeField] private Transform leftArmController;
    [SerializeField] private Transform rightArmController;
    [SerializeField] private float moveSpeed;

    private Camera camera;
    private Player player;
    private bool isLeftTriggerDown;
    private bool hasLeftTriggered;
    private float currentLeftTriggerAxis;
    private float lastLeftTriggerAxis;
    private bool isRightTriggerDown;
    private bool hasRightTriggered;
    private float currentRightTriggerAxis;
    private float lastRightTriggerAxis;

    private void Start()
    {
        camera = Camera.main;
        
        player = ReInput.players.GetPlayer(0);

        if (player == null) return;
     
        player.AddInputEventDelegate(OnLeftTrigger, UpdateLoopType.Update, AXIS_TYPE, "LeftTriggerAction");
        player.AddInputEventDelegate(OnRightTrigger, UpdateLoopType.Update, AXIS_TYPE, "RightTriggerAction");
    }

    #region InputCallbackLogin

    private void Update()
    {
        if (player == null) return;

        var leftHorizontalAxis = player.GetAxis("LeftJoystickHorizontal");
        var leftVerticalAxis = player.GetAxis("LeftJoystickVertical");
        var rightHorizontalAxis = player.GetAxis("RightJoystickHorizontal");
        var rightVerticalAxis = player.GetAxis("RightJoystickVertical");
        
        MoveArm(ArmType.LeftArm, new Vector2(leftHorizontalAxis, leftVerticalAxis));
        MoveArm(ArmType.RightArm, new Vector2(rightHorizontalAxis, rightVerticalAxis));
    }

    private void OnLeftTrigger(InputActionEventData data)
    {
        currentLeftTriggerAxis = data.GetAxis();
        isLeftTriggerDown = currentLeftTriggerAxis > 0.1f;

        if (isLeftTriggerDown)
        {
            if (!hasLeftTriggered &&
                currentLeftTriggerAxis < lastLeftTriggerAxis)
            {
                Debug.Log("left trigger up");
                hasLeftTriggered = true;
            }
        }
        else
        {
            hasLeftTriggered = false;
        }

        lastLeftTriggerAxis = currentLeftTriggerAxis;
    }
    
    private void OnRightTrigger(InputActionEventData data)
    {
        currentRightTriggerAxis = data.GetAxis();
        isRightTriggerDown = currentRightTriggerAxis > 0.1f;

        if (isRightTriggerDown)
        {
            if (!hasRightTriggered &&
                currentRightTriggerAxis < lastRightTriggerAxis)
            {
                Debug.Log("right trigger up");
                hasRightTriggered = true;
            }
        }
        else
        {
            hasRightTriggered = false;
        }

        lastRightTriggerAxis = currentRightTriggerAxis;
    }
    
    #endregion InputCallbackLogin

    private void MoveArm(ArmType arm, Vector2 axis)
    {
        var controller = arm == ArmType.LeftArm ? leftArmController : rightArmController;

        var forwardTransform = camera ? camera.transform : transform;
        var right = forwardTransform.right;
        var forward = forwardTransform.forward;

        right *= axis.x;
        forward *= axis.y;

        var heading = right + forward;
        heading.y = 0;
        heading *= moveSpeed;

        controller.position += heading * Time.deltaTime;
    }
    
    //ToDo send in which arm to let go
    private void DropItem()
    {
        
    }
}

public enum ArmType
{
    LeftArm,
    RightArm
}
