using UnityEngine;
using AudioSystem;

public class IndependentSoundPlayer : MonoBehaviour
{
    [SerializeField] string _name;
    [SerializeField] Vector2 _pitchRange;
    [SerializeField] bool _playOnAwake;
    [SerializeField] bool _randomPitch;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_playOnAwake)
        {
            if (_randomPitch)
                PlayRandomPitchAudio(_name, _pitchRange.x, _pitchRange.y);
            else
                PlayAudio(name);
        }
    }

    public void PlayAudio(string name)
    {
        AudioManager.instance.PlayAudio(name);
    }

    public void PlayRandomPitchAudio(string name, float min, float max)
    {
        AudioManager.instance.PlayRandomPitchAudio(name, min, max);
    }
}