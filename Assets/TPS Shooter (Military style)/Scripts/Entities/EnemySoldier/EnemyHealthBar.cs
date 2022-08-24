using UnityEngine;
using UnityEngine.UI;
using Mirror;
namespace TPSShooter
{
  [RequireComponent(typeof(EnemyBehaviour))]
  public class EnemyHealthBar : NetworkBehaviour
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
    [Command(requiresAuthority = false)]
    public void CmdKill(){
      NetworkServer.Destroy(this.gameObject);
    }
    private void OnDied()
    {

      holder.gameObject.SetActive(false);
      CmdKill();
    }

    private void UpdateHP()
    {
      fillImage.fillAmount = enemy.GetHP() / 100;
    }
  }
}
