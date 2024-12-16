using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    public bool IsActive { get; set; }
    public int Level { get; set; }
    public float Cooldown { get; set; }
    public float cooldownTimer;
    public int DefaultLevel;
    public WeaponLevel[] weaponLevels;
    [SerializeField] protected Player playerRef;
    protected virtual void Start()
    {
        Level = DefaultLevel;
        if (Level > 0)
        {
            Activate();
        }
    }

    public void Activate()
    {
        if (!IsActive)
        {
            cooldownTimer = Cooldown;
            IsActive = true;
        }
        
    }
    public void DeActivate()
    {
        if (IsActive)
        {
            cooldownTimer = 0;
            IsActive = false;
        }
    }
    protected virtual void Update()
    {
        if (cooldownTimer <= 0)
        {
            if (IsActive)
            {
                Execute();
                cooldownTimer = Cooldown;
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
    protected abstract void Execute();
    public WeaponLevel getNextUpgrade()
    {
        foreach (WeaponLevel wl in weaponLevels)
        {
            if (wl.level == (this.Level + 1))
            {
                return wl;
            }
        }
        return null;
    }
    public virtual void UpgradeToNextLevel(WeaponLevel weaponLevel)
    {
        if (weaponLevel.level != (Level + 1))
        {
            Debug.LogError("Wrong level");
            return;
        }
        if (weaponLevel.level == 1)
        {
            Activate();
        }
    }
    public virtual void ResetToDefaultLevel()
    {
        Level = DefaultLevel;
        if (Level == 0)
        {
            DeActivate();
        }
    }
    
}