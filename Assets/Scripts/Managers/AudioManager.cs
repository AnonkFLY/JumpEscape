using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    private AudioClip[] soundEffect;
    [SerializeField]
    private GameObject musicBox;
    private List<MusicBox> _over = new List<MusicBox>();
    private AudioSource bgmComponet;
    private bool open = true;
    private void Start()
    {
        Instance = this;
        bgmComponet = GetComponent<AudioSource>();
    }
    public void PlaySoundEffect(int i)
    {
        if (!open)
            return;
        GetMusicBox().Init(soundEffect[i]);
    }
    private MusicBox GetMusicBox()
    {
        var i = _over.Count;
        if (i < 1)
        {
            return InstanceMusicBox();
        }
        var result = _over[i - 1];
        _over.RemoveAt(i - 1);
        return result;
    }
    public void SetMusic(bool open)
    {
        this.open = open;
        if (open)
        {
            if (!bgmComponet.isPlaying)
                bgmComponet.Play();
            else
                bgmComponet.UnPause();
        }
        else
        {
            bgmComponet.Pause();
        }
    }
    private MusicBox InstanceMusicBox()
    {
        var result = Instantiate(musicBox, transform).GetComponent<MusicBox>();
        result.onOver += OnOverEvent;
        return result;
    }
    private void OnOverEvent(MusicBox musicBox)
    {
        _over.Add(musicBox);
    }
}
