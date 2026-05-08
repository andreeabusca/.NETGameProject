using Silk.NET.Maths;
using Silk.NET.SDL;
using TheAdventure.Models;

namespace TheAdventure;

public class HPRenderer
{
    // Render reference
    private readonly GameRenderer _renderer;
    private int _lifeTexture;

    // Stores width and height of the hp texture
    private TextureData _lifeTextureData;

    public HPRenderer(GameRenderer renderer)
    {
        _renderer = renderer;

        // Loads hp texture
        _lifeTexture = _renderer.LoadTexture(@"Assets\1.png", out _lifeTextureData);
    }

    // Draws HP icons for both Player and Enemy Objects
    public void RenderLives(PlayerObject player, EnemyObject enemy)
    {
        int hpSize = 32; // display size for icons
        int padding = 5; // space between icons
        int margin = 10; // distance from game screen edges

        var screen = _renderer.GetWindowSize();

        // Renders Player HP, left-aligned
        for(int i = 0; i < player.HP; i++)
        {
            // Takes full hp texture
            var src = new Rectangle<int>(0,0,_lifeTextureData.Width,_lifeTextureData.Height);

            // Specifies place on screen to put hp icon
            var dst = new Rectangle<int>(margin + i * (hpSize + padding), margin, hpSize,hpSize);

            _renderer.RenderTextureUI(_lifeTexture,src,dst);
        }

        // Renders Enemy HP, right-aligned
         for(int i = 0; i < enemy.HP; i++)
        {
            // Takes full hp texture
            var src = new Rectangle<int>(0,0,_lifeTextureData.Width,_lifeTextureData.Height);

            //Specifies place on screen to put hp icon
            var dst = new Rectangle<int>(screen.Width - hpSize - margin - (i  * (hpSize + padding)), margin, hpSize,hpSize);

            _renderer.RenderTextureUI(_lifeTexture,src,dst);
        }

    }

}