using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace group_2_assignment5;

public class Frog
{
    
    private static Random rand = new Random();

    public Model Model;                    
    public Vector3 StartPosition, EndPosition, CurrentPosition;
    public float JumpDuration, JumpHeight, ElapsedTime;
    public float Scale = 1f;
    public float Rotation;
    public Vector3 SquashStretch = Vector3.One;


    public Frog(Model model, Vector3 startPos, Vector3 endPos, float jumpDuration, float jumpHeight)
    {
        
        Model = model;
        StartPosition = startPos;
        EndPosition = endPos;
        JumpDuration = jumpDuration;
        JumpHeight = jumpHeight;
        CurrentPosition = startPos; ElapsedTime = 0f;
    }
    
    private Vector3 RandomDestination()
    {
        // Define bounds
        float x = (float)(rand.NextDouble() * 10f - 5f);
        float z = (float)(rand.NextDouble() * 10f - 5f);
        return new Vector3(x, 0f, z);
    }
    
    public void Update(GameTime gameTime)
    {
        ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        float t = ElapsedTime / JumpDuration;

        // Horizontal Lerp
        CurrentPosition = Vector3.Lerp(StartPosition, EndPosition, t);

        // Vertical arc
        CurrentPosition.Y += (float)Math.Sin(t * Math.PI) * JumpHeight;

        // Squash/stretch
        float squashFactor = 1 - 0.2f * (float)Math.Cos(t * Math.PI); // 0.8 -> 1.2
        SquashStretch = new Vector3(squashFactor, 1.2f - 0.2f * squashFactor, squashFactor);

        // Slight mid-air rotation
        Rotation = 0.2f * (float)Math.Sin(t * Math.PI);

        // Loop the jump
        if (t >= 1f)
        {
            ElapsedTime = 0f;
            CurrentPosition = StartPosition; // reset to start
        }
    }

    // Draw method
    public void Draw(Matrix view, Matrix projection)
    {
        if (Model == null) return;

        Matrix world = Matrix.CreateScale(SquashStretch) *
                       Matrix.CreateScale(Scale) *
                       Matrix.CreateRotationY(Rotation) *
                       Matrix.CreateTranslation(CurrentPosition);

        foreach (ModelMesh mesh in Model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.World = world;
                effect.View = view;
                effect.Projection = projection;
                effect.EnableDefaultLighting();
            }
            mesh.Draw();
        }
    }
}