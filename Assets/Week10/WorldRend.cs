using UnityEngine;
using Vector3 = System.Numerics.Vector3;
using UnityEngine.SceneManagement;

public class WorldRend : MonoBehaviour
{
    public Material material;

    public PlayerCube player;

    public Vector3 spherePos = new Vector3(5,1,2);

    void OnPostRender()
    {
        if (material == null) return;

        material.SetPass(0);

        GL.PushMatrix();
        GL.Begin(GL.LINES);

        DrawPlatform();
        DrawCube(player.position);
        DrawSphere(new Vector2(5,1),2,1.5f);

        GL.End();
        GL.PopMatrix();
    }

    void DrawPlatform()
    {
        float length = 20;
        float width = 2;

        Vector3 bl = new Vector3(-length,0,-width);
        Vector3 br = new Vector3(length,0,-width);
        Vector3 tr = new Vector3(length,0,width);
        Vector3 tl = new Vector3(-length,0,width);

        DrawEdge(bl, br);
        DrawEdge(br, tr);
        DrawEdge(tr, tl);
        DrawEdge(tl, bl);
    }

    void DrawCube(Vector3 center)
    {
        float s = 0.5f;

        Vector3[] v = new Vector3[]
        {
            new Vector3(-s,-s,-s),
            new Vector3(s,-s,-s),
            new Vector3(s,s,-s),
            new Vector3(-s,s,-s),

            new Vector3(-s,-s,s),
            new Vector3(s,-s,s),
            new Vector3(s,s,s),
            new Vector3(-s,s,s)
        };

        for(int i=0;i<v.Length;i++)
            v[i]+=center;

        DrawEdge(v[0],v[1]);
        DrawEdge(v[1],v[2]);
        DrawEdge(v[2],v[3]);
        DrawEdge(v[3],v[0]);

        DrawEdge(v[4],v[5]);
        DrawEdge(v[5],v[6]);
        DrawEdge(v[6],v[7]);
        DrawEdge(v[7],v[4]);

        DrawEdge(v[0],v[4]);
        DrawEdge(v[1],v[5]);
        DrawEdge(v[2],v[6]);
        DrawEdge(v[3],v[7]);
    }

    void DrawSphere(Vector2 center, float z, float radius)
    {
        int rings = 10;
        int segments = 20;

        // Horizontal rings (latitude)
        for(int r = -rings; r <= rings; r++)
        {
            float y = radius * r / rings;
            float ringRadius = Mathf.Sqrt(radius * radius - y * y);

            DrawRing(center, z, ringRadius, y, segments);
        }

        // Vertical rings (longitude)
        for(int m = 0; m < rings; m++)
        {
            float angle = (Mathf.PI * 2 / rings) * m;

            for(int i = 0; i < segments; i++)
            {
                float a0 = Mathf.PI * 2 * i / segments;
                float a1 = Mathf.PI * 2 * (i + 1) / segments;

                float x0 = Mathf.Cos(angle) * Mathf.Cos(a0) * radius;
                float y0 = Mathf.Sin(a0) * radius;
                float z0 = Mathf.Sin(angle) * Mathf.Cos(a0) * radius;

                float x1 = Mathf.Cos(angle) * Mathf.Cos(a1) * radius;
                float y1 = Mathf.Sin(a1) * radius;
                float z1 = Mathf.Sin(angle) * Mathf.Cos(a1) * radius;

                Vector3 p0 = new Vector3(x0, y0, z0);
                Vector3 p1 = new Vector3(x1, y1, z1);

                DrawEdge(center, z, p0, p1);
            }
        }
    }

    void DrawRing(Vector2 center, float baseZ, float ringRadius, float y, int segments)
    {
        float step = Mathf.PI * 2 / segments;

        for(int i = 0; i < segments; i++)
        {
            float a0 = step * i;
            float a1 = step * (i + 1);

            float x0 = Mathf.Cos(a0) * ringRadius;
            float z0 = Mathf.Sin(a0) * ringRadius;

            float x1 = Mathf.Cos(a1) * ringRadius;
            float z1 = Mathf.Sin(a1) * ringRadius;

            Vector3 p0 = new Vector3(x0, y, z0);
            Vector3 p1 = new Vector3(x1, y, z1);

            DrawEdge(center, baseZ, p0, p1);
        }
    }

    void DrawEdge(Vector3 a, Vector3 b)
    {
        float p0 = PerspectiveCamera.Instance.GetPerspective(a.Z);
        float p1 = PerspectiveCamera.Instance.GetPerspective(b.Z);

        Vector2 v0 = new Vector2(a.X, a.Y);
        Vector2 v1 = new Vector2(b.X, b.Y);

        GL.Vertex(v0 * p0);
        GL.Vertex(v1 * p1);
    }
    void DrawEdge(Vector2 center, float baseZ, Vector3 a, Vector3 b)
    {
        float p0 = PerspectiveCamera.Instance.GetPerspective(baseZ + a.Z);
        float p1 = PerspectiveCamera.Instance.GetPerspective(baseZ + b.Z);

        Vector2 v0 = new Vector2(a.X, a.Y) + center;
        Vector2 v1 = new Vector2(b.X, b.Y) + center;

        GL.Vertex(v0 * p0);
        GL.Vertex(v1 * p1);
    }
}
