using UnityEngine;
using System.Collections;

public class VignetteManager : MonoBehaviour 
{
	public Color VignetteColor = Color.black;
	public Color ComboVignetteColor = Color.white;
	
	public float IntensityBias = 0.5f;
	public float MinimumIntensity = 0.5f;
	public float MaximumIntensity = 1.0f;
	public float BlendSpeed = 1.0f;
	public float BlendUpdateRate = 1f / 15f;
	
	private float currentValue = 0f;
	private float targetValue = 0f;
	
	private float colorTime;
	private Color currentColor;
	private Color lastColor;
	private Color targetColor;
		
	// Use this for initialization
	void Start() 
	{
		lastColor = targetColor = currentColor = VignetteColor;
		renderer.materials[0].SetColor("_Color", currentColor);
		
		currentValue = targetValue = MinimumIntensity;
		Shader.SetGlobalFloat("_Intensity", MinimumIntensity);
		
		StartCoroutine(DoBlending());
	}
	
	float Bias(float _bias, float _value)
	{
		// Simple bias function y(x) = x^(log(B)/log(0.5))
		return Mathf.Pow(_value, Mathf.Log(_bias) / Mathf.Log(0.5f));
	}
	
	IEnumerator DoBlending()
	{
		while (true)
		{
			float rate = 1f / BlendSpeed * BlendUpdateRate;
			
			if (currentValue == targetValue)
			{
				targetValue = Random.Range(MinimumIntensity, MaximumIntensity);
			}
			else
			{
				float dir = Mathf.Sign(targetValue - currentValue);
				currentValue += dir * rate;
				
				if (dir != Mathf.Sign(targetValue - currentValue))
				{
					currentValue = targetValue;
				}
				
				float intensity = Bias(IntensityBias, currentValue);
				Shader.SetGlobalFloat("_Intensity", intensity);
				//UnityEngine.Debug.Log("Current intensity " + intensity.ToString() + " target intensity " + targetValue.ToString());
			}
			
			if (colorTime < 0f)
			{
				colorTime += rate;
				currentColor = Color.Lerp(lastColor, targetColor, colorTime);
				renderer.materials[0].SetColor("_Color", currentColor);
			}
			
			yield return new WaitForSeconds(BlendUpdateRate);
		}
	}
	
	public void SetVignetteColor(Color color)
	{
		lastColor = currentColor;
		targetColor = color;
		colorTime = 0f;
	}
}
