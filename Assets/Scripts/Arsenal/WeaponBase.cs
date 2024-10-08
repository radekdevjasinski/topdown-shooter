using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    //pola u¿ywane przez ka¿d¹ broñ
    public bool IsActive { get; set; }
    public int Level { get; set; }
    public float Cooldown { get; set; }
    public float cooldownTimer;
    public WeaponLevel[] weaponLevels;

   //metoda aktywuj¹ca umiejêtnoœæ
    public void Activate()
    {
        if (!IsActive)
        {
            cooldownTimer = Cooldown;
            IsActive = true;
        }
    }
    //mechanizm zegara w funkcji Update
    protected virtual void Update()
    {
        if (cooldownTimer <= 0)
        {
            if (IsActive)
            {
                //je¿eli czas min¹³ i umiejêtnoœæ jest aktywna - wykonaj akcjê
                Execute();
                cooldownTimer = Cooldown;
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
    //abstrakcyjna metoda wykonania umiejêtnoœci - ka¿da broñ musi mieæ tak¹ funkcjê
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
}