using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteMusic : MonoBehaviour
{
    public AudioSource music;
    private bool mute = false;
    // Start is called before the first frame update
    void Start()
    {
        // music.mute(false);
    }

    public void MuteMs() {
        if(!mute) {
            mute = true;
            music.Pause();
        } else {
            mute = false;
            music.Play();
        }
        // music.mute(mute);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
