using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void OpenScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame() {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }
}
