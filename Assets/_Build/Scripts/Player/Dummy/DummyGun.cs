using UnityEngine;
public class DummyGun: MonoBehaviour
{
    [SerializeField] private DummyBullet _bullet;
    [SerializeField] private Transform _gunShootPoint;
    [SerializeField] private GameObject _body;
    private Camera _camera;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var bullet = SpawnProjectile();
            bullet.SetVelocity();
            Destroy(bullet, 5);
        }

        // как-то определили направление для пушки
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        var direction = (mousePosition - transform.position).normalized;

        // как-то ее переместили (опционально)
        _gunShootPoint.position = transform.position + direction * 2;

        // повернули пушку в нужном направлении (в моем случае дуло пушки сонаправлено с transform.up в _gunShootPoint)
        _gunShootPoint.up = direction;
    }

    private void Awake()
    {
        _camera = Camera.main;
    }

    private DummyBullet SpawnProjectile()
    {
        Vector3 gunShootPointPosition = _gunShootPoint.position;

        gunShootPointPosition.z = 0;

        DummyBullet projectile = Instantiate(_bullet, gunShootPointPosition,
            _gunShootPoint.rotation);

        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        SetDirection(projectile);

        return projectile;
    }

    private void SetDirection(DummyBullet projectile)
    {
        var direction = _gunShootPoint.forward;
        direction.z = 0;
        direction = direction.normalized;
        projectile.FlyDirection = direction;
    }

}
