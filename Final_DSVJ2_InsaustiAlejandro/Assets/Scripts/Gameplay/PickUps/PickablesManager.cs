public class PickablesManager : MonoBehaviourSingletonInScene<PickablesManager>
{
    LevelManager levelManager;

    //Unity Events
    private void Start()
    {
        levelManager = LevelManager.Get();
    }

    //Methods
    public void OnScorePickedUp(float value)
    {
        levelManager.score += (int)value;
    }
}