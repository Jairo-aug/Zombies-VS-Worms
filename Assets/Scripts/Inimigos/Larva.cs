public class Larva : Enemy {
    protected override float maxSpeed { get; set; } = 1f;
    protected override float maxHealth { get; set; } = 85f;
    protected override float danoAtaque { get; set; } = 10f;
    protected override float intervaloDano { get; set; } = 2f;
    protected override int expAmount { get; set; } = 30;
}