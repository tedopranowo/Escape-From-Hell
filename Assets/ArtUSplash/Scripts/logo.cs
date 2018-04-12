using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class logo : MonoBehaviour {

	public float startScale;
	public float fadeOutDelay;
	int scaleIndex;


	[System.Serializable]
	public class scalingFactors : System.Object
	{
		public float scale;
		public float scaleSpeed;
	}

	public scalingFactors[] scalingPoints;

	[System.Serializable]
	public class graphicPlanes: System.Object
	{
		public GameObject graphicObject;
		public float fadeInDelay;
		public float fadeInSpeed;
		public float fadeOutDelay;
	}

	public graphicPlanes[] allGraphicPlanes;
	List<GameObject> allBumperObjects = new List<GameObject>();
	public string nextSceneName;

	// Use this for initialization
	void Start () {

		foreach (var graphic in allGraphicPlanes) 
		{
			Color startAlphaColor = new Vector4 (1f, 1f, 1f, 0f);
			graphic.graphicObject.GetComponent<MeshRenderer>().material.color = startAlphaColor;
			allBumperObjects.Add(graphic.graphicObject);
		}
		allBumperObjects.Add (gameObject);
		GetComponent<MeshRenderer> ().enabled = false;
		transform.localScale = new Vector3 (startScale, startScale, startScale);
		StartCoroutine (startProcess ());
	}
	
	// Update is called once per frame
	void Update () {

	
	}

	IEnumerator startProcess()
	{
		//delay before start
		yield return new WaitForSeconds (1f);
		StartCoroutine(logoScale());
	}
		
	IEnumerator logoScale()
	{

		//This will run until the logo finishs its animation

		GetComponent<MeshRenderer> ().enabled = true;

		while (scaleIndex < scalingPoints.Length) 
		{
			Vector3 scaleXfer;
			float scaleFactor = 0f;
			Vector3 currentScale = transform.localScale;
			scaleXfer = new Vector3(scalingPoints[scaleIndex].scale, scalingPoints[scaleIndex].scale, scalingPoints[scaleIndex].scale);
			while (transform.localScale != scaleXfer) 
			{
				scaleFactor += Time.deltaTime * scalingPoints[scaleIndex].scaleSpeed;
				transform.localScale = Vector3.Lerp (currentScale, scaleXfer, scaleFactor);
				yield return new WaitForEndOfFrame ();
			}
			scaleIndex++;

		}

		foreach (var graphic in allGraphicPlanes) 
		{
			StartCoroutine (fadeInFunc (graphic));
		}

		yield return new WaitForSeconds (fadeOutDelay);

		foreach (var bumperObj in allBumperObjects) 
		{
			StartCoroutine (fadeOutFunc (bumperObj));

		}

	}


	IEnumerator fadeInFunc (graphicPlanes graphicFadeIn)
	{
		yield return new WaitForSeconds (graphicFadeIn.fadeInDelay);

		float fadeInFactor = 0f;
		float alphaValue = 0f;
		Color alphaColor = new Vector4 (1f, 1f, 1f, 0f);

		while (alphaValue < 1) 
		{
			fadeInFactor += Time.deltaTime * graphicFadeIn.fadeInSpeed;
			alphaValue = Mathf.Lerp (0f, 1f, fadeInFactor);
			alphaColor.a = alphaValue;
			graphicFadeIn.graphicObject.GetComponent<MeshRenderer>().material.color = alphaColor;
			yield return new WaitForEndOfFrame();
		}

	}

	IEnumerator fadeOutFunc(GameObject graphicFadeOut)
	{
		float fadeOutFactor = 0f;
		float fadeOutSpeed = 1.5f;
		float alphaValue = 1f;
		Color alphaColor = new Vector4 (1f, 1f, 1f, 1f);

		while (alphaValue > 0) 
		{
			fadeOutFactor += Time.deltaTime * fadeOutSpeed;
			alphaValue = Mathf.Lerp (1f, 0f, fadeOutFactor);
			alphaColor.a = alphaValue;
			graphicFadeOut.GetComponent<MeshRenderer>().material.color = alphaColor;
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds (1f); 

		if (graphicFadeOut == gameObject) 
		{

			loadScene ();
		}

	}

	void loadScene()
	{
		if (nextSceneName != "") 
		{
			SceneManager.LoadScene (nextSceneName, LoadSceneMode.Single);
		}

	}
}
