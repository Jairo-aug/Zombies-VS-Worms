using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "Scriptable Objects/ZombieData")]
public class ZombieData : ScriptableObject {
    public string zombieName;
    public float zombieCost;
    public float range;
    public float actionTime;
    public float damage;
    public float maxHealth;
}