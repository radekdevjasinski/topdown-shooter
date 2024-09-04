using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    //pola u�ywane przez ka�d� bro�
    public bool IsActive { get; set; }  
    public int Level { get; set; }
    public float Cooldown { get; set; }
    public float cooldownTimer;

   //metoda aktywuj�ca umiej�tno��
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
                //je�eli czas min�� i umiej�tno�� jest aktywna - wykonaj akcj�
                Execute();
                cooldownTimer = Cooldown;
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
    //abstrakcyjna metoda wykonania umiej�tno�ci - ka�da bro� musi mie� tak� funkcj�
    protected abstract void Execute();
}