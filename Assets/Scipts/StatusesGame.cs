using UnityEngine;


[CreateAssetMenu]
public class StatusesGame : ScriptableObject
{
    public readonly IPlayMode MenuMode = new MenuGame();
    public readonly IPlayMode PlayMode = new PlayingMode();

}
