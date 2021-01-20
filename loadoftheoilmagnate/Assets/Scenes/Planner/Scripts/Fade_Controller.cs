using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade_Controller : MonoBehaviour
{
    
    [SerializeField]
    private float fadeSpeed = 0.02f;
    [SerializeField]
    private float red, green, blue, alfa;   

    public bool isFadeOut = false;  

    Image fade_Image;

    // Start is called before the first frame update
    void Start()
    {
        fade_Image = GetComponent<Image>();
        red = fade_Image.color.r;
        green = fade_Image.color.g;
        blue = fade_Image.color.b;
        alfa = fade_Image.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeOut)
        {
            StartFadeOut();
        }
    }
    void StartFadeOut()
    {
        fade_Image.enabled = true; 
        alfa += fadeSpeed;        
        SetAlpha();              
        if (alfa >= 1)
        {             
            isFadeOut = false;
            SceneManager.LoadScene("Title");
        }
    }
    void SetAlpha()
    {
        fade_Image.color = new Color(red, green, blue, alfa);
    }
}
