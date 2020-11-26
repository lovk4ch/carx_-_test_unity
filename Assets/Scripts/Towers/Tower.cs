using UnityEngine;

public class Tower : MonoBehaviour
{
    protected float m_lastShotTime = -0.5f;
    protected Monster monster;

    public float m_shootInterval = 0.5f;
    public float m_range = 4f;
    public GameObject m_projectilePrefab;

    private void Awake()
    {
        m_lastShotTime = -m_shootInterval;
    }
}