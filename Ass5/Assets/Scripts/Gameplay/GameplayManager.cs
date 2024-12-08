using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    public GameObject player;

    private CharacterType[] types;
    private float timeSinceLastSpawn;
    private float spawnInterval = 3;
    private int maxSpawn = 1;
    private int enemySpawned;
    private List<GameObject> enemies;

    [SerializeField]
    private List<GameObject> spawnLocations;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        timeSinceLastSpawn = spawnInterval;
        enemySpawned = 0;

        types = new CharacterType[4];
        types[0] = CharacterType.Warrior;
        types[1] = CharacterType.Rogue;
        types[2] = CharacterType.Minion;
        types[3] = CharacterType.Mage;

        GameObject prefab = Resources.Load<GameObject>("Prefabs/Characters/Knight");
        player = Instantiate(prefab);

        enemies = new List<GameObject>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        if (timeSinceLastSpawn >= spawnInterval && enemySpawned < maxSpawn)
        {
            int enemyTypeIdx = 0;
            int locationIdx = 0;

            GameObject prefab = Resources.Load<GameObject>("Prefabs/Enemies/" + types[enemyTypeIdx].ToString());
            GameObject enemy = Instantiate(prefab, spawnLocations[locationIdx].transform.position, spawnLocations[locationIdx].transform.rotation);
            enemies.Add(enemy);
            timeSinceLastSpawn = 0;
            enemySpawned++;
        }
    }
}
