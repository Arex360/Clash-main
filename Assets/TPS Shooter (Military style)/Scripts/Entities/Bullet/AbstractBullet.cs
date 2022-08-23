using UnityEngine;

namespace TPSShooter
{
  public abstract class AbstractBullet : MonoBehaviour
  {
    public float speed = 100;
    public float damage = 1;
    public float lifeTime;
    public LayerMask hitLayers; // layers that can be affected by bullet
    public BulletDecals decals;

    private float _startShootTime;
    private Vector3 _startPosition;
    private Vector3 _startDirection;

    private void Awake()
    {
      _startShootTime = Time.time;

      _startPosition = transform.position;
      _startDirection = transform.forward;

      Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
      Vector3 nextPosition = GetNextPosition();
      RaycastHit hit;

      if (Physics.Linecast(transform.position, nextPosition, out hit, hitLayers))
      {
        InstantiateDecals(hit);
        OnBulletCollision(hit);
        Destroy(gameObject);
      }
      else
      {
        transform.position = nextPosition;
      }
    }

    private Vector3 GetNextPosition()
    {
      float timeFromStart = Time.time - _startShootTime;
      return _startPosition + (_startDirection * speed * timeFromStart);
    }

    private void InstantiateDecals(RaycastHit hit)
    {
      if (decals == null) return;

      Quaternion decalRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

      foreach (var decalProperty in decals.decalProperties)
      {
        if (!hit.collider.material.name.Contains(decalProperty.surfacePhysicMaterial.name)) continue;

        foreach (GameObject decalPrefab in decalProperty.decalPrefabs)
        {
          GameObject decal = Instantiate(decalPrefab, hit.point, decalRotation, hit.transform);

          float destroyTime = decal.GetComponent<ParticleSystem>() ? decal.GetComponent<ParticleSystem>().main.duration : decals.destroyTime;
          Destroy(decal, destroyTime);
        }
      }
    }

    protected abstract void OnBulletCollision(RaycastHit hit);
  }
}
