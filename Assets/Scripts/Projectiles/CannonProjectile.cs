using UnityEngine;

public class CannonProjectile : Projectile
{
	protected override void Update()
	{
		base.Update();
		transform.Translate (Vector3.forward * m_speed);
	}
}