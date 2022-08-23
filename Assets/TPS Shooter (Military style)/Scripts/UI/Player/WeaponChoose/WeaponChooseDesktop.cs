using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LightDev;
using LightDev.UI;

using TPSShooter;

namespace TPSShooter.UI
{
  public class WeaponChooseDesktop : WeaponChoose
  {
    public override void Subscribe()
    {
      base.Subscribe();

      Events.WeaponChooseStartRequest += Show;
      Events.WeaponChooseFinishRequest += Hide;
    }

    public override void Unsubscribe()
    {
      base.Unsubscribe();

      Events.WeaponChooseStartRequest -= Show;
      Events.WeaponChooseFinishRequest -= Hide;
    }

    protected override void OnStartHiding()
    {
      Events.SwapWeaponRequested.Call(selectedButtonId);
    }
  }
}
