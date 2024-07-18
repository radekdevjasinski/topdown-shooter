using UnityEngine;

public class WalkTowardsStrategy : MonoBehaviour, IMovementStrategy
{
    public void Move(Rigidbody2D rb, GameObject target, float speed)
    {
        if (target != null)
        {
            Vector2 direction = (target.transform.position - rb.transform.position).normalized;
            if (direction.x > 0)
            {
                GetComponentInChildren<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponentInChildren<SpriteRenderer>().flipX = false;

            }
            rb.velocity = direction * speed * Time.fixedDeltaTime;

        }
    }
}
