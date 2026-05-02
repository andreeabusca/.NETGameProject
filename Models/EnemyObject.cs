using Silk.NET.Maths;

namespace TheAdventure.Models;

public class EnemyObject: GameObject
{
    public int X = 350;
    public int Y = 0;
    public int HP = 3;
    private int _direction  = 1;
    private int Speed = 100;
    private readonly GameRenderer _renderer;
    private int _texture;
    private SpriteSheet _sheet;
    private int _frame;
    private int _frames;
    private int _row = 0;
    private double _time;
    private bool _attacking;
    private int _attackType = 0;
     public bool _didDamage { get; set; } = false;
    public Rectangle<int> Dest;
    private int _groundY;

    public EnemyObject(GameRenderer renderer, int groundY) : base()
    {
        _renderer = renderer;
        _groundY = groundY;
        _texture = _renderer.LoadTexture(@"Assets\Kitsune\Run.png", out var tex);
        _sheet = new SpriteSheet(tex.Width, tex.Height,8,1);
        _frames = 8;
        _row = 0;
        Y = _groundY - _sheet.FrameHeight;
        Dest = new Rectangle<int>(X,Y,_sheet.FrameWidth,_sheet.FrameHeight);
    }

    public void Update(double dt)
    {
         var world = _renderer.GetWorld();
        if(!_attacking )
        {
            X += (int)(Speed * (dt / 1000.0) * _direction);
            if(X < world.Origin.X)
            {
                _direction *= -1;
            }
            if(X + _sheet.FrameWidth > world.Size.X)
            {
               _direction *= -1;
            }

            if(Random.Shared.NextDouble() < 0.005)
            {
                StartAttack();
            }
            
        }

        _time += dt;
         if(_time > 120)
        {
            _time = 0;
            _frame++;
             if (_frame >= _frames)
            {
                _frame = 0;
                if (_attacking)
                {
                    EndAttack();
                }
            }
           
        }

         Dest = new Rectangle<int>(X, _groundY - _sheet.FrameHeight,_sheet.FrameWidth,_sheet.FrameHeight);
    }

    private void StartAttack()
    {
        _attacking = true;
        _didDamage = false;
        _attackType = Random.Shared.Next(1,4);
        string file  = $@"Assets\Kitsune\Attack_{_attackType}.png";
        _texture = _renderer.LoadTexture(file, out var tex);
        _frames = _attackType switch
        {
            1 => 10,
            2 => 10,
            3 => 7,
            _ => 1
        };
        _sheet = new SpriteSheet(tex.Width, tex.Height, _frames, 1);
        _frame = 0;
    }

    private void EndAttack()
    {
        _attacking = false;
        _texture = _renderer.LoadTexture(@"Assets\Kitsune\Run.png", out var tex);
        _frames = 8;
        _sheet = new SpriteSheet(tex.Width, tex.Height, _frames, 1);
    }

    public void Die()
    {
        _attacking = false;
        _texture = _renderer.LoadTexture(@"Assets\Kitsune\Dead.png", out var tex);
        _frames = 10;
        _sheet = new SpriteSheet(tex.Width, tex.Height, _frames, 1);
    }
     public void Render(GameRenderer renderer)
    {
        renderer.RenderTexture(_texture, _sheet.GetFrame(_frame,_row), Dest);
    }

    public bool IsAttacking => _attacking;

}