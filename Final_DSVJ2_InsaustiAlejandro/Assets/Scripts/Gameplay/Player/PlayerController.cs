using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IHittable
{
    public Action Died;
    public float mapLimit;
	[SerializeField] LayerMask obstacleLayers;
	[SerializeField] LayerMask pickableLayers;
	[SerializeField] float rotateSpeed;
	[SerializeField] float speed;
    Quaternion direction;
    float rotationTimer;
    bool alive = true;

    //Unity Events
    void Start()
    {
        direction = transform.localRotation;
    }
    private void Update()
    {
        if (!alive) return;

        Move();
    }

    //Methods
    public void Respawn()
    {
        Debug.Log("Player re-spawned");
        alive = true;
    }
    void Move()
    {
        //Get original movement
        Vector3 movement = GetMovement();
        if (movement.x == 0 && movement.z == 0) return; //if movement is null, return

        //If moving in new direction, rotate
        Quaternion newDirection = Quaternion.LookRotation(movement.normalized, transform.up);
        if (newDirection != direction)
        {
            direction = newDirection;
            rotationTimer = 0;
            StopAllCoroutines();
        }
        StartCoroutine(Turn());

        //If movement is posible, move
        if (MovementBlocked(movement)) return; 
        transform.localPosition += movement;
        PickPickables();
    }
    Vector3 GetMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        return new Vector3(horizontalInput, 0, verticalInput) * speed * Time.deltaTime;
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
        float timerMax = Quaternion.Angle(originalRotation, direction);
        rotationTimer = timerMax;

        //Rotate in direction
        do
        {
            rotationTimer -= Time.deltaTime * rotateSpeed;
            transform.localRotation = Quaternion.Lerp(originalRotation, direction, timerMax - rotationTimer);
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