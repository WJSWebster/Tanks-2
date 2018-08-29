using UnityEngine;

[System.Serializable]  // lets you embed a class (Sound) with sub properties in the inspector within another component (Audio Manager)
public class Song
{
    public string name;
    public AudioClip clip;

    // a value that we populate in AudioManager, so hide from inspector
    [HideInInspector]
    public AudioSource source;
}