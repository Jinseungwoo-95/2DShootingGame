using UnityEngine;

[CreateAssetMenu(fileName = "BossSpawner.asset", menuName = "Spawners/BossSpawner")]
public class BossSpawnerData : ScriptableObject
{
    public GameObject[] BossSpawn;
    public int minSpawn;
    public int maxSpawn;
}