using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace LightDev.Core
{
  public partial class Base
  {
    protected List<Sequence> sequences = new List<Sequence>();

    protected virtual void OnDisable()
    {
      KillSequences();
    }

    public virtual Sequence Sequence(params Tween[] tweens)
    {
      Sequence sequence = DOTween.Sequence();
      if(gameObject.layer == LayerMask.NameToLayer("UI"))
      {
        sequence.SetUpdate(true);
      }
      foreach(Tween tween in tweens)
      {
        sequence.Append(tween);
      }

      sequence.OnKill(() => 
      {
        this.sequences.Remove(sequence);
      });

      this.sequences.Add(sequence);

      return sequence;
    }

    public virtual void PauseSequence(string id)
    {
      for (int i = 0; i < this.sequences.Count; i++)
      {
        if (this.sequences[i].stringId != null && this.sequences[i].stringId.Equals(id))
        {
          this.sequences[i].Pause();
        }
      }
    }

    public virtual void PauseSequences()
    {
      foreach (Sequence sequence in this.sequences)
      {
        sequence.Pause();
      }
    }

    public virtual void ResumeSequence(string id)
    {
      for (int i = 0; i < this.sequences.Count; i++)
      {
        if (this.sequences[i].stringId != null && this.sequences[i].stringId.Equals(id))
        {
          this.sequences[i].Play();
        }
      }
    }

    public virtual void ResumeSequences()
    {
      foreach (Sequence sequence in this.sequences)
      {
        sequence.Play();
      }
    }

    public virtual void KillSequence(string id, bool complete = false)
    {
      for (int i = this.sequences.Count - 1; i >= 0; i--)
      {
        if (this.sequences[i].stringId != null && this.sequences[i].stringId.Equals(id))
        {
          var sequence = this.sequences[i];
          this.sequences.RemoveAt(i);
          sequence.Kill(complete);
        }
      }
    }

    public virtual void KillSequences(bool complete = false)
    {
      for (int i = this.sequences.Count - 1; i >= 0; i--)
      {
        this.sequences[i].Kill(complete);
      }

      this.sequences.Clear();
    }

    public virtual bool ContainsSequence(string id)
    {
      for(int i = 0; i < this.sequences.Count; i++)
      {
        if(sequences[i].stringId != null && sequences[i].stringId.Equals(id))
        {
          return true;
        }
      }

      return false;
    }

    public virtual bool IsAnySequenceRunning()
    {
      return sequences.Count != 0;
    }

    public virtual Tween Delay(float delay)
    {
      return DOTween.Sequence().AppendInterval(delay);
    }

    public virtual Tween OnFinish(TweenCallback tweenCallback)
    {
      return DOTween.Sequence().AppendCallback(tweenCallback);
    }

    public virtual Tween Move(Vector3 endValue, float duration)
    {
      if(transform is RectTransform)
        return ((RectTransform)transform).DOAnchorPos(endValue, duration);

      return transform.DOMove(endValue, duration);
    }

    public virtual Tween MoveX(float endValue, float duration)
    {
      if (transform is RectTransform)
        return ((RectTransform)transform).DOAnchorPosX(endValue, duration);

      return transform.DOMoveX(endValue, duration);
    }

    public virtual Tween MoveY(float endValue, float duration)
    {
      if (transform is RectTransform)
        return ((RectTransform)transform).DOAnchorPosY(endValue, duration);

      return transform.DOMoveY(endValue, duration);
    }

    public virtual Tween MoveZ(float endValue, float duration)
    {
      if (transform is RectTransform)
        throw new System.NotImplementedException("RectTransfrom does not have anchoredPosition.z");

      return transform.DOMoveZ(endValue, duration);
    }

    public virtual Tween MoveLocal(Vector3 endValue, float duration)
    {
      if (transform is RectTransform)
        return ((RectTransform)transform).DOAnchorPos(endValue, duration);

      return transform.DOLocalMove(endValue, duration);
    }

    public virtual Tween MoveLocalX(float endValue, float duration)
    {
      if (transform is RectTransform)
        return ((RectTransform)transform).DOAnchorPosX(endValue, duration);

      return transform.DOLocalMoveX(endValue, duration);
    }

    public virtual Tween MoveLocalY(float endValue, float duration)
    {
      if (transform is RectTransform)
        return ((RectTransform)transform).DOAnchorPosY(endValue, duration);

      return transform.DOLocalMoveY(endValue, duration);
    }

    public virtual Tween MoveLocalZ(float endValue, float duration)
    {
      if (transform is RectTransform)
        throw new System.NotImplementedException("RectTransfrom does not have anchoredPosition.z");

      return transform.DOLocalMoveZ(endValue, duration);
    }

    public virtual Tween Rotate(Quaternion endValue, float duration)
    {
      return transform.DORotateQuaternion(endValue, duration);
    }

    public virtual Tween Rotate(Vector3 endValue, float duration)
    {
      return transform.DORotate(endValue, duration);
    }

    public virtual Tween RotateX(float endValue, float duration)
    {
      return transform.DORotate(new Vector3(endValue, GetRotationY(), GetRotationZ()), duration);
    }

    public virtual Tween RotateY(float endValue, float duration)
    {
      return transform.DORotate(new Vector3(GetRotationX(), endValue, GetRotationZ()), duration);
    }

    public virtual Tween RotateZ(float endValue, float duration)
    {
      return transform.DORotate(new Vector3(GetRotationX(), GetRotationY(), endValue), duration);
    }

    public virtual Tween RotateLocal(Quaternion endValue, float duration)
    {
      return transform.DOLocalRotateQuaternion(endValue, duration);
    }

    public virtual Tween RotateLocal(Vector3 endValue, float duration)
    {
      return transform.DOLocalRotate(endValue, duration);
    }

    public virtual Tween RotateLocalX(float endValue, float duration)
    {
      return transform.DOLocalRotate(new Vector3(endValue, GetLocalRotationY(), GetLocalRotationZ()), duration);
    }

    public virtual Tween RotateLocalY(float endValue, float duration)
    {
      return transform.DOLocalRotate(new Vector3(GetLocalRotationX(), endValue, GetLocalRotationZ()), duration);
    }

    public virtual Tween RotateLocalZ(float endValue, float duration)
    {
      return transform.DOLocalRotate(new Vector3(GetLocalRotationX(), GetLocalRotationY(), endValue), duration);
    }

    public virtual Tween Scale(Vector3 endValue, float duration)
    {
      return transform.DOScale(endValue, duration);
    }

    public virtual Tween Scale(float endValue, float duration)
    {
      return Scale(new Vector3(endValue, endValue, endValue), duration);
    }

    public virtual Tween ScaleX(float endValue, float duration)
    {
      return transform.DOScaleX(endValue, duration);
    }

    public virtual Tween ScaleY(float endValue, float duration)
    {
      return transform.DOScaleY(endValue, duration);
    }

    public virtual Tween ScaleZ(float endValue, float duration)
    {
      return transform.DOScaleZ(endValue, duration);
    }

    public virtual Tween Fade(float endValue, float duration)
    {
      if(GetComponent<Image>())
      {
        return GetComponent<Image>().DOFade(endValue, duration);
      }

      if(GetComponent<Text>())
      {
        return GetComponent<Text>().DOFade(endValue, duration);
      }

      throw new NotImplementedException();
    }

    public virtual Tween Color(Color endValue, float duration)
    {
      if (GetComponent<Image>())
      {
        return GetComponent<Image>().DOColor(endValue, duration);
      }

      if(GetComponent<Text>())
      {
        return GetComponent<Text>().DOColor(endValue, duration);
      }

      throw new NotImplementedException();
    }
  }
}
