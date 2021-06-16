using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSliderUI : MonoBehaviour
{
    [SerializeField] private string m_StartMusicVolume = "StartMusicVolume";
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private Slider m_Slider;
    [SerializeField] private float m_Multiplier = 30f;

    private void Awake()
    {
        m_Slider.onValueChanged.AddListener( delegate { HandleSliderValueChange(m_Slider.value); } );
    }

    private void HandleSliderValueChange(float value)
    {
        m_AudioMixer.SetFloat(m_StartMusicVolume, Mathf.Log10(value) * m_Multiplier);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
