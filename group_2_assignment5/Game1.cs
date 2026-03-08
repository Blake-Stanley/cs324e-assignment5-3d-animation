using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_2_assignment5;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private Matrix view, projection;
    private BasicEffect _groundEffect; // Ground effect
    private VertexPositionColor[] _groundVertices; // Ground vertices
    // private Model _tigerModel;
    // private Tiger _tiger1;
    // private Tiger _tiger2;

    private Frog myFrog;
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        view = Matrix.CreateLookAt(
            new Vector3(0, 3, 10),
            Vector3.Zero,
            Vector3.Up);

        projection = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,
            GraphicsDevice.Viewport.AspectRatio,
            0.1f,
            100f
        );
        
        // Ground effect
        _groundEffect = new BasicEffect(GraphicsDevice);
        _groundEffect.VertexColorEnabled = true;
        _groundVertices = new VertexPositionColor[]
        {
            new VertexPositionColor(new Vector3(-20, -1f, -20), new Color(34, 100, 20)),
            new VertexPositionColor(new Vector3( 20, -1f, -20), new Color(34, 100, 20)),
            new VertexPositionColor(new Vector3(-20, -1f,  20), new Color(34, 100, 20)),
            new VertexPositionColor(new Vector3( 20, -1f,  20), new Color(34, 100, 20)),
        };

        base.Initialize();
    }

    protected override void LoadContent()
    {
        //_tigerModel =  Content.Load<Model>("models/Tiger");
        // _tiger1 = new Tiger(_tigerModel, new Vector3(-5f, 0f, 0f), _scale: 0.01f, walkSpeed: 2f);
        //_tiger2 = new Tiger(_tigerModel, new Vector3(0f, 0f, 2f), _scale: 0.015f, walkSpeed: 1.3f);
        
        Model frogModel = Content.Load<Model>("models/frog");
        myFrog = new Frog(frogModel, new Vector3(0, 0, 0), new Vector3(5, 0, 0), 2f, 2f);
        myFrog.Scale = 0.01f;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

       // _tiger1.Update(gameTime);
       // _tiger2.Update(gameTime);
       
       myFrog.Update(gameTime);
       
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(34, 85, 34)); // jungle green background
        _groundEffect.View = view;
        _groundEffect.Projection = projection;
        _groundEffect.World = Matrix.Identity;
        foreach (EffectPass pass in _groundEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            GraphicsDevice.DrawUserPrimitives(
                PrimitiveType.TriangleStrip,
                _groundVertices, 0, 2
            );
        }
        // Draw tigers
         //_tiger1.Draw(view, projection);
         //_tiger2.Draw(view, projection);
         
         myFrog.Draw(view, projection);

        base.Draw(gameTime);
    }
}