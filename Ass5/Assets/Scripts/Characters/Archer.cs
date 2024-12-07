using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Character
{
    public int ammoPerRound;
    public int currentAmmo;
    private float reloadTime;
    private float timeSinceReload;

    public int targetHits;
    private int hitsForPierceShot; // number of hits needed to activate pierce shot 

    [SerializeField]
    private GameObject arrowPrefab;
    private List<Arrow> arrows = new List<Arrow>();
    private Queue<GameObject> arrowPool = new Queue<GameObject>();

    protected override void Awake()
    {
        base.Awake();

        ability = Stats.GetInstantiatedAbility() as PierceShot;
        ability.Initialize(this);

        ammoPerRound = 10;
        currentAmmo = ammoPerRound;
        reloadTime = 3;
        timeSinceReload = reloadTime;
        targetHits = 0;
        hitsForPierceShot = 5;
    }
    
    protected override void Update()
    {
        base.Update();
        if ((targetHits >= hitsForPierceShot) & !ability.abilityIsActivated)
            ability.Activate();
        ReloadAmmo();
        UpdateArrows();
    }

    public override void Attack()
    {
        base.Attack();
        if (!ability.abilityIsActivated) 
        {
            if (currentAmmo > 0)
                currentAmmo--;
            else
                return;
        }
        GameObject arrowObject = GetArrowFromPool();
        arrowObject.SetActive(true);

        Vector3 direction = transform.forward;
        Arrow arrow = new Arrow(this, transform.position, direction, Stats.attackRange);
        arrows.Add(arrow);
        arrowObject.GetComponent<ArrowCollision>().associatedArrow = arrow;
    }

    public void HitTarget()
    {
        if (!ability.abilityIsActivated)
            targetHits++;
    }

    private void ReloadAmmo()
    {
        if (currentAmmo == 0) // No ammo left
        {
            if (timeSinceReload >= reloadTime)
            {
                timeSinceReload = 0;
                currentAmmo = ammoPerRound;
            }
            else
                timeSinceReload += Time.deltaTime;
        }
    }

    private void UpdateArrows()
    {
        for (int i = arrows.Count - 1; i >= 0; i--)
        {
            Arrow arrow = arrows[i];
            if (arrow.IsActive)
            {
                arrow.UpdatePosition(Time.deltaTime);

                GameObject arrowObject = arrowPool.Dequeue();
                arrowObject.transform.position = arrow.Position;
                arrowPool.Enqueue(arrowObject);
            }
            else
            {
                GameObject arrowObject = arrowPool.Dequeue();
                arrowObject.SetActive(false);
                arrowPool.Enqueue(arrowObject);

                arrows.RemoveAt(i);
            }
        }
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

}
