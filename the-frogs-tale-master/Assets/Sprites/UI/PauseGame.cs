using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {
    private bool isPaused;
    // public GameObject healthBar;

    // Start is called before the first frame update
    void Start() {
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Pause() {
        if(!isPaused) {
            Time.timeScale = 0f;
            isPaused = true;
            // healthBar.setActive(false);
        } else {
            Time.timeScale = 1f;
            isPaused = false;
            // healthBar.setActive(true);
        }
    }
}
