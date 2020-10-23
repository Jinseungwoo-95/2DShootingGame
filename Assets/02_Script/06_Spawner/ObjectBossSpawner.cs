using UnityEngine;

public class ObjectBossSpawner : MonoBehaviour
{
    public BossSpawnerData bossSpawnerData;
    public GridController grid;
    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject lootDrop;

    void Start()
    {
        grid = GetComponentInChildren<GridController>();
    }

    public void BossDeath()
    {
        portal.SetActive(true);
        lootDrop.SetActive(true);
    }

    public void InitialiseObjectSpawning()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        GameObject go = Instantiate(bossSpawnerData.BossSpawn[GameController.instance.StageLevel - 1], grid.availablePoints[96], Quaternion.identity, transform) as GameObject;
    }
}