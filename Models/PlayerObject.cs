using Silk.NET.Maths;

namespace TheAdventure.Models;

public class PlayerObject: GameObject
{
    public int X = 0;
    public int Y = 0;
    public int HP = 7;
    private int _texture;
    private int _frames;
    private int _row = 0;
    private SpriteSheet _sheet;
    private int _current_Frame;
    private double _time;
    private const int Speed = 200;
    private bool _isAttacking = false;
    public bool _didDamage { get; set; } = false;
    private bool _isGrounded = true;
    private double _verticalVelocity = 0;
    private int _attackType = 0;
    public Rectangle<int> Dest;
    private readonly GameRenderer _renderer;
    private int _groundY;

    public PlayerObject(GameRenderer renderer, int groundY) : base()
    {
        _renderer = renderer;
        _groundY = groundY;
        _texture = renderer.LoadTexture(@"Assets\Karasu_tengu\Run.png", out var tex);
        _frames = 8;
        _sheet = new SpriteSheet(tex.Width, tex.Height,_frames,1);
        X = 0;
        Y = _groundY - _sheet.FrameHeight;
        Dest = new Rectangle<int>(X,Y,_sheet.FrameWidth,_sheet.FrameHeight);
    }

    public void Update(bool left, bool right, bool up, double dt, bool attack)
    {
       double move = Speed * (dt / 1000);
        if (!_isAttacking)
        {
            if(left)X -= (int)move;
            if(right)X += (int)move;
            if(up && _isGrounded)
            {
                _verticalVelocity = -800;
                _isGrounded = false;
            }
        }

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
        if(_time > 100)
        {
            _time = 0;
            _current_Frame++;
            if(_current_Frame >= _frames)
            {
                _current_Frame = 0;
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
        Dest = new Rectangle<int>(X,Y,_sheet.FrameWidth,_sheet.FrameHeight);
    }

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