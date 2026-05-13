public class Minhoca : Enemy {
    protected override float maxSpeed { get; set; } = 2f;
    protected override float maxHealth { get; set; } = 50f;
    protected override float danoAtaque { get; set; } = 10f;
    protected override float intervaloDano { get; set; } = 2f;
    protected override int expAmount { get; set; } = 15;
}