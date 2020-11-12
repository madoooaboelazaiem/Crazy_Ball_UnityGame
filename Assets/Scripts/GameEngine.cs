using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameEngine : MonoBehaviour
{
    public float speed;
    public float shiftSpeed;
    public Text scoreText;
    public Text heartScore;
    public int newCapsule = 10;
    public GameObject[] PrefabCapsule;
    public GameObject PrefabHeart;
    public GameObject[] PrefabBrick;

    public Transform startPos;
    public GameObject capsule;
    public GameObject[] capsules;
    public GameObject[] hearts;
    public GameObject[] bricks;

    public int capsuleAmount;
    public int heartGeneration = 0;
    public int brickGeneration = 0;

    public int score = 0 ;
    public int lastScore = 0 ;

    public int health = 3 ;
    public bool flipMode = false;
    public float flipValue = 0.001f;
    public float flipSpeed = 2.0f;
    public float cameraValueY = 1.2f;
    public float cameraValueZ = 4f;
    float lastTime;
    float changeColorTime;
    float changeColorPeriod = 15.0f;
    public string newGameScene;
    public string mainSceneMenu;
    public GameObject pauseMenu;
    public bool isPaused=true;
    public float level=50;
    public GameObject gameOverMenu;
    public GameObject newColorObject;
    public GameObject camera1;
    public GameObject camera2;
    public AudioListener camera1Audio;
    public AudioListener camera2Audio;
    public int camPosition = 0;
    public AudioClip GameBackgroundMusic;
    public AudioClip TitleBackgroundMusic;
    public AudioSource audioSource;
    public AudioClip collectCoin;
    public AudioClip collectHP;
    public AudioClip hitObstacle;
    public AudioClip flipModeSong;
    public int totalScoreSoFar = 0;
    public Rigidbody2D rb;
    public float acc_x;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastTime = Time.time;
        camera1Audio = camera1.GetComponent<AudioListener>();
        camera2Audio = camera2.GetComponent<AudioListener>();
        camera2Audio.enabled = false;
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // 3.62 for flipped mode and 0.528 Normal camera view 1.82y -12z flip -0.66y -8z 
    }

    // Update is called once per frame
    void Update()
    {  
        if (Input.GetKeyDown(KeyCode.C))
        {
            switchCam();
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            score+=10;
            totalScoreSoFar+=10;
            scoreText.text = "Score: " + score;
        }
        if(Input.GetKeyDown(KeyCode.E)){
            if(health<3){
            health+=1;
            heartScore.text = "Health: " + health;
            }

        }
        if(Input.GetKeyDown(KeyCode.R)){newColorObject = GameObject.Instantiate (PrefabCapsule[Random.Range(0,PrefabCapsule.Length)]) as GameObject; 
            Color newCarColor = newColorObject.gameObject.GetComponent<Renderer> ().material.color;
            GameObject.Find("Car").GetComponent<Renderer> ().material.color = newCarColor;
            GameObject.Destroy(newColorObject.gameObject);
            }
        if(Input.GetKeyDown(KeyCode.Escape)&&health!=0){

            if(isPaused){
                Resume();
            }
            else{
                Pause();
            }
        }
        if(health==0){
                AudioSource carAudio = GameObject.Find("Car").GetComponent<AudioSource>();
                carAudio.Stop();
                camera2Audio.enabled = false;
                camera1Audio.enabled = false;
                gameOverMenu.SetActive(true);
                Time.timeScale=0f;
        }
        if(health !=0 && !isPaused){
            Time.timeScale=1f;
        }
        
        if(Time.timeScale==1f){
        float x = Random.Range(-4,4);
        // float h = Input.GetAxis("Horizontal");
        transform.Translate(0,0,speed);
        // transform.Rotate(0,h*speed* Time.deltaTime*10,0);

        float h = Input.GetAxis("Horizontal");
        if (SystemInfo.deviceType == DeviceType.Desktop) 
        { 
            if(Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.D)){
                if(transform.localPosition.x<4){
                    // transform.Translate(h*shiftSpeed* Time.deltaTime,0,0);
                    transform.Translate(shiftSpeed,0,0);
                }
            }
            if(Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.A)){
                if(transform.localPosition.x>-4){
                    // transform.Translate(h*shiftSpeed* Time.deltaTime,0,0);
                    transform.Translate(-shiftSpeed,0,0);
                }

            }
        }
        else
        {
                // Player movement in mobile devices
            // Building of force vector 
            // Vector3 movement = new Vector3 (Input.acceleration.x, 0.0f, 0f);
            // // Adding force to rigidbody
            // GetComponent<Rigidbody>().AddForce(movement * speed * Time.deltaTime);
        // Vector3 dir = Vector3.zero;
        // dir.x = -Input.acceleration.y;
        // dir.z = Input.acceleration.x;
        // if (dir.x > dir.z)
        // {
        //     if(transform.localPosition.x>-4){
        //     transform.Translate(Input.acceleration.x, 0, -Input.acceleration.z);
        //     // transform.position += (Vector3.left * speed * Time.deltaTime)/10;
        //     }
        // }
        // else if (dir.x < dir.z)
        // {
        //     if(transform.localPosition.x<4){
        //     transform.Translate(Input.acceleration.x, 0, -Input.acceleration.z);
        //     // transform.position += (Vector3.right * speed * Time.deltaTime)/10;
        //     }
        // }
        // transform.position = new Vector2(Mathf.Clamp(transform.position.x,-4f,4f),transform.position.y);
            transform.Translate(Input.acceleration.x * speed * Time.deltaTime*30, 0, 0);
            if(transform.position.x < -4) 
                transform.position = new Vector3(-4,transform.position.y,transform.position.z);
            if(transform.position.x > 4) 
                transform.position = new Vector3(4,transform.position.y,transform.position.z);




        // if (x < -0.1f)
        // {
        //     rb.velocity = new Vector2(-shiftSpeed, 0);
        // }
        // else if (x > 0.1f)
        // {
        //     rb.velocity = new Vector2(shiftSpeed, 0);
        // }
        }
        if(Input.GetKey("space") && (Time.time - lastTime > 1.0f)){
            flip();
        }



        capsules = GameObject.FindGameObjectsWithTag("collider");
        foreach (GameObject cap in capsules)
        {
            if(cap.name == "Capsule (3)(Clone)")
            {
            if(cap.transform.position.z+5 < startPos.position.z){
                GameObject.Destroy(cap.gameObject);
            } //Do Something
            }
            if(cap.name == "Capsule (4)(Clone)")
            {
            if(cap.transform.position.z+5 < startPos.position.z){
                GameObject.Destroy(cap.gameObject);
            }//Do Something
            }
            if(cap.name == "Capsule (5)(Clone)")
            {
            if(cap.transform.position.z+5 < startPos.position.z){
                GameObject.Destroy(cap.gameObject);
            }//Do Something
            }
            
        }
        
        for(int i=capsules.Length;i<capsuleAmount;i++){
            capsule = Instantiate(PrefabCapsule[Random.Range(0,PrefabCapsule.Length)],new Vector3(x,startPos.position.y, startPos.position.z+newCapsule), Quaternion.identity);
            newCapsule+=150;
        }
        if(capsules.Length == capsuleAmount){
            newCapsule=50;
        }
        if(heartGeneration == 100){
            heartGeneration=0;
            GameObject heart = Instantiate(PrefabHeart,new Vector3(x,startPos.position.y, startPos.position.z+50), Quaternion.identity);
            heart.transform.Translate(0,0.2f,0);
        }
        hearts = GameObject.FindGameObjectsWithTag("heart");
        foreach (GameObject hea in hearts)
        {if(hea.name == "Heart(Clone)"){
            if(hea.transform.position.z+5 < startPos.position.z){
                GameObject.Destroy(hea.gameObject);
            }
        }}
        bricks = GameObject.FindGameObjectsWithTag("bricks");
        foreach (GameObject b in bricks)
        {if(b.name == "Cube(Clone)" || b.name == "Cube (1)(Clone)"|| b.name == "Cube (2)(Clone)"){
            if(b.transform.position.z+5 < startPos.position.z){
                GameObject.Destroy(b.gameObject);
            }
        }}
        if(brickGeneration == 200){
            int brickType = Random.Range(0,PrefabBrick.Length);
            // Debug.Log("    "+brickType+"    "+PrefabBrick.Length);
            if(brickType == 2){
                x = 0;
            }else if(brickType == 1){
                x = Random.Range(-2,3);
                x-=0.1f;
            }
            else{
                x = Random.Range(-3,4);

            }
            GameObject brick = Instantiate(PrefabBrick[brickType],new Vector3(x,0.7f, startPos.position.z+100), Quaternion.identity);
            brick.transform.Translate(0,0.2f,0);
            brickGeneration=0;
        }
        if(score<5){
            score = 0;
        }
        if(score!=0 && (totalScoreSoFar >= 50)){
            totalScoreSoFar = 0;
            if(speed>2.6f){
                speed=2.6f;
            }else{
            speed+=0.1f;
            }
        }
        //Change car color
        if (Time.time > changeColorTime ) {
            changeColorTime = Time.time + changeColorPeriod;
            newColorObject = GameObject.Instantiate (PrefabCapsule[Random.Range(0,PrefabCapsule.Length)]) as GameObject;
            Color newCarColor = newColorObject.gameObject.GetComponent<Renderer> ().material.color;
            GameObject.Find("Car").GetComponent<Renderer> ().material.color = newCarColor;
            GameObject.Destroy(newColorObject.gameObject);
        }
        heartGeneration+=1;
        brickGeneration+=1;

        }
        // else{
        //     PlayTitleTheme();
        // }


    }
    private void FixedUpdate(){
    }
    public void PlayGameTheme()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.PlayOneShot(TitleBackgroundMusic);
    }
    //     public void PlayTitleTheme()
    // {
    //     audioSource.Stop();
    //     audioSource.loop = true;
    //     audioSource.PlayOneShot(TitleBackgroundMusic);
    // }
    public void Resume(){
        audioSource.Stop();
        AudioSource carAudio = GameObject.Find("Car").GetComponent<AudioSource>();
        carAudio.loop = true;
        carAudio.Play();
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale=1f;
    }
    public void Pause(){
        GameObject.Find("Car").GetComponent<AudioSource>().Stop();
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.PlayOneShot(TitleBackgroundMusic);
        isPaused=true;
        Time.timeScale=0f;
        pauseMenu.SetActive(true);
    }
    public void flip(){
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
            audioSource.Stop();
            audioSource.PlayOneShot(flipModeSong);

            float v = Input.GetAxis("Vertical");
            if(flipMode){ //To Flip
                // if(camPosition == 0){ //camera1
                // if(camera1.transform.localPosition.y >0.6){
                //     camera1.transform.Translate(0,cameraValueY,0);

                // }else{
                //     camera1.transform.Translate(0,-cameraValueY,0);
                // }
                while(transform.localPosition.y > 0.5f){
                    // transform.Translate(0,-v*flipSpeed* Time.deltaTime,0);
                    transform.Translate(0,-flipValue,0);
                }
                camera1.transform.Translate(0,1,0);
                // transform.Translate(0.5F,0,0);
                // }else{
                //     while(transform.localPosition.y > 0.5f){
                //     // transform.Translate(0,-v*flipSpeed* Time.deltaTime,0);
                //         transform.Translate(0,-flipValue,0);
                // }                }
                flipMode = false;
            }else{ // To Normal
                // if(camPosition == 0){ //camera1
                    while(transform.localPosition.y <  3.6f){
                        transform.Translate(0,flipValue,0);
                            // transform.Translate(0,v*flipSpeed*Time.deltaTime,0);
                    }
                    camera1.transform.Translate(0,-1,0);
                // if(camera1.transform.localPosition.y <1.8){
                //     camera1.transform.Translate(0,cameraValueY,cameraValueZ);

                // }else{
                //     camera1.transform.Translate(0,-cameraValueY,-cameraValueZ);
                // }
                // }else{
                //     while(transform.localPosition.y <  3.6f){
                //         transform.Translate(0,flipValue,0);
                //             // transform.Translate(0,v*flipSpeed*Time.deltaTime,0);
                //     }
                // }
                // transform.Translate(0.5F,0,0);
                flipMode = true;
            }
            lastTime = Time.time;
    }
    public void NewGame(){
        // Application.LoadLevel(0);
        SceneManager.LoadScene(newGameScene);
    }
    public void QuitGame(){
        Application.Quit();
    }
    public void QuitMain(){       
        SceneManager.LoadScene(mainSceneMenu);
    }
    public void switchCam(){
        camera1Audio = camera1.GetComponent<AudioListener>();
        camera2Audio = camera2.GetComponent<AudioListener>();
        if (camPosition == 0)
        {
            camera1.SetActive(true);
            camera1Audio.enabled = true;

            camera2Audio.enabled = false;
            camera2.SetActive(false);
            camPosition = 1;
        }

        //Set camera position 2
        else
        {
            camera2.SetActive(true);
            camera2Audio.enabled = true;

            camera1Audio.enabled = false;
            camera1.SetActive(false);
            camPosition = 0;
        }        
    }
    void OnCollisionEnter(Collision collision) {
    }

    
    void OnTriggerEnter(Collider collider) { 
        if(collider.gameObject.CompareTag("bricks")){
                health-=1; 
                audioSource.Stop();
                audioSource.PlayOneShot(hitObstacle);
                heartScore.text = "Health: " + health;
                GameObject.Destroy(collider.gameObject);
        }
        // float x = Random.Range(-4,4);
        if(collider.gameObject.CompareTag("collider")){
            Color carColor = GameObject.Find("Car").GetComponent<Renderer> ().material.color;
            Color colliderCollor = collider.gameObject.GetComponent<Renderer> ().material.color;
            if(carColor.Equals(colliderCollor))
            {
                if(flipMode){
                    if(score>=5){
                    score-=5;
                    audioSource.Stop();
                    audioSource.PlayOneShot(hitObstacle);

                    }
                }else{
                    score+=10;
                    totalScoreSoFar+=10;
                    audioSource.Stop();
                    audioSource.PlayOneShot(collectCoin);
                    }
                // speed = (float)(speed + 0.01);
                // GetComponent<AudioSource>().Play(); Play Point Collected Sound Here
            }
            else{
                if(flipMode){
                    audioSource.Stop();
                    audioSource.PlayOneShot(collectCoin);
                    score+=10;
                    totalScoreSoFar+=10;
                    }else{                     
                    if(score>=5){
                    audioSource.Stop();
                    audioSource.PlayOneShot(hitObstacle);
                    score-=5;

                    }}
               // GetComponent<AudioSource>().Play(); Play Point Lost Sound Here
            } 
        GameObject.Destroy(collider.gameObject);
        // Play Sound
        // 
        scoreText.text = "Score: " + score;
        }
        if(collider.gameObject.CompareTag("heart")){
            audioSource.Stop();
            audioSource.PlayOneShot(collectHP);
            if(health<3){
                health+=1;
                // GetComponent<AudioSource>().Play(); Play Heart Sound Here
            }
            GameObject.Destroy(collider.gameObject);
            // GetComponent<AudioSource>().Play();
            heartScore.text = "Health: " + health;
        }
        
    }
}
