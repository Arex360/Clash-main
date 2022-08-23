using UnityEngine;
using UnityEngine.UI;

using LightDev;
using LightDev.UI;

namespace TPSShooter.UI
{
  public class PlayerHP : CanvasElement
  {
    [Header("References")]
    public Image healthBar;

    public override void Subscribe()
    {
      Events.SceneLoaded += Show;
      Events.GamePaused += Hide;
      Events.GameResumed += Show;
      Events.PlayerDied += Hide;
      Events.PlayerChangedHP += OnPlayerChangedHP;
    }

    public override void Unsubscribe()
    {
      Events.SceneLoaded -= Show;
      Events.GamePaused -= Hide;
      Events.GameResumed -= Show;
      Events.PlayerDied -= Hide;
      Events.PlayerChangedHP -= OnPlayerChangedHP;
    }

    private void OnPlayerChangedHP()
    {
      var player = PlayerBehaviour.GetInstance();
      healthBar.fillAmount = player.GetCurrentHP() / player.GetMaxHP();
    }
  }
}