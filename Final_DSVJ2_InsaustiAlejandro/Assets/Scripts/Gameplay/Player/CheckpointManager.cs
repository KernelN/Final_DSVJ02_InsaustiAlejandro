using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Action PlayerReachedLastArea;
	[SerializeField] Transform player;
	[SerializeField] Transform[] checkpointTransforms;
	List<float> checkpoints;
    int currentPoint;
    int highestPoint;

    //Unity Events
    private void Start()
    {
        checkpoints = new List<float>();

        foreach (var checkpoint in checkpointTransforms)
        {
            checkpoints.Add(checkpoint.position.z);
        }
    }
    private void LateUpdate()
    {
        if (currentPoint >= checkpoints.Count) return;

        int newPoint = currentPoint > 0 ? currentPoint - 1 : 0; //make min point reachable
        for (int i = newPoint; i < checkpoints.Count; i++)
        {
            //if player doesn't reach area, return
            if (player.transform.position.z < checkpoints[i]) return; 

            //Update current area
            currentPoint = i;

            //if current area is before highest area, return
            if (currentPoint <= highestPoint) continue;
         
            //Update Highest area
            Debug.Log("Player passed through spawn point " + (currentPoint + 1));
            highestPoint = currentPoint;

            //if current area isn't the last one, return
            if (highestPoint < checkpoints.Count - 1) return;

            //Send action
            PlayerReachedLastArea.Invoke();
            break;
        }
    }

    //Methods
    public float GetCurrentCheckpointPosition()
    {
        return checkpoints[currentPoint];
    }
    public float GetHighestCheckpointPosition()
    {
        return checkpoints[highestPoint];
    }
}