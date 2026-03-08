using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_2_assignment5;

public class Bird
{
    // Fields
    private Model _model;
    private Vector3 _position;
    private float _scale;
    private float _flySpeed;
    private float _wingRotation;
    private float _verticalOffset;
    private float _bankAngle;
    private float _elaspedTime;
    private Vector3 _startPos;
    private Vector3 _endPos;
    private float _lerpBird;
    private float _wingFreq;
    private Color _color;
    
    public Bird(Model _model, Vector3 startPosition, float _scale, 
        float _flySpeed, float  _verticalOffset, float _wingFreq, Color _color)
    {
        this._model = _model;
        this._scale = _scale;
        this._flySpeed = _flySpeed;
        this._wingFreq = _wingFreq;
        this._verticalOffset = _verticalOffset;
        _startPos = startPosition;
        _endPos = new Vector3(startPosition.X + 100f, startPosition.Y, startPosition.Z);
        this._color = _color;
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _elaspedTime += dt;
        
        float oneCycleDis = Vector3.Distance(_startPos, _endPos);
        float oneCycleTime = oneCycleDis / _flySpeed;
        _lerpBird = _elaspedTime / oneCycleTime;
        
        // lerp position.X between two points
        _position = Vector3.Lerp(_startPos, _endPos, _lerpBird);

        // Reset elapsed time when reached the endpoint
        if (_lerpBird >= 1f)
        {
            _elaspedTime = 0f;
        }
        
        // wing rotation(flap)
        _wingRotation = (float)Math.Sin(_elaspedTime * _wingFreq) * 0.1f;
        
        // vertical offset(bobbing)
        _position.Y += (float)Math.Sin(_elaspedTime * 4f) * _verticalOffset;
        
        // bank angle(tilt)
        _bankAngle = (float)Math.Cos(_elaspedTime * 4f) * 0.1f;
    }

    public void Draw(Matrix view, Matrix projection)
    {
        Matrix[] transforms = new Matrix[_model.Bones.Count];
        _model.CopyAbsoluteBoneTransformsTo(transforms);
        // Root transform
        Matrix rootWorld = Matrix.CreateScale(_scale)
            * Matrix.CreateRotationX(MathHelper.ToRadians(0))
            * Matrix.CreateRotationY(MathHelper.ToRadians(50))
            * Matrix.CreateRotationZ(_bankAngle)
            * Matrix.CreateTranslation(_position);

        foreach (ModelMesh mesh in _model.Meshes)
        {
            Matrix childTransform = Matrix.Identity;
            
            string name = mesh.Name.ToLower();

            if (name.Contains("wing"))
            { 
                childTransform = Matrix.CreateRotationX(_wingRotation);
            }

            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();
                effect.DiffuseColor = _color.ToVector3();
                effect.View = view;
                effect.Projection = projection;
                effect.World = childTransform * transforms[mesh.ParentBone.Index] * rootWorld;
            }
            mesh.Draw();
        }
    }
}

