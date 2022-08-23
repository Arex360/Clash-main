using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LightDev;

namespace TPSShooter
{
  public class HealthPickUp : PickUp
  {
    public float value;

    protected override void OnPlayerCollision(PlayerBehaviour player)
    {
      player.IncreaseHP(value);
      Events.PlayerPickUpHealthPack.Call();
      Destroy(gameObject);
    }
  }
}
