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

        //First followup
        if (!player) return;
        FollowPlayer();

        //Link Actions
        player.GetComponent<FrogController>().Died += OnPlayerDeath;
    }
    private void LateUpdate()
    {
        if (!player) return;
        FollowPlayer();
    }

    //Methods
    void GoToMinDistance()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, GetMinZAxis());
    }
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

    //Event Receivers
    void OnPlayerDeath()
    {
        GoToMinDistance();
    }
}