using UnityEngine;
using Vector3 = System.Numerics.Vector3;
using UnityEngine.SceneManagement;

public class WorldRend : MonoBehaviour
{
    public Material material;

    public PlayerCube player;

    void OnPostRender()
    {
        if (material == null) return;

        material.SetPass(0);

        GL.PushMatrix();
        GL.Begin(GL.LINES);

        DrawPlatform();
        DrawCube(player.position);
        DrawSphere(new Vector3(5,1,0));

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

    void DrawSphere(Vector3 center)
    {
        int segments = 20;
        float radius = 1;

        for(int i=0;i<segments;i++)
        {
            float a0 = Mathf.PI*2*i/segments;
            float a1 = Mathf.PI*2*(i+1)/segments;

            Vector3 p0 = new Vector3(Mathf.Cos(a0)*radius,0,Mathf.Sin(a0)*radius) + center;
            Vector3 p1 = new Vector3(Mathf.Cos(a1)*radius,0,Mathf.Sin(a1)*radius) + center;

            DrawEdge(p0,p1);
        }
    }

    void DrawEdge(Vector3 a, Vector3 b)
    {
        GL.Vertex3(a.X,a.Y,a.Z);
        GL.Vertex3(b.X,b.Y,b.Z);
    }
}
