using System;
using Rewired;
using Unity.Mathematics;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    public PickerTool pickerTool;
    public float moveSpeed;
    public Vector2 bounds;
    
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
        if(player == null || 
           !pickerTool) return;

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
        heading *= Time.deltaTime;
        
        pickerTool.transform.position += heading;

        //Bind the arm within a certain bounds
        var xBoundsHalved = bounds.x * 0.5f;
        var yBoundsHalved = bounds.y * 0.5f;
        var localPickerPosition = pickerTool.transform.localPosition;
        localPickerPosition = new Vector3(
            Mathf.Clamp(localPickerPosition.x, -xBoundsHalved, xBoundsHalved),
            localPickerPosition.y, 
            Mathf.Clamp(localPickerPosition.z, -yBoundsHalved, yBoundsHalved));

        pickerTool.transform.localPosition = localPickerPosition;
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
                
                if(pickerTool) pickerTool.PickUp();
                hasActionTriggered = true;
            }
        }
        else
        {
            hasActionTriggered = false;
        }

        lastActionAxisValue = axis;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        var position = transform.position;
        position.y = position.y - 5;
        Gizmos.DrawWireCube(position, new Vector3(bounds.x, 10, bounds.y));
    }
}
