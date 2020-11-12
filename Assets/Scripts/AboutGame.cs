using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AboutGame : MonoBehaviour
{
    // Start is called before the first frame update
    public string startgame;
    public string mainmenu;
    public AudioClip TitleBackgroundMusic;
    private AudioSource audioSource;
    public GameObject creditsMenu;
    public GameObject optionsMenu;
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
    }
    public void MuteAllSound()
    {
        AudioListener.volume = 0;
    }
    
    public void UnMuteAllSound()
    {
        AudioListener.volume = 1;
    }
        // Update is called once per frame
    void Update()
    {

    }
    public void MainMenu(){
        SceneManager.LoadScene(mainmenu);
    }
    public void StartGame(){
        SceneManager.LoadScene(startgame);
    }
    public void Credits(){
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(true);
        
    }
    public void Options(){
        creditsMenu.SetActive(false);
        optionsMenu.SetActive(true);
        audioSource.loop = true;
        audioSource.PlayOneShot(TitleBackgroundMusic);
        
    }
}
