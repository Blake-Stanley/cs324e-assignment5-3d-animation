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
    private Model _tigerModel;
    private Tiger _tiger1;
    private Tiger _tiger2;
    private Model _birdModel;
    private Bird _bird1;
    private Bird _bird2;
    private Model _monkeyModel;
    private Monkey _monkey1;
    private Monkey _monkey2;
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        view = Matrix.CreateLookAt(
            new Vector3(0, 3, 15),
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
        _tigerModel =  Content.Load<Model>("models/Tiger");
        Texture2D tigerTexture = Content.Load<Texture2D>("models/Tiger_Diffuse");
        _tiger1 = new Tiger(_tigerModel, tigerTexture, new Vector3(-5f, 0.48f, 7f), _scale: 1.2f, walkSpeed: 2f);
        _tiger2 = new Tiger(_tigerModel, tigerTexture, new Vector3(0f, 0.3f, 10f), _scale: 1.0f, walkSpeed: 1.3f);
        
        _birdModel = Content.Load<Model>("models/Love_birds");
        _bird1 = new Bird(_birdModel, new Vector3(-12f, 1f, -10f), 0.1f, 6f,
            0.8f, 20f, Color.Aqua);
        _bird2 = new Bird(_birdModel, new Vector3(-17f, 2f, -15f), 0.08f, 10f,
            2f, 40f, Color.Red);

        _monkeyModel = Content.Load<Model>("models/monkey");
        Texture2D monkeyTexture = Content.Load<Texture2D>("models/monkey_fur");
        _monkey1 = new Monkey(_monkeyModel, monkeyTexture, new Vector3(3f, 0.5f, 5f), _scale: 1.0f, swingSpeed: 1.5f);
        _monkey2 = new Monkey(_monkeyModel, monkeyTexture, new Vector3(-2f, 0.3f, 3f), _scale: 0.7f, swingSpeed: 2.5f);

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _tiger1.Update(gameTime);
        _tiger2.Update(gameTime);
        
        _bird1.Update(gameTime);
        _bird2.Update(gameTime);
        _monkey1.Update(gameTime);
        _monkey2.Update(gameTime);
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
         _tiger1.Draw(view, projection);
         _tiger2.Draw(view, projection);
         
         _bird1.Draw(view, projection);
         _bird2.Draw(view, projection);
         _monkey1.Draw(view, projection);
         _monkey2.Draw(view, projection);

        base.Draw(gameTime);
    }
}