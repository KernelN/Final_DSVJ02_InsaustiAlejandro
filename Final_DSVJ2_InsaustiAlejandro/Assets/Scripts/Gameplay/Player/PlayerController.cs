using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] LayerMask collisionLayers;
	[SerializeField] float speed;

    //#region Unity Events
    private void Update()
    {
        Move();
    }
    //#endregion

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
        Vector3 pos = transform.position + direction;
        float radius = transform.localScale.x / 2;
        return Physics.OverlapSphere(pos, radius, collisionLayers).Length > 0;
    }
}