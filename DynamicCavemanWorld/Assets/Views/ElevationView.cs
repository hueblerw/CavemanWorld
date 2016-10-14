using UnityEngine;
using System.Collections;

public class ElevationView : MonoBehaviour {

    public static Mesh BuildMesh(SingleValueLayer elevationLayer, MeshFilter meshFilter, MeshCollider meshCollider, MeshRenderer meshRenderer)
    {
        // Convert to mesh data
        Vector3[] vertices = new Vector3[4];
        int[] triangles = new int[2 * 3];
        Vector3[] normals = new Vector3[4];

        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(1, 0, 0);
        vertices[2] = new Vector3(0, 0, -1);
        vertices[3] = new Vector3(1, 0, -1);

        triangles[0] = 0;
        triangles[1] = 3;
        triangles[2] = 2;
        triangles[3] = 0;
        triangles[4] = 1;
        triangles[5] = 3;

        normals[0] = Vector3.up;
        normals[1] = Vector3.up;
        normals[2] = Vector3.up;
        normals[3] = Vector3.up;

        // Create a new mesh and populate it with the data from the elevation layer
        Mesh world = new Mesh();
        world.triangles = triangles;
        world.vertices = vertices;
        world.normals = normals;

        // Return our mesh to the controller
        return world;
    }

}
