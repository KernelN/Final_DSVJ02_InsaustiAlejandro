using System;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Action<PlatformController> Died;
    public float mapLimit;
    [SerializeField] LayerMask obstacleLayers;
    [SerializeField] LayerMask hittableLayers;
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
        if (movement.z == 0) return;
        transform.position += movement;

        //whole enemy is beyond right limit
        if (transform.localPosition.x - transform.localScale.z > mapLimit)
        {
            Died.Invoke(this);
        }
        //whole enemy is beyond left limit
        else if (transform.localPosition.x + transform.localScale.z < -mapLimit)
        {
            Died.Invoke(this);
        }
    }
    Vector3 GetMovement()
    {
        return transform.forward * speed * Time.deltaTime;
    }
}