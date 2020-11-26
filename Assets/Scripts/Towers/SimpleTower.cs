using UnityEngine;

public class SimpleTower : Tower {

	private void Update()
	{
		if (m_lastShotTime + m_shootInterval > Time.time)
			return;

		if (monster == null)
		{
			float range = m_range;
			foreach (var _monster in FindObjectsOfType<Monster>())
			{
				float distance = Vector3.Distance(transform.position, _monster.transform.position);
				if (distance < range)
				{
					monster = _monster;
					var projectile = Instantiate(m_projectilePrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
					var projectileBeh = projectile.GetComponent<GuidedProjectile>();
					projectileBeh.m_target = monster.gameObject;
				}
			}
		}
	}
}