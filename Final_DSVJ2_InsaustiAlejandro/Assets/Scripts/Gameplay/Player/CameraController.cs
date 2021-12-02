using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("X=Min; Y=Max")]
    [SerializeField] Vector2 followRange;
    [SerializeField] Transform player;

    //Unity Events
    private void Start()
    {
        //Get player
        if(!player)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        //first followup
        if (!player) return;
        FollowPlayer();
    }
    private void Update()
    {
        if (!player) return;
        FollowPlayer();
    }

    //Methods
    void FollowPlayer()
    {
        if (Mathf.Abs(player.position.z - transform.position.z) < followRange.x)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, GetMinZAxis());
        }
        else if (Mathf.Abs(player.position.z - transform.position.z) > followRange.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, GetMaxZAxis());
        }
    }
    float GetMaxZAxis()
    {
        if (player.position.z > transform.position.z + followRange.y)
        {
            return player.position.z - followRange.y;
        }
        else if (player.position.z < transform.position.z - followRange.y)
        {
            return player.position.z + followRange.y;
        }

        return transform.position.z;
    }
    float GetMinZAxis()
    {
        return player.position.z - followRange.x;
    }
}