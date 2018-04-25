using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BezierCollider2D))] 
public class MeshGenerator : MonoBehaviour {

	public Material mat;
    EdgeCollider2D edgeCollider;
	public float border = -20;
	public Mesh mesh;
	void Start()
	{
        edgeCollider = GetComponent<EdgeCollider2D>();
		mesh = new Mesh();
		Vector2[] edgePoints = edgeCollider.points;
		Vector3[] slopeMesh = new Vector3[edgePoints.Length*2];
		Vector2[] orientation = new Vector2[edgePoints.Length*2];

		for(int i = 0; i < edgePoints.Length*2; i++)
		{
			slopeMesh[i] = edgePoints[i/2];
			orientation[i] = new Vector2(1f, 1f);
			i++;
			slopeMesh[i] = new Vector3(slopeMesh[(i-1)].x,slopeMesh[(i-1)].y +border,slopeMesh[(i-1)].z);
			orientation[i] = new Vector2(0f, 0f);
		}

		mesh.vertices = slopeMesh;
		mesh.uv = orientation;
		List<int> triangles = new List<int>();
		for(int i=0;i<slopeMesh.Length-3;i++)
		{
			triangles.Add(i);
			triangles.Add(i+2);
			triangles.Add(i+1);

			i++;
			triangles.Add(i);
			triangles.Add(i+1);
			triangles.Add(i+2);
		}

		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();
		MeshRenderer mr = GetComponent<MeshRenderer>();
		mr.material = mat;
		mr.sortingLayerName = "Ground";
		GetComponent<MeshFilter>().mesh = mesh;
	}
}
