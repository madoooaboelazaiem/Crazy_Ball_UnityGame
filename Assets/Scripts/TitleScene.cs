using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    public AudioClip TitleBackgroundMusic;
    private AudioSource audioSource;
    public string newGameScene;
    public string aboutGameScene;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.loop = true;
        audioSource.PlayOneShot(TitleBackgroundMusic);
        // GetComponent<AboutGame>().onClick.AddListener(AboutGame);  
        // GetComponent<NewGame>().onClick.AddListener(NewGame);  
        // GetComponent<QuitGame>().onClick.AddListener(QuitGame);  

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Options(){
        Debug.Log("About    " + aboutGameScene);
        SceneManager.LoadScene(aboutGameScene);
    }
    public void NewGame(){
        Debug.Log("New    " + newGameScene);
        SceneManager.LoadScene(newGameScene);
    }
    public void QuitGame(){
        Application.Quit();
            }
}
