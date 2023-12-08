using UnityEngine;

[CreateAssetMenu(menuName = "Scene Connection")]
public class SceneConnection : ScriptableObject
{
    public static SceneConnection ActiveConnection { get; set; }
}