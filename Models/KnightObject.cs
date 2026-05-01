using Silk.NET.Maths;

namespace TheAdventure.Models;

public class KnightObject: GameObject
{
    public int X = 200;
    public int Y = 0;
    public int HP = 3;
    private readonly GameRenderer _renderer;
    private int _texture;
    private SpriteSheet _sheet;
    private int _frame;
    private int _frames;
    private int _row = 0;
    private double _time;
    private bool _attacking;
    private double _attackTimer;
    public Rectangle<int> Dest;
    private int _groundY;

    public KnightObject(GameRenderer renderer, int groundY) : base()
    {
        _renderer = renderer;
        _groundY = groundY;
        _texture = _renderer.LoadTexture(@"Assets\knight1_spritelist_1.png", out var tex);
        _sheet = new SpriteSheet(tex.Width, tex.Height,8,1);
        _frames = 8;
        _row = 1;
        Dest = new Rectangle<int>(X,Y,_sheet.FrameWidth,_sheet.FrameHeight);
    }

    public void Update(double dt)
    {
        _time += dt;
        if(_time > 120)
        {
            _time = 0;
            _frame = (_frame + 1) %  _frames;

        }

        if(!_attacking && Random.Shared.NextDouble() < 0.01)
        {
            _attacking = true;
            _attackTimer = 0;
            _row = Random.Shared.Next(4,5);
            _frames = 4;
            _frame = 0;
        }
        if (_attacking)
        {
            _attackTimer += dt;
            if(_attackTimer > 800)
            {
                _attacking = false;
                _row = 1;
                _frames = 7;
                _frame = 0;
            }
        }

         Dest = new Rectangle<int>(X, _groundY - _sheet.FrameHeight,_sheet.FrameWidth,_sheet.FrameHeight);
    }

     public void Render(GameRenderer renderer)
    {
        renderer.RenderTexture(_texture, _sheet.GetFrame(_frame,_row), Dest);
    }

    public bool IsAttacking => _attacking;

}