using Rewired;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    public Transform controller;
    public float moveSpeed;
    
    [Header("AxisNames")]
    public string horizontalAxisName;
    public string verticalAxisName;
    public string actionAxisName;

    private Transform forwardTransform;
    private Player player;
    private float lastActionAxisValue;
    private bool isActionDown;
    private bool hasActionTriggered;

    private void Start()
    {
        var camera = Camera.main;
        forwardTransform = camera != null ? camera.transform : transform;
        player = ReInput.players.GetPlayer(0);
    }

    private void Update()
    {
        if(player == null) return;

        MoveArm(new Vector2(player.GetAxis(horizontalAxisName), player.GetAxis(verticalAxisName)));
        DoAction(player.GetAxis(actionAxisName));
    }

    private void MoveArm(Vector2 axis)
    {
        var right = forwardTransform.right;
        var forward = forwardTransform.forward;

        right *= axis.x;
        forward *= axis.y;

        var heading = right + forward;
        heading.y = 0;
        heading *= moveSpeed;

        controller.position += heading * Time.deltaTime;
    }

    private void DoAction(float axis)
    {
        isActionDown = axis > 0.1f;

        if (isActionDown)
        {
            if (!hasActionTriggered &&
                axis < lastActionAxisValue)
            {
                Debug.Log(name + " trigger up");
                hasActionTriggered = true;
            }
        }
        else
        {
            hasActionTriggered = false;
        }

        lastActionAxisValue = axis;
    }
}
