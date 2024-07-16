public interface IWeapon
{
    bool IsActive { get; set; }
    int Level { get; set; }
    void Activate();
}