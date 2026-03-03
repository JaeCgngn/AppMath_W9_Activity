using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class LineGen : MonoBehaviour
{
    public Material material;
    public float cubeSize;
    public Vector2 cubePos;
    
    public float zPos;
    public float zRot = 0;

    public float sphereRadius = 1f;
    public int sphereSegments = 10;

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

        DrawSphere(cubePos, zPos, sphereRadius);


        GL.End();
        GL.PopMatrix();
    }

 

    public void DrawSphere(Vector2 center, float zPos, float radius)
    {
        float frontZ = PerspectiveCamera.Instance.GetPerspective(zPos);

        DrawCircle(center, frontZ, radius);
        DrawCircleVertical(center, frontZ, radius);
        DrawCircleDepth(center, zPos, radius);
        DrawSphereLongitudes(center, zPos, radius);
    }

    public void DrawCircle(Vector2 center, float perspective, float radius)
    {
        float step = Mathf.PI * 2f / sphereSegments;

        for (int i = 0; i < sphereSegments; i++)
        {
            float a0 = step * i;
            float a1 = step * (i + 1);

            Vector2 p0 = new Vector2(Mathf.Cos(a0), Mathf.Sin(a0)) * radius + center;
            Vector2 p1 = new Vector2(Mathf.Cos(a1), Mathf.Sin(a1)) * radius + center;

            GL.Vertex(p0 * perspective);
            GL.Vertex(p1 * perspective);

        }

    }

    public void DrawCircleVertical(Vector2 center, float perspective, float radius)
    {
        float step = Mathf.PI * 2f / sphereSegments;

    for (int i = 0; i < sphereSegments; i++)
    {
        float a0 = step * i;
        float a1 = step * (i + 1);

        Vector2 p0 = new Vector2(Mathf.Cos(a0) * radius, Mathf.Sin(a0) * radius) + center;
        Vector2 p1 = new Vector2(Mathf.Cos(a1) * radius, Mathf.Sin(a1) * radius) + center;

        GL.Vertex(p0 * perspective);
        GL.Vertex(p1 * perspective);
    }
    }

    public void DrawCircleDepth(Vector2 center, float zPos, float radius)
    {
        // int rings = 15;

        // for (int r = 1; r <= sphereSegments; r++)
        // {
        //     float t = (float)r / (rings + 1);
        //     float zOffset = Mathf.Lerp(-radius, radius, t);
        //     float ringRadius = Mathf.Sqrt(radius * radius - zOffset * zOffset);

        //     float perspective = PerspectiveCamera.Instance.GetPerspective(zPos + zOffset);
        //     DrawCircle(center, perspective, ringRadius);

        // }

        int rings = 10;

        for (int r = 1; r <= rings; r++)
        {
            float t = (float)r / (rings + 1);

            // vertical position on sphere
            float yOffset = Mathf.Lerp(-radius, radius, t);

            // correct shrinking toward poles
            float ringRadius = Mathf.Sqrt(radius * radius - yOffset * yOffset);

            DrawHorizontalRing(center, zPos, ringRadius, yOffset);
        }
    }

    public void DrawSphereLongitudes(Vector2 center, float zPos, float radius)
    {
        int meridians = 10; 

        for (int m = 0; m < meridians; m++)
        {
            float angle = (Mathf.PI * 2f / meridians) * m;

            float step = Mathf.PI * 2f / sphereSegments;

            for (int i = 0; i < sphereSegments; i++)
            {
                float a0 = step * i;
                float a1 = step * (i + 1);

                float x0 = Mathf.Cos(angle) * Mathf.Cos(a0) * radius;
                float y0 = Mathf.Sin(a0) * radius;
                float z0 = Mathf.Sin(angle) * Mathf.Cos(a0) * radius;

                float x1 = Mathf.Cos(angle) * Mathf.Cos(a1) * radius;
                float y1 = Mathf.Sin(a1) * radius;
                float z1 = Mathf.Sin(angle) * Mathf.Cos(a1) * radius;

                float p0 = PerspectiveCamera.Instance.GetPerspective(zPos + z0);
                float p1 = PerspectiveCamera.Instance.GetPerspective(zPos + z1);

                GL.Vertex((new Vector2(x0, y0) + center) * p0);
                GL.Vertex((new Vector2(x1, y1) + center) * p1);
            }
        }
    }

    public void DrawHorizontalRing(Vector2 center, float zPos, float baseRadius, float yOffset)
    {
        float step = Mathf.PI * 2f / sphereSegments;

        for (int i = 0; i < sphereSegments; i++)
        {
            float a0 = step * i;
            float a1 = step * (i + 1);

            // true sphere math
            float x0 = Mathf.Cos(a0) * baseRadius;
            float z0 = Mathf.Sin(a0) * baseRadius;

            float x1 = Mathf.Cos(a1) * baseRadius;
            float z1 = Mathf.Sin(a1) * baseRadius;

            float p0 = PerspectiveCamera.Instance.GetPerspective(zPos + z0);
            float p1 = PerspectiveCamera.Instance.GetPerspective(zPos + z1);

            Vector2 v0 = new Vector2(x0, yOffset) + center;
            Vector2 v1 = new Vector2(x1, yOffset) + center;

            GL.Vertex(v0 * p0);
            GL.Vertex(v1 * p1);
        }
    }
}

