using UnityEngine;
using UnityEngine.UI;

namespace TPSShooter
{
  [RequireComponent(typeof(EnemyBehaviour))]
  public class EnemyHealthBar : MonoBehaviour
  {
    public Transform holder;
    public Image fillImage;

    private EnemyBehaviour enemy;
    private Transform player;

    private void Start()
    {
      player = PlayerBehaviour.GetInstance().transform;
      enemy = GetComponent<EnemyBehaviour>();

      enemy.onHpChanged += OnHpChanged;
      enemy.onDied += OnDied;
      UpdateHP();
    }

    private void Update()
    {
      holder.LookAt(player);
      holder.AnulateRotationExceptY();
    }

    private void OnHpChanged()
    {
      UpdateHP();
    }

    private void OnDied()
    {
      holder.gameObject.SetActive(false);
    }

    private void UpdateHP()
    {
      fillImage.fillAmount = enemy.GetHP() / 100;
    }
  }
}
