using UnityEngine;

public class GuidedProjectile : Projectile {
	public GameObject m_target;

	protected override void Update () {
		base.Update();

		if (m_target == null) {
			Destroy (gameObject);
			return;
		}

		var translation = m_target.transform.position - transform.position;
		if (translation.magnitude > m_speed) {
			translation = translation.normalized * m_speed;
		}
		transform.Translate (translation);
	}
}
