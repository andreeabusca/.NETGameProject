using Silk.NET.Maths;
namespace TheAdventure.Models;


public class RenderableGameObject: GameObject
{
    public int TextureId { get; set;}
    public Rectangle<int> TextureSource { get; set;}
    public Rectangle<int> TextureDestination { get; set; }
    public TextureData TextureInformation { get; set; }

    public RenderableGameObject(string file, GameRenderer renderer)
    {
        TextureId = renderer.LoadTexture(file, out var tex);
        TextureInformation = tex;
        TextureSource = new Rectangle<int>(0,0, tex.Width, tex.Height);
        TextureDestination = new Rectangle<int>(0, 0, tex.Width, tex.Height);

    }

    public virtual bool Update(double dt) => true;

    public virtual void Render(GameRenderer renderer)
    {
        renderer.RenderTexture(TextureId, TextureSource, TextureDestination);
    }
}
