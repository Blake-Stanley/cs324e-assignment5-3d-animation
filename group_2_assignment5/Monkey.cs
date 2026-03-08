using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_2_assignment5;

public class Monkey
{
    // Fields
    private Model _model;
    private Vector3 _position;
    private Texture2D _texture;
    private float _scale;
    private float _swingSpeed;
    private float _swingDirection;  // +1 = swing right, -1 = swing left
    private float _bodyLean;   // side-to-side lean while walking
    private float _bodyBob;    // forward-back sway
    private float _elapsedTime;
    private float _lerpMonkey;  // normalized swing progress for lerp
    private float _baseY;

    private const float left_bound = -4f;
    private const float right_bound = 4f;

    public Monkey(Model _model, Texture2D texture, Vector3 startPosition, float _scale = 0.01f, float swingSpeed = 1.5f)
    {
        this._texture = texture;
        this._model = _model;
        this._position = startPosition;
        this._scale = _scale;
        this._swingSpeed = swingSpeed;
        this._swingDirection = 1f;
        this._lerpMonkey = 0.0f;
        this._elapsedTime = 0.0f;
        this._baseY = startPosition.Y;
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        this._elapsedTime += dt;

        // Progress lerpMonkey based on direction and speed
        _lerpMonkey += _swingDirection * _swingSpeed * dt / (right_bound - left_bound);
        _lerpMonkey = MathHelper.Clamp(_lerpMonkey, 0f, 1f);

        // Root: lerp position.X smoothly between boundaries
        _position.X = MathHelper.Lerp(left_bound, right_bound, _lerpMonkey);

        // Flips the direction at boundaries
        if (_lerpMonkey >= 1f) _swingDirection = -1f;
        if (_lerpMonkey <= 0f) _swingDirection = 1f;

        // Body leans in the direction of travel
        _bodyLean = _swingDirection * (float)System.Math.Sin(_elapsedTime * 5f) * 0.15f;

        // Forward-back body bob as monkey walks
        _bodyBob = (float)System.Math.Sin(_elapsedTime * 10f) * 0.08f;

        // Slight bounce as monkey moves
        _position.Y = _baseY + (float)System.Math.Abs(System.Math.Sin(_elapsedTime * 5f)) * 0.15f;
    }

    public void Draw(Matrix view, Matrix projection)
    {
        // Facing angle, keeps monkey facing forward
        float facingAngle = _swingDirection > 0 ? MathHelper.ToRadians(30f) : MathHelper.ToRadians(10f);

        // Root transform
        Matrix rootWorld = Matrix.CreateScale(_scale)
                           * Matrix.CreateRotationX(MathHelper.ToRadians(75f))
                           * Matrix.CreateRotationY(MathHelper.ToRadians(0f))
                           * Matrix.CreateRotationZ(MathHelper.ToRadians(170f))
                           * Matrix.CreateRotationY(facingAngle)
                           * Matrix.CreateTranslation(_position);
        foreach (ModelMesh mesh in _model.Meshes)
        {
            // Determines the per-mesh child transform based on name
            Matrix childTransform = Matrix.Identity;

            // Single mesh: apply body lean (Z) and forward-back bob (X) as secondary transform
            childTransform = Matrix.CreateRotationZ(_bodyLean)
                           * Matrix.CreateRotationX(_bodyBob);

            // Apply child transform first then root world transform
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.World = childTransform * rootWorld;
                effect.View = view;
                effect.Projection = projection;
                effect.EnableDefaultLighting();
                effect.TextureEnabled = true;
                effect.Texture = _texture;
            }

            mesh.Draw();
        }
    }
}
