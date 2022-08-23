using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LightDev.Core;

using DG.Tweening;

namespace TPSShooter
{
  [RequireComponent(typeof(Renderer))]
  public class PickUpAnimation : Base
  {
    new private Renderer renderer;
    private static readonly UnityEngine.Color Black = new UnityEngine.Color(0.0001f, 0.0001f, 0.0001f);
    private static readonly UnityEngine.Color White = new UnityEngine.Color(0.9999f, 0.9999f, 0.9999f);
    private static readonly int EmmisionPropertyID = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
      renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
      RunAnimation();
    }

    private void RunAnimation()
    {
      SetEmmisionColor(Black);
      Sequence(
        DOTween.To((value) =>
        {
          LerpEmmisionColor(value);
        }, 0, 1, 0.5f).SetEase(Ease.InSine),
        DOTween.To((value) =>
        {
          LerpEmmisionColor(value);
        }, 1, 0, 0.5f).SetEase(Ease.InSine)
      ).SetLoops(-1);
    }

    private void LerpEmmisionColor(float lerp)
    {
      SetEmmisionColor(UnityEngine.Color.Lerp(Black, White, lerp));
    }

    private void SetEmmisionColor(UnityEngine.Color color)
    {
      renderer.material.SetColor(EmmisionPropertyID, color);
    }
  }
}
