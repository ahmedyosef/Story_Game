using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class Story_Manger : MonoBehaviour
{
    public List<UnityEvent2> steps;
    public int step_Numper;
    public VideoPlayer videoPlayer;
    public void Start()
    {
        payStep_Numpr(0);
    }
    

    public void payStep_Numpr(int stepNum)
    {
        step_Numper=stepNum;
        steps[stepNum].Invoke();
    }
    public void next()
    {
        step_Numper += 1;
        steps[step_Numper].Invoke();
    }
    public void back()
    {
        step_Numper -= 1;
        steps[step_Numper].Invoke();
    }
    IEnumerator playvideoandwaite()
    {
        videoPlayer.Play();
        while (videoPlayer.isPlaying) {
            yield return null;
        }
        next();
    }
    void test()
    {

    }
    public void OnVideoFinished(VideoPlayer vp)
    {
        
        vp.loopPointReached += OnVideoFinished;
        vp.isLooping = false;
        Invoke("test", 5f);
        StartCoroutine(playvideoandwaite());
        
    }

}
