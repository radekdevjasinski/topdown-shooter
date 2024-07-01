using UnityEngine;

public class FlyTowardsStrategy : MonoBehaviour, IMovementStrategy
{
    public void Move(Rigidbody2D rb, GameObject target, float speed)
    {
        if (target != null)
        {
            Vector2 direction = (target.transform.position - rb.transform.position).normalized;
            rb.velocity = direction * speed * Time.fixedDeltaTime;
        }
    }
}
