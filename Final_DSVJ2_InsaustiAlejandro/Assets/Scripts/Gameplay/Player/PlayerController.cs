using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IHittable
{
    public float mapLimit;
	[SerializeField] LayerMask collisionLayers;
	[SerializeField] float rotateSpeed;
	[SerializeField] float speed;
    Quaternion direction;
    float rotationTimer;


    //Unity Events
    void Start()
    {
        direction = transform.localRotation;
    }
    private void Update()
    {
        Move();
    }
    
    //Methods
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
        float radius = transform.localScale.x / 2;

        //Check for map limits and obstacles
        if (pos.x + radius > mapLimit) return true;
        if (pos.x - radius < -mapLimit) return true;
        return Physics.OverlapSphere(pos, radius, collisionLayers).Length > 0;
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
        GetComponent<SphereCollider>().enabled = false;
    }
}