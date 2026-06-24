using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "Scriptable Objects/ZombieData")]
public class ZombieData : ScriptableObject {
    public string zombieName;
    public int zombieCost;
    public float range;
    public float reactionRange;
    public float actionTime;
    public float damage;
    public float maxHealth;
    public float healAmount;
    public int fleshificationAmount;
}