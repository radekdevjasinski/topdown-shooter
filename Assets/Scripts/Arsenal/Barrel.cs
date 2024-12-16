using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : WeaponBase
{
    public float explosionRadius = 3f;
    public float barrelDuration = 60f;
    public float barrelDamage = 1f;
    public float barrelCooldown = 10f;
    public float dominoTime = 0.1f;
    [SerializeField] private float spawnRadius = 5f; 
    private GameObject barrelPrefab;
    private float defaultDamage;


    protected override void Start()
    {
        Cooldown = barrelCooldown;
        defaultDamage = barrelDamage;
        barrelPrefab = Resources.Load<GameObject>("Prefabs/Weapons/BarrelObject");
        base.Start();
    }

    private Vector3 GetRandomPositionAroundPlayer()
    {
        // Generowanie losowej pozycji wok� gracza w zadanym promieniu
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Cos(angle) * (spawnRadius + Random.Range(-1f,1f));
        float y = Mathf.Sin(angle) * (spawnRadius + Random.Range(-1f, 1f));


        return new Vector3( playerRef.transform.position.x + x,  playerRef.transform.position.y + y,  playerRef.transform.position.z);
    }
    protected override void Execute()
    {
        Vector3 spawnPosition = GetRandomPositionAroundPlayer();
        GameObject barrel = Instantiate(barrelPrefab, spawnPosition, Quaternion.identity);
        barrel.GetComponent<BarrelObject>().barrel = this;
        Destroy(barrel, barrelDuration);
    }
    public override void UpgradeToNextLevel(WeaponLevel weaponLevel)
    {
        base.UpgradeToNextLevel(weaponLevel);
        Cooldown += weaponLevel.cooldownChange;
        barrelDamage += weaponLevel.damageChange;
        Level = weaponLevel.level;
    }
    public override void ResetToDefaultLevel()
    {
        Cooldown = barrelCooldown;
        barrelDamage = defaultDamage;
        base.ResetToDefaultLevel();
    }
}
