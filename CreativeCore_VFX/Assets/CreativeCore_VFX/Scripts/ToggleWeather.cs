using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWeather : MonoBehaviour
{
    public KeyCode switchUp = KeyCode.UpArrow;
    public KeyCode switchDown = KeyCode.DownArrow;

    public ParticleSystem[] weatherParticleSystems;

    int currentWeatherIndex = -1;

    private void Start()
    { }

    void Update()
    {

        if (Input.GetKeyDown(switchUp))
        {
            SwitchWeather(true);
        }
        else if (Input.GetKeyDown(switchDown))
        {
            SwitchWeather(false);
        }
    }

    /// <summary>
    /// Cycle weather up or down, if index rolls over, no weather
    /// </summary>
    /// <param name="up">true if go up, false for down</param>
    private void SwitchWeather(bool up)
    {
        int shift = up ? 1 : -1;

        if (currentWeatherIndex >= 0 && currentWeatherIndex < weatherParticleSystems.Length)
        {
            weatherParticleSystems[currentWeatherIndex].Stop();
            weatherParticleSystems[currentWeatherIndex].Clear();
        }

        currentWeatherIndex += shift;

        if (currentWeatherIndex >= 0 && currentWeatherIndex < weatherParticleSystems.Length)
        {
            weatherParticleSystems[currentWeatherIndex].Play();
        }
        

        if (currentWeatherIndex >= weatherParticleSystems.Length)
        {
            currentWeatherIndex = -1;
        }
        else if (currentWeatherIndex < -1)
        {
            currentWeatherIndex = weatherParticleSystems.Length - 1;
        }
    }


    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), "Use up/down arrows to cycle weather");
        GUI.Label(new Rect(10, 30, 300, 20), "Current weather index: " + currentWeatherIndex.ToString());
        GUI.Label(new Rect(10, 50, 300, 20), "Use space to toggle fire");
    }
}