using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Action<EnemyController> Died;
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
        if (MovementBlocked(movement)) return;
        HitObjects(movement);
        transform.position += movement;

        //whole enemy is beyond right limit
        if (transform.localPosition.x - transform.localScale.z> mapLimit) 
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
    bool MovementBlocked(Vector3 direction)
    {
        Vector3 pos = transform.position + direction;
        Vector3 radius = transform.localScale / 2;
        Quaternion rotation = transform.rotation;
        return Physics.OverlapBox(pos, radius, rotation, obstacleLayers).Length > 0;
    }
    void HitObjects(Vector3 direction)
    {
        Vector3 pos = transform.position + direction; //remove direction? could collide before expected
        Vector3 radius = transform.localScale / 2;
        Quaternion rotation = transform.rotation;
        Collider[] colls = Physics.OverlapBox(pos, radius, rotation, hittableLayers);
        IHittable hit;
        foreach (Collider col in colls)
        {
            hit = col.GetComponent<IHittable>();
            if (hit == null) continue; //if hitted component doesn't have IHittable, skip
            hit.GetHitted();
        }
    }
}