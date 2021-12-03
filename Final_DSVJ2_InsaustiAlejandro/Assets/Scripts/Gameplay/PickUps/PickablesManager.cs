using System;

public class PickablesManager : MonoBehaviourSingletonInScene<PickablesManager>
{
	public Action<int> ScoreChanged;
	
	//Unity Events

	//Methods
	public void OnScorePickedUp(float value)
    {
		ScoreChanged.Invoke((int)value);
    }
}