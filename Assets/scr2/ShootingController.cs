using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public GameObject bulletPrefab;    // ������ ����
    public Transform firePoint;        // �����, ������ ����� ���������� ����
    public float bulletSpeed = 20f;    // �������� ����
    public float fireRate = 1f;        // ������� ��������
    private float nextFireTime = 0f;   // ����� ��� ��������� ��������

    void Update()
    {
        // ���� ������ ����� ��� ���������� ��������, ��������
        if (Time.time >= nextFireTime)
        {
            Attack();
        }
    }

    public void Attack()
    {
        // ���������, ���� ������ ���� � ����� ������ ������������
        if (bulletPrefab != null && firePoint != null)
        {
            // ������� ����
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

            if (bulletRigidbody != null)
            {
                // ���������� ���� � �������, ���� ������� firePoint
                Vector3 direction = firePoint.forward;  // ����������� ����
                bulletRigidbody.velocity = direction * bulletSpeed;
            }

            // ������������� ����� ��� ���������� ��������
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
}


