using UnityEngine; 

namespace MalbersAnimations.Scriptables
{
    ///<summary> Store a list of Materials</summary>
    [CreateAssetMenu(menuName = "Malbers Animations/Collections/Audio Source Set", order = 1000)]
    public class AudioClipListVar : ScriptableList<AudioClip>
    {
        public void Play(AudioSource source)
        {
            var clip = Item_GetRandom();
            source.clip = clip;
            source.Play();
        }


        public void Play()
        {
            var NewGO = new GameObject() { name = "Audio [" + this.name +"]"};
            var source = NewGO.AddComponent<AudioSource>();
            source.spatialBlend = 1f;
            var clip = Item_GetRandom();
            source.clip = clip;
            source.Play();
        }
    }

    [System.Serializable]
    public class AudioClipReference
    {
        public bool UseConstant = true;

        public AudioClip ConstantValue;
        [RequiredField] public AudioClipListVar Variable;

        public AudioClip GetValue() => UseConstant ? ConstantValue : Variable.Item_GetRandom();

        public AudioClip GetValue(int index) => UseConstant ? ConstantValue : Variable.Item_Get(index);

        public AudioClip GetValue(string name) => UseConstant ? ConstantValue : Variable.Item_Get(name);

        public void Play(AudioSource source)
        {
            var clip = GetValue();
            source.clip = clip;
            source.Play();
        }
    }


}