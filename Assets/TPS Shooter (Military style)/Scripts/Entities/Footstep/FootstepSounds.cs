using System.Collections;
using UnityEngine;

namespace TPSShooter
{
  // Add this script to GameObject that has to play Footstep sounds.
  // 
  // FootStep sound will be played when AnimationEvent (name = PlayFootstepSound) is called.
  public class FootstepSounds : MonoBehaviour
  {
    public LayerMask footstepLayers = 1 << 0;
    public AudioSource audioSource;
    public FootsetpData footsetpData;

    // Used for not overlapping sound on sound. 
    //
    // For example, in blend trees AnimationEvent can be called several times
    // in the same moment as two animations are blended
    private bool canPlaySound = true;

    #region AnimationEvent

    private void PlayFootstepSound()
    {
      if (canPlaySound == false) return;

      StartCoroutine(ElapseTime());

      RaycastHit hit;
      Vector3 start = transform.position + transform.up;
      Vector3 dir = Vector3.down;

      if (Physics.Raycast(start, dir, out hit, 1.3f, footstepLayers))
      {
        if (hit.collider.GetComponent<MeshRenderer>())
        {
          PlayMeshSound(hit.collider.GetComponent<MeshRenderer>());
        }
        else if (hit.collider.GetComponent<Terrain>())
        {
          PlayTerrainSound(hit.collider.GetComponent<Terrain>(), hit.point);
        }
      }
    }

    #endregion

    private IEnumerator ElapseTime()
    {
      canPlaySound = false;
      float elapsedTime = 0.15f;

      while (elapsedTime >= 0)
      {
        elapsedTime -= Time.deltaTime;
        yield return null;
      }

      canPlaySound = true;
    }

    // Play sound when the player on MeshRenderer
    private void PlayMeshSound(MeshRenderer meshRenderer)
    {
      if (footsetpData.textureTypes.Length > 0)
      {
        foreach (TextureType type in footsetpData.textureTypes)
        {
          if (type.footstepSounds.Length == 0)
          {
            return;
          }

          foreach (Texture tex in type.textures)
          {
            if (meshRenderer.material.mainTexture == tex)
            {
              PlaySound(audioSource, type.footstepSounds[Random.Range(0, type.footstepSounds.Length)], true, 0.9f, 1f);
              return;
            }
          }
        }
      }
    }

    // Play sound when the player on Terrain
    private void PlayTerrainSound(Terrain terrain, Vector3 hitPoint)
    {
      if (footsetpData.textureTypes.Length > 0)
      {
        int textureIndex = GetTerrainTexture(hitPoint);

        foreach (TextureType type in footsetpData.textureTypes)
        {
          if (type.footstepSounds.Length == 0)
          {
            return;
          }

          foreach (Texture tex in type.textures)
          {
            if (terrain.terrainData.splatPrototypes[textureIndex].texture == tex)
            {
              PlaySound(audioSource, type.footstepSounds[Random.Range(0, type.footstepSounds.Length)], true, 0.9f, 1.1f);
              return;
            }
          }
        }
      }
    }

    private void PlaySound(AudioSource audioS, AudioClip clip, bool randomizePitch = false, float randomPitchMin = 1, float randomPitchMax = 1)
    {
      audioS.clip = clip;

      if (randomizePitch == true)
        audioS.pitch = Random.Range(randomPitchMin, randomPitchMax);

      audioS.Play();
    }

    // Returns an array containing the relative mix of textures
    // on the main terrain at this world position.
    //
    // The number of values in the array will equal the number
    // of textures added to the terrain.
    private static float[] GetTextureMix(Vector3 worldPos)
    {
      Terrain terrain = Terrain.activeTerrain;
      TerrainData terrainData = terrain.terrainData;
      Vector3 terrainPos = terrain.transform.position;

      // calculate which splat map cell the worldPos falls within (ignoring y)
      int mapX = (int)(((worldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
      int mapZ = (int)(((worldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);

      // get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
      float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

      // extract the 3D array data to a 1D array:
      float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];
      for (int n = 0; n < cellMix.Length; ++n)
      {
        cellMix[n] = splatmapData[0, 0, n];
      }

      return cellMix;

    }

    // Returns the zero-based index of the most dominant texture
    // on the main terrain at this world position.
    private static int GetTerrainTexture(Vector3 worldPos)
    {
      float[] mix = GetTextureMix(worldPos);

      float maxMix = 0;
      int maxIndex = 0;

      // loop through each mix value and find the maximum
      for (int n = 0; n < mix.Length; ++n)
      {
        if (mix[n] > maxMix)
        {
          maxIndex = n;
          maxMix = mix[n];
        }
      }

      return maxIndex;
    }
  }
}
