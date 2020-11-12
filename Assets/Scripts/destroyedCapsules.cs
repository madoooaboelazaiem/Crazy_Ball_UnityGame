using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class destroyedCapsules : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {  
        float x = Random.Range(-4,4);
        float v = Input.GetAxis("Vertical");
        // float h = Input.GetAxis("Horizontal");
        transform.Translate(0,0,GameObject.Find("Car").GetComponent<GameEngine>().speed);
    

    }
    
    
}
