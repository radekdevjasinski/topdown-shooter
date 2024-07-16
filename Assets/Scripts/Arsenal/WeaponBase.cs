using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    public bool IsActive { get; set; }  
    public int Level { get; set; }
    public float Cooldown { get; set; }
    protected float cooldownTimer;

   
    public void Activate()
    {
        if (!IsActive && cooldownTimer <= 0)
        {
            //Execute();
            cooldownTimer = Cooldown;
            IsActive = true;
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
}