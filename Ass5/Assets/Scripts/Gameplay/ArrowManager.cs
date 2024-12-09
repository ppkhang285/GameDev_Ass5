using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public static ArrowManager Instance { get; private set; }

    [SerializeField]
    private GameObject arrowPrefab;
    private List<Arrow> arrows = new List<Arrow>();
    private Queue<GameObject> arrowPool = new Queue<GameObject>();
    public Dictionary<GameObject, Arrow> arrowMap = new Dictionary<GameObject, Arrow>();

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
    }

    // Update is called once per frame
    void Update()
    {
        UpdateArrows();
    }

    private void UpdateArrows()
    {
        List<GameObject> arrowsToDeactivate = new List<GameObject>();

        foreach (var kvp in arrowMap)
        {
            GameObject arrowObject = kvp.Key;
            Arrow arrow = kvp.Value;

            if (arrow.IsActive)
            {
                arrow.UpdatePosition(Time.deltaTime);
                arrowObject.transform.position = arrow.Position;
            }
            else
                arrowsToDeactivate.Add(arrowObject);
        }

        foreach (var arrowObject in arrowsToDeactivate)
        {
            DeactivateArrow(arrowObject);
        }
    }

    public GameObject SpawnArrow(Archer archer)
    {
        GameObject arrowObject = GetArrowFromPool();
        arrowObject.SetActive(true);

        Vector3 direction = archer.transform.forward;
        Vector3 position = archer.transform.position;
        float range = archer.Stats.attackRange;
        Arrow arrow = new Arrow(archer, position, direction, range);
        arrows.Add(arrow);

        arrowMap[arrowObject] = arrow;
        return arrowObject;
    }

    private GameObject GetArrowFromPool()
    {
        if (arrowPool.Count > 0 && !arrowPool.Peek().activeInHierarchy)
        {
            return arrowPool.Dequeue();
        }

        GameObject newArrow = Instantiate(arrowPrefab, transform.position, transform.rotation);
        arrowPool.Enqueue(newArrow);
        return newArrow;
    }

    public void DeactivateArrow(GameObject arrowObject)
    {
        arrowObject.SetActive(false);
        arrowPool.Enqueue(arrowObject);
        arrowMap.Remove(arrowObject);
    }

    public float NotifyArrowHit(GameObject arrowObject)
    {
        if (arrowMap.TryGetValue(arrowObject, out Arrow arrow))
        {
            arrow.Deactivate();
            arrow.archer.HitTarget();
            return arrow.archer.CurrentDamage;
        }
        return 0;
    }
}
