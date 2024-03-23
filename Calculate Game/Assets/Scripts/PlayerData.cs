using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public int Coin;
    public int BestScore;

    public int Item1 = 5;
    public int Item2 = 5;
    public int Item3 = 5;
    public int Item4 = 5;
}
