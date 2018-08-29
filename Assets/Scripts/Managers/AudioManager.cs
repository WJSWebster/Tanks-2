using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class AudioManager : MonoBehaviour
{
    private static AudioManager m_Instance; // either a previous or the current instance of this class
    private bool m_IsSceneTransitioning = false;  // whether or not we are currently transitioning between scenes
    private int m_NextScene;                // The build index of the scene that we are now transitioning into 
    private float m_TransitionTime;         // The amount of time that a transition is meant to take
    private Stopwatch m_TransitionTimer;    // a stop watch used to calculate progress through a transition

    public AudioMixerGroup m_MixerGroup;    // the Audio Mixer group which these songs will belong to
    public Song[] m_MenuMusic;              // an array of songs that are played in the menus

    void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (m_Instance != null)  // if there is already another instance of an audio manager, then this one is not needed
            {
                // destroy this Game Object
                DestroyImmediate(gameObject);
            }
            else  // this is the first time this script has been called to:
            {
                // assigning this iteration of the script as the instance & 
                // making it not destroyed on the next scene load
                m_Instance = this;
                DontDestroyOnLoad(gameObject);

                // for each element in the MenuMusic array, create a relevant
                // song object and populate it, set it to loop, as well as add
                // it to the correct Audio Mixer group
                for (int i = 0; i < m_MenuMusic.Length; i++)
                {
                    Song song = m_MenuMusic[i];

                    song.source = gameObject.AddComponent<AudioSource>();
                    song.source.clip = song.clip;
                    song.source.loop = true;
                    song.source.outputAudioMixerGroup = m_MixerGroup;

                    if (i != SceneManager.GetActiveScene().buildIndex)
                    {
                        song.source.volume = 0f;
                    }
                    // else the song is the one for this scene, and volume is set to full by default

                    // plays all songs, whether they are to be heard or not, 
                    // so that they can be faded in and out without restarting song
                    song.source.Play();
                }
            }
        }
        else /*if (m_Instance != null)*/  // is not a scene that requires this audio manager
        {
            UnityEngine.Debug.Log("Destroying instance");

            DestroyImmediate(m_Instance);
            DestroyImmediate(this);
        }
    }

    // called to from LerpBetween, when a transition (or lerp) 
    // between scenes is about to begin
    public void TransitionScene(int nextScene, float transitionTime)
    {
        m_IsSceneTransitioning = true;
        m_NextScene = nextScene;
        m_TransitionTime = transitionTime;

        m_TransitionTimer = new Stopwatch();
        m_TransitionTimer.Start();
    }

    private void EndTransition()
    {
        m_IsSceneTransitioning = false;

        // if transition has ended, immediately set the relevant tracks
        // volume to full and all others to zero
        for (int i = 0; i < m_MenuMusic.Length; i++)
        {
            if(i == m_NextScene)
            {
                m_MenuMusic[i].source.volume = 1f;
            }
            else
            {
                m_MenuMusic[i].source.volume = 0f;
            }
        }

        // stop the time, as no longer being used, and reset for next time
        m_TransitionTimer.Stop();
        m_TransitionTimer.Reset();
    }

    private void Update()
    {
        if(m_IsSceneTransitioning)
        {
            float elapsedTime = (float)m_TransitionTimer.Elapsed.TotalSeconds;

            if (elapsedTime > m_TransitionTime)  // if transition has finished
            {
                EndTransition();
                return;
            }
            // else, time of transition has not yet fully elapsed:

            // declares and assigns a var of the percentage through the transition we are 
            // (and as such, the volume of the music which we are transitioning into), 
            // aswell as the inverse percentage
            float percentageFromLastScene = elapsedTime / m_TransitionTime;
            float percentageToNextScene = 1 - percentageFromLastScene;

            // sets the two track's volumes to these values, respectively
            m_MenuMusic[m_NextScene].source.volume = percentageFromLastScene;
            m_MenuMusic[m_NextScene - 1].source.volume = percentageToNextScene; // TODO: hard-coded

            UnityEngine.Debug.Log("Music1.Volume: " + m_MenuMusic[m_NextScene - 1].source.volume + ", Music2.Volume: " + m_MenuMusic[m_NextScene].source.volume);
        }
    }
}
