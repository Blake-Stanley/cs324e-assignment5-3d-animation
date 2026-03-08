using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_2_assignment5;

public class Tiger
{
    // Fields
    private Model _model;
    private Vector3 _position;
    private float _scale;
    private float _walkSpeed;
    private float _walkDirection;  // +1 = walk right, -1 = walk left
    private float _legRotation;
    private float _tailRotation;
    private float _elapsedTime;
    private float _lerpTiger;  // normalized walk progress for lerp

    private const float left_bound = -5f;
    private const float right_bound = 5f;

    public Tiger(Model _model, Vector3 startPosition, float _scale = 0.01f, float walkSpeed = 2f)
    {
        this._model = _model;
        this._position = startPosition;
        this._scale = _scale;
        this._walkSpeed = walkSpeed;
        this._walkDirection = 1f;
        this._lerpTiger = 0.0f;
        this._elapsedTime = 0.0f;
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        this._elapsedTime += dt;
        
        // Progress lerpTiger based on direction and speed
        _lerpTiger += _walkDirection * _walkSpeed * dt / (right_bound - left_bound);
        _lerpTiger = MathHelper.Clamp(_lerpTiger, 0f, 1f);
        
        // Root: lerp position.X smoothly between boundaries
        _position.X = MathHelper.Lerp(left_bound, right_bound, _lerpTiger);
        
        // Flips the direction at boundaries
        if (_lerpTiger >= 1f) _walkDirection = -1f;
        if (_lerpTiger <= 0f) _walkDirection = 1f;
        
        // Leg rotation, sine walk cycle
        _legRotation = (float)System.Math.Sin(_elapsedTime * 6f) * 0.4f;
        
        // tail sway, slower
        _tailRotation = (float)System.Math.Sin(_elapsedTime * 3f) * 0.3f;
        _position.Y = (float)System.Math.Abs(System.Math.Sin(_elapsedTime * 6f)) * 0.2f;
    }

    public void Draw(Matrix view, Matrix projection)
    {
        // Facing angle, keeps tiger facing forward rotating 180 when moving left
        float facingAngle = _walkDirection > 0 ? MathHelper.ToRadians(150f) : MathHelper.ToRadians(120f);
        
        // Root transform
        Matrix rootWorld = Matrix.CreateScale(_scale)
                           * Matrix.CreateRotationX(MathHelper.ToRadians(75f))
                           * Matrix.CreateRotationY(MathHelper.ToRadians(0f))
                           * Matrix.CreateRotationZ(MathHelper.ToRadians(130f))
                           * Matrix.CreateRotationY(facingAngle)
                           * Matrix.CreateTranslation(_position);

        foreach (ModelMesh mesh in _model.Meshes)
        {
            // Determines the per-mesh child tranform based on name
            Matrix childTransform = Matrix.Identity;

            string name = mesh.Name.ToLower();
            
            if (name.Contains("leg") || name.Contains("front") || name.Contains("back"))
            {
                // Legs rotate around local X axis
                childTransform = Matrix.CreateRotationX(_legRotation);
            }
            else if (name.Contains("tail"))
            {
                // Tail sways around its local Z axis
                childTransform = Matrix.CreateRotationZ(_tailRotation);
            }
            
            // Apply child transform first locally the root world transform
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.World = childTransform * rootWorld;
                effect.View = view;
                effect.Projection = projection;
                effect.EnableDefaultLighting();
                effect.TextureEnabled = false;
                effect.DiffuseColor = new Vector3(0.8f, 0.5f, 0.1f);
            }

            
            mesh.Draw();
        }
    }
}