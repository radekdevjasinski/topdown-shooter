using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private float slashSpeed = 100f;
    [SerializeField] private float slashCooldown = 2f;
    private Animator playerAnimator;


    void Start()
    {
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
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
