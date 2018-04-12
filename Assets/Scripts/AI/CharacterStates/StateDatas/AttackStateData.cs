using UnityEngine;

public class AttackStateData : CharacterStateData {
    [SerializeField][Tooltip("Projectile to fire.")]
    GameObject m_projectile = null;
    public GameObject projectile { get { return m_projectile; } }

    [SerializeField][Tooltip("Our target that we are attacking.")]
    Transform m_target = null;
    public Transform target { get { return m_target; } set { m_target = value; } }
    
    [SerializeField][Tooltip("Time to wait for attack.")]
    float m_cooldownTime = 0.0f;
    public float cooldownTime { get { return m_cooldownTime; } }
    
    [SerializeField][Tooltip("The distance of the spawned projectile from the character")]
    float m_spawnDistance = 0.0f;
    public float spawnDistance { get { return m_spawnDistance; } }

    [Tooltip("timer we are using to attack.")]
    Timer m_timer = null;
    public Timer timer { get{ return m_timer; } set { m_timer = value; } }

    [SerializeField][Tooltip("How many projectiles at once")]
    int m_projectiles = 1;
    public int projectiles { get { return m_projectiles; }}
    
    [SerializeField][Tooltip("how many degrees between projectiles")]
    float m_angle = 0;
    public float angle { get { return m_angle; }}

    [SerializeField]
    FindTargetStateData m_findTransform;
    public FindTargetStateData findTransform { get { return m_findTransform; } }

    [SerializeField][Tooltip("The damage dealt by this projectile upon impact")]
    private int m_damage;
    public int damage {  get { return m_damage; } }

    [SerializeField][Tooltip("The status effect applied on unit hit by this projectile")]
    private StatusEffect m_statusEffectOnHit;
    public StatusEffect statusEffectOnHit {  get { return m_statusEffectOnHit; } }

    [SerializeField][Tooltip("The speed of the projectile")]
    private float m_projectileSpeed;
    public float projectileSpeed { get { return m_projectileSpeed; } }
}
