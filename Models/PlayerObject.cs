using Silk.NET.Maths;

namespace TheAdventure.Models;

// Player character, a crow
// Class handles movement, jumps, attack logic, animation, and rendering.
public class PlayerObject: GameObject
{
    // Character position when game starts
    public int X = 0;
    public int Y = 0;

    // Player health
    public int HP = 7;

    // data necessary for jumps
    private bool _isGrounded = true;
    private int _groundY;
    private double _verticalVelocity = 0;

    // Attack state and type
    private bool _isAttacking = false;
    private int _attackType = 0;

    // Makes sure that attack has damage just once
    public bool _didDamage { get; set; } = false;

    // Rendering and animation
    private int _texture;
    private int _frames;
    private int _row = 0;
    private int _current_Frame;
    private double _time;
    private const int Speed = 200;

    // Sprite animation handler
    private SpriteSheet _sheet;

    // Destination regctangle (where the sprite is drawn)
    public Rectangle<int> Dest;

    // Render reference
    private readonly GameRenderer _renderer;

    public PlayerObject(GameRenderer renderer, int groundY) : base()
    {
        _renderer = renderer;
        _groundY = groundY;

        // Loads default running animation
        _texture = renderer.LoadTexture(@"Assets\Karasu_tengu\Run.png", out var tex);
        _frames = 8;
        _sheet = new SpriteSheet(tex.Width, tex.Height,_frames,1);
        X = 0;

         // Places player on ground
        Y = _groundY - _sheet.FrameHeight;
        Dest = new Rectangle<int>(X,Y,_sheet.FrameWidth,_sheet.FrameHeight);
    }

    public void Update(bool left, bool right, bool up, double dt, bool attack)
    {
       double move = Speed * (dt / 1000);

        // Player moves only if it is not attacking
        if (!_isAttacking)
        {
            // Moves by changing X coordonate
            if(left)X -= (int)move;

            if(right)X += (int)move;

            // Jumps
            if(up && _isGrounded)
            {
                _verticalVelocity = -800;
                _isGrounded = false;
            }
        }

        // Returns to ground
        if (!_isGrounded)
        {
            _verticalVelocity += 2000 * (dt / 1000.0);
            Y += (int)(_verticalVelocity * (dt / 1000));
        }

        int playerFeetY = Y + _sheet.FrameHeight;

        if(playerFeetY >= _groundY)
        {
            Y = _groundY - _sheet.FrameHeight;
            _isGrounded = true;
            _verticalVelocity = 0;
        }

        // Starts attack by choosing random player attack type sprite
        if(attack && !_isAttacking)
        {
            _isAttacking = true;
            _didDamage = false;
            _attackType = Random.Shared.Next(1,4);
            string file = $@"Assets\Karasu_tengu\Attack_{_attackType}.png";
            _texture = _renderer.LoadTexture(file,out var tex);
            _frames = _attackType switch
            {
                1 => 6,
                2 => 4,
                3 => 3,
                _ => 1
            };
            _sheet = new SpriteSheet(tex.Width,tex.Height,_frames,1);
            _current_Frame = 0;
            _time = 0;
        }

        _time += dt;

        if(_time > 120)
        {
            _time = 0;
            _current_Frame++;

            // Loop animation
            if(_current_Frame >= _frames)
            {
                _current_Frame = 0;

                // Ends attack by reloading default running animation
                if (_isAttacking)
                {
                    _isAttacking = false;
                    _texture = _renderer.LoadTexture(@"Assets\Karasu_tengu\Run.png", out var tex);
                    _sheet = new SpriteSheet(tex.Width,tex.Height,8,1);
                    _frames = 8;
                }
            }
        }

        var world = _renderer.GetWorld();

        // Keeps player character between screen bounds
        if(X < world.Origin.X)
        {
            X = world.Origin.X;
        }

        if(X + _sheet.FrameWidth > world.Size.X)
        {
            X = world.Size.X - _sheet.FrameWidth;
        }

        if(Y < 0)
        {
            Y = 0;
        }

        // Updates position for rendering
        Dest = new Rectangle<int>(X,Y,_sheet.FrameWidth,_sheet.FrameHeight);
    }

    // Loads dying animation
     public void Die()
    {
        _isAttacking = false;
        _texture = _renderer.LoadTexture(@"Assets\Karasu_tengu\Dead.png", out var tex);
        _frames = 6;
        _sheet = new SpriteSheet(tex.Width, tex.Height, _frames, 1);
        _current_Frame = 0;
        _time = 0;
    }

    public void Render(GameRenderer renderer)
    {
        renderer.RenderTexture(_texture, _sheet.GetFrame(_current_Frame,_row), Dest);
    }

    public bool IsAttacking => _isAttacking;
}