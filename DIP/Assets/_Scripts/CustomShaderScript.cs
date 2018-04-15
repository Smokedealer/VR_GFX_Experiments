using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CustomShaderScript : MonoBehaviour
{

	public Material effectMaterial;

	public float red = 0;
	
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, effectMaterial);
	}

	private void Update()
	{
	}
}
