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

    public Arrow(Archer archer, Vector3 startPosition, Vector3 direction, float range)
    {
        this.archer = archer;
        this.startPosition = startPosition;
        Position = startPosition;
        Direction = direction.normalized;
        speed = 3;
        this.range = range;
        IsActive = true;
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

