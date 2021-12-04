﻿using System;
using System.Collections;
using UnityEngine;

public class FrogController : MonoBehaviour, IHittable
{
    public Action Died;
    public float mapLimit;
    [SerializeField] LayerMask obstacleLayers;
    [SerializeField] LayerMask pickableLayers;
    [SerializeField] LayerMask notWalkableLayers;
    [SerializeField] LayerMask platformLayers;
    [SerializeField] float rotateSpeed;
    [SerializeField] float movementSpeed;
    Transform platformTransform;
    Quaternion directionRotation;
    Vector3 direction;
    IEnumerator rotationRoutine;
    float platformDistance;
    float movementTimer;
    float rotationTimer;
    bool alive = true;

    //Unity Events
    void Start()
    {
        directionRotation = transform.localRotation;
    }
    private void Update()
    {
        if (!alive) return;

        GetDirection();
        StartCoroutine(Move());
    }
    void LateUpdate()
    {
        if (platformTransform && movementTimer == 0)
        {
            Vector3 newPos = transform.position;
            newPos.x = platformTransform.position.x + platformDistance;
            transform.position = newPos;
        }
    }

    //Methods
    public void Respawn()
    {
        Debug.Log("Player re-spawned");
        alive = true;

        direction = Vector3.zero;
        movementTimer = 0;
        rotationTimer = 0;
    }
    void GetDirection()
    {
        if (direction != Vector3.zero) return;
        direction = GetMovement().normalized;
    }
    IEnumerator Move()
    {
        //If there is no direction, return
        if (direction == Vector3.zero) yield break; //if movement is null, return
        if (movementTimer != 0) yield break; //if another coroutine in progress, return

        //Get positions
        Vector3 movement = direction;
        Vector3 originalPos = transform.position;
        Vector3 targetPos = transform.position + movement;

        //If moving in new direction, rotate
        Quaternion newDirection = Quaternion.LookRotation(direction, transform.up);
        if (newDirection != directionRotation)
        {
            StopCoroutine(rotationRoutine);
            directionRotation = newDirection;
            rotationTimer = 0;
        }
        rotationRoutine = Turn();
        StartCoroutine(rotationRoutine);

        //If movement is posible, move
        if (MovementBlocked(movement))
        {
            direction = Vector3.zero;
            yield break;
        }

        //Set Timer
        float timerMax = (originalPos - targetPos).magnitude;
        movementTimer = timerMax;

        //Move in direction
        do
        {
            movementTimer -= Time.deltaTime * movementSpeed;
            transform.position = Vector3.Lerp(originalPos, targetPos, timerMax - movementTimer);
            yield return null;
        } while (movementTimer > 0);

        //Reset walking variables
        direction = Vector3.zero;
        movementTimer = 0;

        //Check if player is still standing above floor
        if (IsAboveWater())
        {
            Died.Invoke();
            yield break;
        }

        //Pick up anything below player
        PickPickables();
        yield break;
    }
    Vector3 GetMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Return only 1 direction
        if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
        {
            return new Vector3(horizontalInput, 0, 0);
        }
        else
        {
            return new Vector3(0, 0, verticalInput);
        }
    }
    bool MovementBlocked(Vector3 direction)
    {
        //Set Variables
        Vector3 pos = transform.position + direction;
        float radius = transform.localScale.z / 2;

        //Check for map limits and obstacles
        if (pos.x + radius > mapLimit) return true;
        if (pos.x - radius < -mapLimit) return true;
        return Physics.OverlapSphere(pos, radius, obstacleLayers).Length > 0;
    }
    bool IsAboveWater()
    {
        RaycastHit platform;
        if (Physics.Raycast(transform.position, -transform.up, out platform, 10, platformLayers))
        {
            platformTransform = platform.transform;
            platformDistance = transform.position.x - platformTransform.position.x;
            return false;
        }
        else
        {
            platformTransform = null;
        }
        return Physics.Raycast(transform.position, -transform.up, 10, notWalkableLayers);
    }
    void PickPickables()
    {
        //Set Variables
        Vector3 pos = transform.position;
        float radius = transform.localScale.z / 2;

        //Pick Pickables
        Collider[] pickables = Physics.OverlapSphere(pos, radius, pickableLayers);
        if (pickables.Length < 1) return;

        //Hit Pickables
        foreach (var pickable in pickables)
        {
            //If collider has IHittable, Hit
            pickable.GetComponent<IHittable>()?.GetHitted();
        }
    }
    IEnumerator Turn()
    {
        //Return if already in a loop or if direction is null
        if (rotationTimer > 0) yield break;

        //Set Rotation
        Quaternion originalRotation = transform.localRotation;

        //Set Timer
        float timerMax = Quaternion.Angle(originalRotation, directionRotation);
        rotationTimer = timerMax;

        //Rotate in direction
        do
        {
            rotationTimer -= Time.deltaTime * rotateSpeed;
            transform.localRotation = Quaternion.Lerp(originalRotation, directionRotation, timerMax - rotationTimer);
            yield return null;
        } while (rotationTimer > 0);
        yield break;
    }

    //Interface Implementations
    public void GetHitted()
    {
        Debug.Log("Player died");
        alive = false;
        Died.Invoke();
    }
}