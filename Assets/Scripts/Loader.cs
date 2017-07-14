// loads the raw binary data into a texture saved as a Unity asset 
// (so can be de-activated after a given data cube has been converted)
// adapted from a XNA project by Kyle Hayward 
// http://graphicsrunner.blogspot.ca/2009/01/volume-rendering-101.html
// Gilles Ferrand, University of Manitoba 2016

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.IO; // to get BinaryReader
using System.Linq; // to get array's Min/Max

public class Loader : MonoBehaviour {

	[Header("Folder with images to create 3DTexture")]
	[SerializeField]
	public string folder;
	[Header("Define Opacity of 3DTexture")]
	[SerializeField][Range(0, 1)]
	public float opacity = 1;
	[SerializeField]
	[Header("Number of Ray Marching Iterations")]
	public int iterations;
	[Header("Alpha Value -> Transparent")]
	[SerializeField][Range(0, 1)]
	public float alpha = 0.1f;
	[Header("Slice along axis 1: min")]
	[SerializeField][Range(0, 1)]
	public float _SliceAxis1Min = 0.0f;
	[Header("Slice along axis 1: max")]
	[SerializeField][Range(0, 1)]
	public float _SliceAxis1Max = 1.0f;
	[Header("Slice along axis 2: min")]
	[SerializeField][Range(0, 1)]
	public float _SliceAxis2Min = 0.0f;
	[Header("Slice along axis 2: max")]
	[SerializeField][Range(0, 1)]
	public float _SliceAxis2Max = 1.0f;
	[Header("Slice along axis 3: min")]
	[SerializeField][Range(0, 1)]
	public float _SliceAxis3Min = 0.0f;
	[Header("Slice along axis 3: max")]
	[SerializeField][Range(0, 1)]
	public float _SliceAxis3Max = 1.0f;
	[SerializeField][Range(0, 1)]
	public float _DataMin = 0.0f;
	[SerializeField][Range(0, 1)]
	public float _DataMax = 1.0f;
	[SerializeField]
	public float _Normalization = 2.0f;

	[Header("For Raw Files Only")]
	public string path = @"Assets/";
	public string filename = "skull";
	public string extension = ".raw";
	public int[] size = new int[3] {512, 512, 512};
	public bool mipmap;

	private Texture2D[] slices;

	void Start() {
		Camera.main.depthTextureMode = DepthTextureMode.Depth;
		slices = Resources.LoadAll<Texture2D> (folder) as Texture2D[];

		Color[] colors;

		if (slices.Length != 0 && !folder.Equals(string.Empty))
			// load the raw data from MRI images specified in the folder field
			colors = GenerateVolumeTexture();
		else
			// Load the raw data from the specified raw file
			colors = LoadRAWFile();
			
		// create the texture
		Texture3D texture = new Texture3D (size [0], size [1], size [2], TextureFormat.RGBA32, false);
		texture.SetPixels (colors);
		texture.Apply ();
		// assign it to the material of the parent object
		GetComponent<Renderer> ().material.SetTexture ("_Data", texture);
		// save it as an asset for re-use
		#if UNITY_EDITOR
		AssetDatabase.CreateAsset (texture, path + filename + "1.asset");
		#endif

	}

	private void Update(){
		GetComponent<Renderer> ().material.SetFloat ("_Opacity", opacity);
		GetComponent<Renderer> ().material.SetInt ("_Iterations", iterations);
		GetComponent<Renderer> ().material.SetFloat ("_Alpha", alpha);
		GetComponent<Renderer> ().material.SetFloat ("_SliceAxis1Min", _SliceAxis1Min);
		GetComponent<Renderer> ().material.SetFloat ("_SliceAxis1Max", _SliceAxis1Max);
		GetComponent<Renderer> ().material.SetFloat ("_SliceAxis2Min", _SliceAxis2Min);
		GetComponent<Renderer> ().material.SetFloat ("_SliceAxis2Max", _SliceAxis2Max);
		GetComponent<Renderer> ().material.SetFloat ("_SliceAxis3Min", _SliceAxis3Min);
		GetComponent<Renderer> ().material.SetFloat ("_SliceAxis3Max", _SliceAxis3Max);
		GetComponent<Renderer> ().material.SetFloat ("_DataMin", _DataMin);
		GetComponent<Renderer> ().material.SetFloat ("_DataMax", _DataMax);
		GetComponent<Renderer> ().material.SetFloat ("_Normalization", _Normalization);
	}

	private Color[] LoadRAWFile()
	{
		Color[] colors;

		Debug.Log ("Opening file "+path+filename+extension);
		FileStream file = new FileStream(path+filename+extension, FileMode.Open);
		Debug.Log ("File length = "+file.Length+" bytes, Data size = "+size[0]*size[1]*size[2]+" points -> "+file.Length/(size[0]*size[1]*size[2])+" byte(s) per point");

		BinaryReader reader = new BinaryReader(file);
		byte[] buffer = new byte[size[0] * size[1] * size[2]]; // assumes 8-bit data
		reader.Read(buffer, 0, sizeof(byte) * buffer.Length);
		reader.Close();

		colors = new Color[buffer.Length];
		Color color = Color.black;
		for (int i = 0; i < buffer.Length; i++)
		{
			color.a = (float)buffer[i] / byte.MaxValue; //scale the scalar values to [0, 1]
			colors [i] = color;
		}

		return colors;
	}

	private Color[] GenerateVolumeTexture()
	{
		var w = size[0];
		var h = size[1];
		var d = size[2];

		// skip some slices if we can't fit it all in
		var countOffset = (slices.Length - 1) / (float)d;

		Color[] volumeColors = new Color[w * h * d];

		var sliceCount = 0;
		var sliceCountFloat = 0f;
		for(int z = 0; z < d; z++)
		{
			sliceCountFloat += countOffset;
			sliceCount = Mathf.FloorToInt(sliceCountFloat);
			for(int x = 0; x < w; x++)
			{
				for(int y = 0; y < h; y++)
				{
					var idx = x + (y * w) + (z * (w * h));

					Color c = slices[sliceCount].GetPixelBilinear(x / (float)w, y / (float)h); 

					if (!(c.r < 0.1f && c.g < 0.1f && c.b < 0.1f)){
						c.a = 0.2f;
						volumeColors[idx] = c;
					}
				}
			}
		}

		return volumeColors;
	}

}
