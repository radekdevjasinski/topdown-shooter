public interface IWeapon
{
    bool IsActive { get; set; }
    int Level { get; set; }
    float Cooldown { get; set; }

    void Activate();
}