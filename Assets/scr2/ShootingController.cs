using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public GameObject bulletPrefab;    // Префаб пули
    public Transform firePoint;        // Точка, откуда будет спавниться пуля
    public float bulletSpeed = 20f;    // Скорость пули
    public float fireRate = 1f;        // Частота стрельбы
    private float nextFireTime = 0f;   // Время для следующей стрельбы

    void Update()
    {
        // Если прошло время для следующего выстрела, стреляем
        if (Time.time >= nextFireTime)
        {
            Attack();
        }
    }

    public void Attack()
    {
        // Проверяем, если префаб пули и точка спауна присутствуют
        if (bulletPrefab != null && firePoint != null)
        {
            // Создаем пулю
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

            if (bulletRigidbody != null)
            {
                // Направляем пулю в сторону, куда смотрит firePoint
                Vector3 direction = firePoint.forward;  // Направление пули
                bulletRigidbody.velocity = direction * bulletSpeed;
            }

            // Устанавливаем время для следующего выстрела
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
}


