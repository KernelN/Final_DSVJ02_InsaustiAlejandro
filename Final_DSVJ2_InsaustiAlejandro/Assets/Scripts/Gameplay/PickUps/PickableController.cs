using System;
using UnityEngine;

public class PickableController : MonoBehaviour, IHittable
{
    public float publicValue { get { return value; } }
	[SerializeField] PickablesManager manager;
	[SerializeField] float value;
    Action<float> PickedUp;

    //Unity Events
    private void Start()
    {
        if (!manager)
        {
            manager = PickablesManager.Get();
        }

        PickedUp += manager.OnScorePickedUp;
    }
    void OnDestroy()
    {
        PickedUp -= manager.OnScorePickedUp;
    }

    //Interface Implementations
    public void GetHitted()
    {
        PickedUp.Invoke(value);
        Destroy(gameObject);
    }
}