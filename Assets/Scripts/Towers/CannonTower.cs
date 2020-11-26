using UnityEngine;

public class CannonTower : Tower {
    private const float m_accuracy = 0.001f;
    private Vector3 relativePos;

    public Transform m_shootPoint;
    public float m_rotationSpeed = 1.3f;
    public float m_rotationAccuracy = 4f;

    private void Update()
    {
        if (m_shootPoint == null)
            return;

        // set target for the cannon
        if (monster == null)
        {
            float range = m_range;
            foreach (var _monster in FindObjectsOfType<Monster>())
            {
                float distance = Vector3.Distance(transform.position, _monster.transform.position);
                if (distance < range)
                {
                    monster = _monster;
                    range = distance;
                }
            }
        }
        else
        {
            Debug.Log(Vector3.Distance(transform.position, monster.transform.position));
            if (Vector3.Distance(transform.position, monster.transform.position) > m_range)
            {
                monster = null;
                return;
            }

            relativePos = GetTargetPoint(monster, monster.transform.position, 0);
            if (relativePos == m_shootPoint.position)
            {
                monster = null;
                return;
            }

#if UNITY_EDITOR

            Debug.DrawLine(m_shootPoint.position, monster.transform.position, Color.blue);
            Debug.DrawLine(m_shootPoint.position, m_shootPoint.position + relativePos, Color.blue);
            Debug.DrawLine(monster.transform.position, m_shootPoint.position + relativePos, Color.green);

#endif

            if (m_lastShotTime + m_shootInterval > Time.time)
                return;

            // checking the gun sight accuracy
            if (GetSightDirection() > m_rotationAccuracy)
                return;

            Instantiate(m_projectilePrefab, m_shootPoint.position, transform.rotation);
            m_lastShotTime = Time.time;
        }
    }

    private Vector3 GetTargetPoint(Monster monster, Vector3 position, float time)
    {
        // set direction to target point
        var direction = Vector3.Distance(position, m_shootPoint.position);

        // check the ability to hit
        if (direction > m_range * 2f)
        {
            // Debug.Log("Target point is out of range");
            return m_shootPoint.position;
        }

        // calculate the time for projectile shoot
        var shoot = m_projectilePrefab.GetComponent<CannonProjectile>();
        var t = direction / shoot.m_speed;

        // calculate the approximation for the target point
        Vector3 v_monster = monster.TargetDir.normalized * monster.m_speed;
        Vector3 delta = v_monster * (t - time);
        Vector3 approach_point = position + delta;

        if (Mathf.Abs(t - time) > m_accuracy)
        {
            return GetTargetPoint(monster, approach_point, t);
        }

        Vector3 relativeDir = approach_point - m_shootPoint.position;

        return relativeDir;
    }

    private float GetSightDirection()
    {
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_rotationSpeed);
        Vector3 v_rotation = transform.rotation.eulerAngles - rotation.eulerAngles;
        v_rotation = new Vector3(
            Mathf.Min(v_rotation.x, 360 - v_rotation.x),
            Mathf.Min(v_rotation.y, 360 - v_rotation.y),
            Mathf.Min(v_rotation.z, 360 - v_rotation.z)
        );
        return v_rotation.magnitude;
    }
}