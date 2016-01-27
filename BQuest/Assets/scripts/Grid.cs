using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	public int tile = 16;
	public Vector2 grid = Vector2.one * 10f;
	public bool exibirGrade = true;

	public const float PPU = 100f; //- pixels per unit

	void OnDrawGizmos () {
		if (!exibirGrade)
			return;

		Gizmos.color = Color.grey;
		Vector3 p1, p2;
		for (int i=0; i<grid.x; i++) {
			p1 = p2 = transform.position;
			p1.x = p2.x = (float)(i*tile) / PPU;
			p2.y = -(float)(grid.y*tile) / PPU;
			Gizmos.DrawLine(p1, p2);
		}
		for (int j=0; j<grid.y; j++) {
			p1 = p2 = transform.position;
			p2.x = (float)(grid.x*tile) / PPU;
			p1.y = p2.y = -(float)(j*tile) / PPU;
			Gizmos.DrawLine(p1, p2);
		}
	}

}
