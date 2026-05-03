using Silk.NET.Maths;

namespace TheAdventure.Models;

// Enemy character, a fox
// Class handles movement, attack logic, animation, and rendering.
public class EnemyObject: GameObject
{
    // Character position when game starts
    public int X = 350;
    public int Y = 0;
    
    // Enemy health
    public int HP = 3;

    // Ground level reference
    private int _groundY;

    // Movement direction ( 1 = right, -1 = left)
    private int _direction  = 1;

    // Attack state and type
    private bool _attacking;
    private int _attackType = 0;

    // Makes sure that attack has damage just once
     public bool _didDamage { get; set; } = false;

    // Rendering and animation
     private int _texture;
     private int _frame;
    private int _frames;
    private int _row = 0;
    private double _time;
    private int Speed = 100;

    // Sprite animation handler
     private SpriteSheet _sheet;

    // Destination regctangle (where the sprite is drawn)
     public Rectangle<int> Dest;

    // Render reference
    private readonly GameRenderer _renderer;

    public EnemyObject(GameRenderer renderer, int groundY) : base()
    {
        _renderer = renderer;
        _groundY = groundY;

        // Loads default running animation
        _texture = _renderer.LoadTexture(@"Assets\Kitsune\Run.png", out var tex);
        _sheet = new SpriteSheet(tex.Width, tex.Height,8,1);
        _frames = 8;
        _row = 0;

        // Places enemy on ground
        Y = _groundY - _sheet.FrameHeight;
        Dest = new Rectangle<int>(X,Y,_sheet.FrameWidth,_sheet.FrameHeight);
    }

    public void Update(double dt)
    {
        var world = _renderer.GetWorld();

        // Enemy moves only if it is not attacking
        if(!_attacking )
        {
            // Enemy moves by modifying X coordonate
            X += (int)(Speed * (dt / 1000.0) * _direction);

            // Change direction when reaching screen edges
            if(X < world.Origin.X)
            {
                _direction *= -1;
            }
            if(X + _sheet.FrameWidth > world.Size.X)
            {
               _direction *= -1;
            }

            // Enemy attacks at random
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

            // Loop animation
             if (_frame >= _frames)
            {
                _frame = 0;

                // Ends attack animation if all frames have been displayed
                if (_attacking)
                {
                    EndAttack();
                }
            }
           
        }

        // Updates position for rendering
         Dest = new Rectangle<int>(X, _groundY - _sheet.FrameHeight,_sheet.FrameWidth,_sheet.FrameHeight);
    }

    // Starts attack by choosing random enemy attack type sprite
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

    // Ends attack by reloading default running animation
    private void EndAttack()
    {
        _attacking = false;
        _texture = _renderer.LoadTexture(@"Assets\Kitsune\Run.png", out var tex);
        _frames = 8;
        _sheet = new SpriteSheet(tex.Width, tex.Height, _frames, 1);
    }

    // Loads dying animation
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