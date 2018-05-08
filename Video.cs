using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;






public class Video : MonoBehaviour {
	public VideoClip videoToPlay;
	private VideoPlayer videoPlayer;
	private VideoSource videoSource; 
	public RawImage image;
	private AudioSource audioSource; 

public	void StartLeft(){
		Application.runInBackground = true;
		StartCoroutine (playVideo ());
	}

	IEnumerator playVideo(){

	videoPlayer=gameObject.AddComponent<VideoPlayer>();
	audioSource=gameObject.AddComponent<AudioSource>();

	videoPlayer.playOnAwake=false;
	audioSource.playOnAwake=false;
	audioSource.Pause();

	videoPlayer.source =VideoSource.VideoClip;


	videoPlayer.audioOutputMode= VideoAudioOutputMode.AudioSource;

	videoPlayer.EnableAudioTrack(0, true);
	videoPlayer.SetTargetAudioSource(0,audioSource);

	videoPlayer.clip=videoToPlay;
	videoPlayer.Prepare();

	while (!videoPlayer.isPrepared)
	{
		yield return null;
	}

	Debug.Log("done prep video");

	image.texture=videoPlayer.texture;

		videoPlayer.Play ();

	audioSource.Play();

	Debug.Log("playing video");
	while(videoPlayer.isPlaying)
	{
		Debug.LogWarning("video time: " +Mathf.FloorToInt((float)videoPlayer.time));
		yield return null;
	}
	Debug.Log("done playing Video");
	}

	void Update(){
	}
}





