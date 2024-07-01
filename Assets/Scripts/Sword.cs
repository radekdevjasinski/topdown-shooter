using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IAbility
{
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private float slashSpeed = 100f;
    [SerializeField] private float slashCooldown = 2f;
    private Animator playerAnimator;
    private bool isActive = true;

    public bool IsActive => isActive;

    void Start()
    {
        playerAnimator = Player.Instance.gameObject.GetComponent<Animator>();
        StartCoroutine(SlashCoroutine());
    }

    IEnumerator SlashCoroutine()
    {
        yield return new WaitForSeconds(slashCooldown);
        Slash();
        StartCoroutine(SlashCoroutine());
    }

    void Slash()
    {
        if (isActive)
        {
            GameObject slash = Instantiate(slashPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = slash.GetComponent<Rigidbody2D>();
            Vector2 dir;
            if (playerAnimator.GetFloat("WasFacing") > 0)
            {
                dir = Vector2.right;
            }
            else
            {
                slash.transform.rotation = Quaternion.Euler(0, 180, 0);
                dir = Vector2.left;
            }
            rb.velocity = dir * slashSpeed * Time.fixedDeltaTime;
        }
    }

    public void Activate(bool state)
    {
        isActive = state;
    }

    public void Upgrade()
    {
        throw new System.NotImplementedException();
    }
}
