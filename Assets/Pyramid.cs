using Unity.VisualScripting;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Pyramid : MonoBehaviour
{
    public Material material;

    public Vector2 pyramidPos;
    public Vector2 pyramidPos2;

    public float zPos;
    public float zPos2;
    public float yRot = 0f;

    public float baseSize = 2f;
    public float height = 2.5f;

    void Update()
    {
        yRot += Time.deltaTime;
    }

    private void OnPostRender()
    {
        DrawLine();
    }

    public void DrawLine()
    {
        if (material == null)
        {
            Debug.LogError("You need to add a material");
            return;
        }
        GL.PushMatrix();

        GL.Begin(GL.LINES);
        material.SetPass(0);

        // DrawPyramid(pyramidPos, zPos, baseSize, height);
        DrawPyramid2(pyramidPos2, zPos2, baseSize, height);
        


        GL.End();
        GL.PopMatrix();
    }

    // public void DrawPyramid(Vector2 center, float baseZ, float size, float height)
    // {
    //     float half = size * 0.5f;

    //     Vector3 bl = new Vector3(-half, -half, 0);
    //     Vector3 br = new Vector3( half, -half, 0);
    //     Vector3 tr = new Vector3( half,  half, 0);
    //     Vector3 tl = new Vector3(-half,  half, 0);

    //     Vector3 apex = new Vector3(0, 0, height);

    //     //base 
    //     DrawEdge(center, baseZ, bl, br);
    //     DrawEdge(center, baseZ, br, tr);
    //     DrawEdge(center, baseZ, tr, tl);
    //     DrawEdge(center, baseZ, tl, bl);

    //     //sides
    //     DrawEdge(center, baseZ, bl, apex);
    //     DrawEdge(center, baseZ, br, apex);
    //     DrawEdge(center, baseZ, tr, apex);
    //     DrawEdge(center, baseZ, tl, apex);
    // }

    // public void DrawEdge(Vector2 center, float baseZ, Vector3 a, Vector3 b)
    // {
    //     Vector3 ra = RotateY(a);
    //     Vector3 rb = RotateY(b);

    //     float p0 = PerspectiveCamera.Instance.GetPerspective(baseZ + ra.Z);
    //     float p1 = PerspectiveCamera.Instance.GetPerspective(baseZ + rb.Z);

    //     Vector2 v0 = new Vector2(ra.X, ra.Y) + center;
    //     Vector2 v1 = new Vector2(rb.X, rb.Y) + center;

    //     GL.Vertex(v0 * p0);
    //     GL.Vertex(v1 * p1);
    // }

    public void DrawPyramid2(Vector2 center, float baseZ, float size, float height)
    {
        float half = size * 0.5f;

        Vector3 bl = new Vector3(-half, 0, -half);
        Vector3 br = new Vector3( half, 0, -half);
        Vector3 tr = new Vector3( half, 0,  half);
        Vector3 tl = new Vector3(-half, 0,  half);

        Vector3 apex = new Vector3(0, height, 0);

        DrawEdge2(center, baseZ, bl, br);
        DrawEdge2(center, baseZ, br, tr);
        DrawEdge2(center, baseZ, tr, tl);
        DrawEdge2(center, baseZ, tl, bl);

        DrawEdge2(center, baseZ, bl, apex);
        DrawEdge2(center, baseZ, br, apex);
        DrawEdge2(center, baseZ, tr, apex);
        DrawEdge2(center, baseZ, tl, apex);

    }

    void DrawEdge2(Vector2 center, float baseZ, Vector3 a, Vector3 b)
    {
        Vector3 ra = RotateY(a);
        Vector3 rb = RotateY(b);

        float p0 = PerspectiveCamera.Instance.GetPerspective(baseZ + ra.Z);
        float p1 = PerspectiveCamera.Instance.GetPerspective(baseZ + rb.Z);

        Vector2 v0 = new Vector2(ra.X, ra.Y) + center;
        Vector2 v1 = new Vector2(rb.X, rb.Y) + center;

        GL.Vertex(v0 * p0);
        GL.Vertex(v1 * p1);
    }

    Vector3 RotateY(Vector3 v)
    {
        float cos = Mathf.Cos(yRot);
        float sin = Mathf.Sin(yRot);

        float x = v.X * cos - v.Z * sin;
        float z = v.X * sin + v.Z * cos;

        return new Vector3(x, v.Y, z);
    }
}
