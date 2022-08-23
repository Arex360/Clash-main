using UnityEngine;

namespace LightDev.Core
{
  public partial class Base
  {
    public virtual void Unparent()
    {
      transform.parent = null;
    }

    public virtual void SetSiblingIndex(int index)
    {
      transform.SetSiblingIndex(index);
    }
    
    public virtual Vector3 GetPosition()
    {
      if(transform is RectTransform)
        return ((RectTransform)transform).anchoredPosition;

      return transform.position;
    }

    public virtual float GetPositionX()
    {
      if (transform is RectTransform)
        return ((RectTransform)transform).anchoredPosition.x;

      return transform.position.x;
    }

    public virtual float GetPositionY()
    {
      if (transform is RectTransform)
        return ((RectTransform)transform).anchoredPosition.y;

      return transform.position.y;
    }

    public virtual float GetPositionZ()
    {
      if (transform is RectTransform)
        throw new System.NotImplementedException("RectTransfrom does not have anchoredPosition.z");

      return transform.position.z;
    }

    public virtual Vector3 GetLocalPosition()
    {
      if (transform is RectTransform)
        return ((RectTransform)transform).anchoredPosition;

      return transform.localPosition;
    }

    public virtual float GetLocalPositionX()
    {
      if (transform is RectTransform)
        return ((RectTransform)transform).anchoredPosition.x;

      return transform.localPosition.x;
    }

    public virtual float GetLocalPositionY()
    {
      if (transform is RectTransform)
        return ((RectTransform)transform).anchoredPosition.y;

      return transform.localPosition.y;
    }

    public virtual float GetLocalPositionZ()
    {
      if (transform is RectTransform)
        throw new System.NotImplementedException("RectTransfrom does not have anchoredPosition.z");

      return transform.localPosition.z;
    }

    public virtual Quaternion GetRotation()
    {
      return transform.rotation;
    }

    public virtual Quaternion GetLocalRotation()
    {
      return transform.localRotation;
    }

    public virtual Vector3 GetEulerRotation()
    {
      return transform.eulerAngles;
    }

    public virtual Vector3 GetLocalEulerRotation()
    {
      return transform.localEulerAngles;
    }

    public virtual float GetRotationX()
    {
      return transform.eulerAngles.x;
    }

    public virtual float GetRotationY()
    {
      return transform.eulerAngles.y;
    }

    public virtual float GetRotationZ()
    {
      return transform.eulerAngles.z;
    }

    public virtual float GetLocalRotationX()
    {
      return transform.localEulerAngles.x;
    }

    public virtual float GetLocalRotationY()
    {
      return transform.localEulerAngles.y;
    }

    public virtual float GetLocalRotationZ()
    {
      return transform.localEulerAngles.z;
    }

    public virtual Vector3 GetScale()
    {
      return transform.localScale;
    }

    public virtual float GetScaleX()
    {
      return transform.localScale.x;
    }

    public virtual float GetScaleY()
    {
      return transform.localScale.y;
    }

    public virtual float GetScaleZ()
    {
      return transform.localScale.z;
    }

    public virtual void SetPosition(Vector3 position)
    {
      if (transform is RectTransform)
      {
        ((RectTransform)transform).anchoredPosition = position;
      }
      else
      {
        transform.position = position;
      }
    }

    public virtual void SetPositionX(float x)
    {
      if (transform is RectTransform)
      {
        RectTransform rectTransform = (RectTransform)transform;
        rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
      }
      else
      {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
      }
    }

    public virtual void SetPositionY(float y)
    {
      if (transform is RectTransform)
      {
        RectTransform rectTransform = (RectTransform)transform;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
      }
      else
      {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
      }
    }

    public virtual void SetPositionZ(float z)
    {
      if (transform is RectTransform)
      {
        throw new System.NotImplementedException("RectTransfrom does not have anchoredPosition.z");
      }
      else
      {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
      }
    }

    public virtual void SetLocalPosition(Vector3 localPosition)
    {
      if (transform is RectTransform)
      {
        ((RectTransform)transform).anchoredPosition = localPosition;
      }
      else
      {
        transform.localPosition = localPosition;
      }
    }

    public virtual void SetLocalPositionX(float x)
    {
      if (transform is RectTransform)
      {
        RectTransform rectTransform = (RectTransform)transform;
        rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
      }
      else
      {
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
      }
    }

    public virtual void SetLocalPositionY(float y)
    {
      if (transform is RectTransform)
      {
        RectTransform rectTransform = (RectTransform)transform;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
      }
      else
      {
        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
      }
    }

    public virtual void SetLocalPositionZ(float z)
    {
      if (transform is RectTransform)
      {
        throw new System.NotImplementedException("RectTransfrom does not have anchoredPosition.z");
      }
      else
      {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
      }
    }

    public virtual void SetRotation(Quaternion rotation)
    {
      transform.rotation = rotation;
    }

    public virtual void SetRotation(Vector3 eulerAngles)
    {
      transform.eulerAngles = eulerAngles;
    }

    public virtual void SetLocalRotation(Quaternion rotation)
    {
      transform.localRotation = rotation;
    }

    public virtual void SetLocalRotation(Vector3 eulerAngles)
    {
      transform.localEulerAngles = eulerAngles;
    }

    public virtual void SetRotationX(float x)
    {
      transform.eulerAngles = new Vector3(x, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    public virtual void SetRotationY(float y)
    {
      transform.eulerAngles = new Vector3(transform.eulerAngles.x, y, transform.eulerAngles.z);
    }

    public virtual void SetRotationZ(float z)
    {
      transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, z);
    }

    public virtual void SetLocalRotationX(float x)
    {
      transform.localEulerAngles = new Vector3(x, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    public virtual void SetLocalRotationY(float y)
    {
      transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, y, transform.localEulerAngles.z);
    }

    public virtual void SetLocalRotationZ(float z)
    {
      transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, z);
    }

    public virtual void SetScale(Vector3 scale)
    {
      transform.localScale = scale;
    }

    public virtual void SetScale(float scale)
    {
      transform.localScale = new Vector3(scale, scale, scale);
    }

    public virtual void SetScale(float x, float y, float z)
    {
      transform.localScale = new Vector3(x, y, z);
    }

    public virtual void SetScaleX(float x)
    {
      transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    public virtual void SetScaleY(float y)
    {
      transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
    }

    public virtual void SetScaleZ(float z)
    {
      transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
    }
  }
}
