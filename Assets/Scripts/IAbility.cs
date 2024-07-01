public interface IAbility
{
    void Activate(bool state);
    void Upgrade();
    bool IsActive { get; }
}
