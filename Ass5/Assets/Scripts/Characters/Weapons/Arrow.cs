using UnityEngine;

public class Arrow
{
    public Vector3 Position { get; private set; }
    public Vector3 Direction { get; private set; }
    public bool IsActive;
    private Vector3 startPosition;
    private float range;
    private float speed;
    public Archer archer;
    public bool isPierce;
    public float damage;

    public Arrow(Archer archer)
    {
        this.archer = archer;
        this.startPosition = archer.transform.position;
        startPosition.y += archer.gameObject.GetComponent<BoxCollider>().size.y / 2;
        Position = startPosition;
        Direction = archer.transform.forward.normalized;
        speed = 20;
        this.range = archer.AttackRange;
        IsActive = true;
        isPierce = archer.ability.abilityIsActivated;
        damage = archer.CurrentDamage;
    }

    public void UpdatePosition(float deltaTime)
    {
        if (!IsActive) return;

        Position += Direction * speed * deltaTime;

        if (OutOfRange())
            Deactivate();
    }

    private bool OutOfRange()
    {
        return (Position - startPosition).magnitude > range;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}

