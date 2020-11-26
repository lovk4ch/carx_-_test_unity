using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float m_lifetime = 10;
    public float m_speed = 0.2f;
    public int m_damage = 10;

	protected void Destroy()
    {
		Destroy(gameObject);
	}

    protected virtual void Update()
    {
		if (m_lifetime > 0)
			m_lifetime -= Time.deltaTime;
		else
			Destroy();
	}

    protected virtual void OnTriggerEnter(Collider other)
    {
		var monster = other.gameObject.GetComponent<Monster>();
		if (monster == null)
			return;

		monster.m_hp -= m_damage;
		if (monster.m_hp <= 0)
		{
			Destroy(monster.gameObject);
		}
		Destroy();
	}
}