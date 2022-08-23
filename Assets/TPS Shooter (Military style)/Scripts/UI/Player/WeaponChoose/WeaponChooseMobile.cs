using LightDev;

namespace TPSShooter.UI
{
  public class WeaponChooseMobile : WeaponChoose
  {
    public override void Subscribe()
    {
      base.Subscribe();

      Events.WeaponChooseStartRequest += TryShow;
      foreach (WeaponChooseButton button in buttons)
      {
        if (button is WeaponChooseButtonMobile)
        {
          var b = (WeaponChooseButtonMobile)button;
          b.requestWeaponSwap += OnRequestSwap;
          b.requestWeaponDrop += OnRequestDrop;
        }
      }
    }

    public override void Unsubscribe()
    {
      base.Unsubscribe();

      Events.WeaponChooseStartRequest -= TryShow;
      foreach (WeaponChooseButton button in buttons)
      {
        if (button is WeaponChooseButtonMobile)
        {
          var b = (WeaponChooseButtonMobile)button;
          b.requestWeaponSwap -= OnRequestSwap;
          b.requestWeaponDrop -= OnRequestDrop;
        }
      }
    }

    protected virtual void TryShow()
    {
      if (gameObject.activeSelf) Hide();
      else Show();
    }

    protected virtual void OnRequestSwap()
    {
      Events.SwapWeaponRequested.Call(selectedButtonId);
			Hide();
    }

    protected virtual void OnRequestDrop()
    {
      Events.DropWeaponRequested.Call(selectedButtonId);
      Hide();
    }
  }
}
