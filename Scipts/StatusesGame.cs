using UnityEngine;


[CreateAssetMenu]
public class StatusesGame : ScriptableObject
{
    public readonly IStateGame PauseGame = new PauseGame();
    public readonly IStateGame FrozenGame = new FrozenGame();
    public readonly IStateGame RunningGame = new RunningGame();

}
