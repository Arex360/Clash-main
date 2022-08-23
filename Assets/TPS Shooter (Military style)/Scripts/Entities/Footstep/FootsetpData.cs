using UnityEngine;

namespace TPSShooter
{

    [CreateAssetMenu]
    public class FootsetpData : ScriptableObject
    {
        public TextureType[] textureTypes;
    }

    [System.Serializable]
    public class TextureType
    {
        public string name;
        public Texture[] textures;
        public AudioClip[] footstepSounds;
    }

}