using UnityEngine;
public interface IMovementStrategy
{
    void Move(Rigidbody2D rb, GameObject target, float speed);
}
