using Unity.VisualScripting;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Capsule : MonoBehaviour
{

    public Material material;

    public Vector2 capsulePos;

    public float zPos;
    //public float zRot = 0;

    public float radius = 0.5f;
    public float height = 3f;
    public int segments = 24;
    public int horizontalRings = 3;
    public int verticalRings = 4; 


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

        DrawCapsule(capsulePos, zPos);
        


        GL.End();
        GL.PopMatrix();
    }

    public void DrawCapsule(Vector2 center, float baseZ)
    {
        float halfBody = Mathf.Max(0, (height * 0.5f) - radius);

        DrawEdge(center, baseZ,
            new Vector3(-radius, -halfBody, 0),
            new Vector3(-radius,  halfBody, 0));

        DrawEdge(center, baseZ,
            new Vector3( radius, -halfBody, 0),
            new Vector3( radius,  halfBody, 0));

        DrawHorizontalRing(center, baseZ, radius, halfBody);
        DrawHorizontalRing(center, baseZ, radius, -halfBody);

        DrawHemisphere(center, baseZ, halfBody, true);
        DrawHemisphere(center, baseZ, -halfBody, false);

        DrawCapsuleVertical(center, baseZ, halfBody);
    }

    void DrawHorizontalRing(Vector2 center, float zPos, float baseRadius, float yOffset)
    {
        float step = Mathf.PI * 2f / segments;

        for (int i = 0; i < segments; i++)
        {
            float a0 = step * i;
            float a1 = step * (i + 1);

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


    void DrawHemisphere(Vector2 center, float baseZ, float yCenter, bool top)
    {
        for (int r = 1; r <= horizontalRings; r++)
        {
            float t = (float)r / horizontalRings;
            float angle = t * (Mathf.PI * 0.5f);

            float yOffset = Mathf.Sin(angle) * radius;
            float ringRadius = Mathf.Cos(angle) * radius;

            if (!top) yOffset = -yOffset;

            DrawHorizontalRing(center, baseZ, ringRadius, yCenter + yOffset);
        }
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

    void DrawCapsuleVertical(Vector2 center, float baseZ, float halfBody)
    {
        
        float step = Mathf.PI * 2f / verticalRings;

        for (int m = 0; m < verticalRings; m++)
        {
            float angle = step * m;

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            // vertical body line
            DrawEdge(center, baseZ,
                new Vector3(x, -halfBody, z),
                new Vector3(x,  halfBody, z));

            // connect into hemispheres
            DrawHemisphereVertical(center, baseZ, halfBody, angle, true);
            DrawHemisphereVertical(center, baseZ, -halfBody, angle, false);
        }
    }

    void DrawHemisphereVertical(Vector2 center, float baseZ, float yCenter, float angle, bool top)
    {
        int steps = verticalRings;
        float prevX = 0;
        float prevY = 0;
        float prevZ = 0;
        bool first = true;

        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps;
            float a = t * (Mathf.PI * 0.5f);

            float y = Mathf.Sin(a) * radius;
            float ringR = Mathf.Cos(a) * radius;

            if (!top) y = -y;

            float x = Mathf.Cos(angle) * ringR;
            float z = Mathf.Sin(angle) * ringR;

            if (!first)
            {
                DrawEdge(center, baseZ,
                    new Vector3(prevX, yCenter + prevY, prevZ),
                    new Vector3(x, yCenter + y, z));
            }

            prevX = x;
            prevY = y;
            prevZ = z;
            first = false;
        }
    }
}
