using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    public GameObject player;

    private CharacterType[] types = { CharacterType.Warrior, CharacterType.Rogue, CharacterType.Minion, CharacterType.Mage };
    private float timeSinceLastEnemySpawn;
    private float enemySpawnInterval = 3;
    private int maxSpawn;
    private int enemySpawned;
    private List<GameObject> enemies;
    private int level;

    [SerializeField]
    private List<GameObject> spawnLocations;

    [SerializeField]
    private GameObject itemPrefab;
    private List<GameObject> items;
    private float timeSinceLastItemSpawn;
    private float itemSpawnInterval = 10;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        //level = GameManager.Instance.Level;

        //GameObject prefab = Resources.Load<GameObject>("Prefabs/Characters/" + GameManager.Instance.CharacterType);

        level = 1;

        GameObject prefab = Resources.Load<GameObject>("Prefabs/Characters/Knight");
        player = Instantiate(prefab);

        enemies = new List<GameObject>();
        maxSpawn = 5 * (level + 1);
        timeSinceLastEnemySpawn = enemySpawnInterval;
        enemySpawned = 0;

        items = new List<GameObject>();
        timeSinceLastItemSpawn = itemSpawnInterval;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastEnemySpawn += Time.deltaTime;
        timeSinceLastItemSpawn += Time.deltaTime;
        //SpawnEnemy();
        SpawnItem();
    }

    void SpawnEnemy()
    {
        if (timeSinceLastEnemySpawn >= enemySpawnInterval && enemySpawned < maxSpawn)
        {
            int enemyTypeIdx = Random.Range(0, types.Length);
            int locationIdx = Random.Range(0, spawnLocations.Count);

            GameObject prefab = Resources.Load<GameObject>("Prefabs/Enemies/" + types[enemyTypeIdx].ToString());
            GameObject enemy = Instantiate(prefab, spawnLocations[locationIdx].transform.position, spawnLocations[locationIdx].transform.rotation);
            enemies.Add(enemy);
            timeSinceLastEnemySpawn = 0;
            enemySpawned++;
        }
    }

    void SpawnItem()
    {
        if (timeSinceLastItemSpawn >= itemSpawnInterval)
        {
            float x = Random.Range(-27, 45);
            float z = Random.Range(-29, 19);
            GameObject item = Instantiate(itemPrefab, new Vector3(x, 0, z), itemPrefab.transform.rotation);
            items.Add(item);
            timeSinceLastItemSpawn = 0;
        }
    }
}
