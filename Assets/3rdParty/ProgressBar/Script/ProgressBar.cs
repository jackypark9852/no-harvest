using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode]

public class ProgressBar : MonoBehaviour
{

    [Header("Title Setting")]
    public string Title;
    public Color TitleColor;
    public Font TitleFont;
    public int TitleFontSize = 10;

    [Header("Bar Setting")]
    public Color BarColor;   
    public Color BarBackGroundColor;
    public Sprite BarBackGroundSprite;
    [Range(0f, 100f)]  //|
    public int Alert = 20;
    public Color BarAlertColor;

    [Header("Sound Alert")]
    public AudioClip sound;
    public bool repeat = false;
    public float RepeatRate = 1f;

    private Image bar, barBackground;
    private float nextPlay;
    private AudioSource audiosource;
    private Text txtTitle;
    private float barValue;
    public float BarValue
    {
        get { return barValue; }

        set
        {
            value = Mathf.Clamp(value, 0, 100);
            barValue = value;
            UpdateValue(barValue);

        }
    }

    //|
    [SerializeField] TMP_Text text;
    [SerializeField] int losingFarmValue = 50;


    private void Awake()
    {
        bar = transform.Find("Bar").GetComponent<Image>();
        barBackground = GetComponent<Image>();
        barBackground = transform.Find("BarBackground").GetComponent<Image>();
        audiosource = GetComponent<AudioSource>();
    }

    private void Start()
    {

        bar.color = BarColor;
        barBackground.color = BarBackGroundColor; 
        barBackground.sprite = BarBackGroundSprite;

        UpdateValue(barValue);


    }

    void UpdateValue(float val)
    {
        text.text = $"{Mathf.Min(losingFarmValue, Mathf.RoundToInt(val * losingFarmValue))}/{losingFarmValue} PLANTED";  //| Hard-coded
        val *= 100;  //|
        bar.fillAmount = val / 100;

        if (Alert >= val)
        {
            bar.color = BarAlertColor;
        }
        else
        {
            bar.color = BarColor;
        }
    }


    private void Update()
    {
        if (!Application.isPlaying)
        {           
            UpdateValue(0.5f);  //|

            bar.color = BarColor;
            barBackground.color = BarBackGroundColor;

            barBackground.sprite = BarBackGroundSprite;           
        }
        else
        {
            if (Alert >= barValue && Time.time > nextPlay)
            {
                nextPlay = Time.time + RepeatRate;
                //| audiosource.PlayOneShot(sound);
            }
        }
    }

}
