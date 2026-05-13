public class Worm : Enemy {
    protected override float maximumSpeed { get; set; } = 2f;
    protected override float maximumHealth { get; set; } = 50f;
    protected override float attackDamage { get; set; } = 10f;
    protected override float attackInterval { get; set; } = 2f;
    protected override int expAmount { get; set; } = 15;
}