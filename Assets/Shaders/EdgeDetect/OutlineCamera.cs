using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class OutlineCamera : MonoBehaviour 
{
	private static OutlineCamera _instance;
	public static OutlineCamera instance
	{
		// If singleton not set, find it.
		get
		{
			if(_instance == null)
				return (_instance = FindObjectOfType(typeof(OutlineCamera)) as OutlineCamera);

			return _instance;
		}
	}

	private readonly List<Outline> outlines = new List<Outline>();

	[Range(1.0f, 5.0f)] [SerializeField]
	private float lineThickness = 1.5f;

	[Range(0, 10)] [SerializeField]
	private float lineIntensity = 1.0f;

	[Range(0, 1)] [SerializeField]
	private float fillAmount = 0.25f;

	[SerializeField]
	private Color lineColor0 = Color.red;
	[SerializeField]
	private Color lineColor1 = Color.green;
	[SerializeField]
	private Color lineColor2 = Color.blue;

	[SerializeField] 
	private bool additiveRendering = false;

	[SerializeField]
	private bool backfaceCulling = true;

	[Header("These settings may affect performance.")]
	[SerializeField]
	private bool cornerOutlines = false;
	[SerializeField]
	private bool addLinesBetweenColors = false;

	[Header("Advances settings")]
	[SerializeField]
	private bool scaleWithScreenSize = true;
	[Range(0.1f, 0.9f)] [SerializeField]
	private float alphaCutoff = 0.5f;
	[SerializeField]
	private bool flipY = false;
	[SerializeField]
	private Camera sourceCamera;

	private Camera outlineCamera;
	private Material outlineMaterial1;
	private Material outlineMaterial2;
	private Material outlineMaterial3;
	private Material outlineEraseMaterial;

	private Shader outlineShader;
	private Shader outlineBufferShader;

	private Material outlineShaderMaterial;
	private RenderTexture rt;
	private RenderTexture rtExtra;

	private CommandBuffer commandBuffer;

	private List<Material> materialBuffer = new List<Material>();

	private Material GetMaterialFromID(int id)
	{
		switch(id)
		{
			case 0:
				return outlineMaterial1;
			case 1:
				return outlineMaterial2;
			default:
				return outlineMaterial3;
		}
	}

	private Material CreateMaterial(Color emissionColor)
	{
		Material mat = new Material(outlineBufferShader);
		mat.SetColor("_Color", emissionColor);
		mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		mat.SetInt("_ZWrite", 0);

		mat.DisableKeyword("_ALPHATEST_ON");
		mat.EnableKeyword("_ALPHABLEND_ON");
		mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
		mat.renderQueue = 3000;
		return mat;
	}

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		
	}
}
