using UnityEngine;

public class PlayerController : MonoBehaviour, IHittable
{
    public float mapLimit;
	[SerializeField] LayerMask collisionLayers;
	[SerializeField] float speed;

    //Unity Events
    private void Update()
    {
        Move();
    }
    
    //Methods
    void Move()
    {
        Vector3 movement = GetMovement();
        if (movement.x == 0 && movement.z == 0) return;
        if (MovementBlocked(movement)) return;
        transform.Translate(movement);
    }
    Vector3 GetMovement()
    {
        float horizontalMove = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float verticalMove = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        return new Vector3(horizontalMove, 0, verticalMove);
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

    //Interface Implementations
    public void GetHitted()
    {
        Debug.Log("Player died");
        GetComponent<SphereCollider>().enabled = false;
    }
}