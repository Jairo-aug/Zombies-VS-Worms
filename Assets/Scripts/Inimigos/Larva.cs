public class Larva : Enemy {
    protected override float maximumSpeed { get; set; } = 1f;
    protected override float maximumHealth { get; set; } = 85f;
    protected override float attackDamage { get; set; } = 10f;
    protected override float attackInterval { get; set; } = 2f;
    protected override int expAmount { get; set; } = 30;
}