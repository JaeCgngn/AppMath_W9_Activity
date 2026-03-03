using UnityEngine;

public class Pyramid : MonoBehaviour
{
    public Material material;

    public Vector2 cubePos;

    public float zPos2;
    public float zPos;
    public float zRot = 0;


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


        


        GL.End();
        GL.PopMatrix();
    }
}
