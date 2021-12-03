using System;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    public Action PlayerReachedLastArea;
	[SerializeField] Transform player;
	[SerializeField] Transform[] areasTransform;
	List<float> areas;
    int highestArea;

    //Unity Events
    private void Start()
    {
        areas = new List<float>();

        foreach (var area in areasTransform)
        {
            areas.Add(area.position.z);
        }
    }
    private void LateUpdate()
    {
        if (highestArea >= areas.Count) return;

        while (player.transform.position.z > areas[highestArea])
        {
            Debug.Log("Player get passed spawn point " + (highestArea + 1));
            highestArea++;
            if (highestArea >= areas.Count)
            {
                PlayerReachedLastArea.Invoke();
                break;
            }
        }
    }

    //Methods
    public float GetHighestAreaPosition()
    {
        return areas[highestArea];
    }
}