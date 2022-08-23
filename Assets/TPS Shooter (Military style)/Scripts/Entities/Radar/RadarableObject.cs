using UnityEngine;
using UnityEngine.UI;

namespace TPSShooter.UI
{
  // Add this script to GameObject that has to be displayed on the Radar.
  public class RadarableObject : MonoBehaviour
  {
    public GameObject RadarableImagePrefab;

    private Radar _radar;

    // Created GameObject from prefab
    private GameObject _radarableImgObject;

    // Used by Radar when it defines local position of GameObject created from Prefab
    private RectTransform _createdRectTransform;
    public void SetRectLocalPosition(Vector3 position)
    {
      _createdRectTransform.localPosition = position;
    }

    public float ImageHalfHeight { get; private set; }

    private void OnValidate()
    {
      if (RadarableImagePrefab != null && RadarableImagePrefab.GetComponent<Image>() == null)
      {
        RadarableImagePrefab = null;
        Debug.LogError("RadarableObject: RadarableImagePrefab has to have Image Component.");
      }
    }

    private void Start()
    {
      InitializeRadarableObject();
    }

    private void InitializeRadarableObject()
    {
      _radar = Radar.GetInstance();
      if (_radar == null)
      {
        Debug.LogError("No radar on the scene.");
        return;
      }

      _radarableImgObject = Instantiate(RadarableImagePrefab);
      _radarableImgObject.transform.SetParent(_radar.GetRadarImage().transform);
      _radarableImgObject.transform.localScale = Vector3.one;

      _createdRectTransform = _radarableImgObject.GetComponent<Image>().rectTransform;

      ImageHalfHeight = RadarableImagePrefab.GetComponent<Image>().rectTransform.rect.height / 2;

      _radar.AddRadarableObject(this);
    }

    protected void DestroyRadarableObject()
    {
      Destroy(_radarableImgObject);

      _radar.RemoveRadarableObject(this);
    }
  }
}
