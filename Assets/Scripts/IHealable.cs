public interface IHealable {
    bool isHealthFull { get; }
    public void GetHealed(float healedAmount);
}