using System.Data.Common;
using Silk.NET.Maths;

namespace TheAdventure.Models;

public class SpriteSheet
{
    private readonly int _columns;
    private readonly int _rows;
    private readonly int _frameWidth;
    private readonly int _frameHeight;

    public SpriteSheet(int textureWidth, int textureHeight, int columns, int rows)
    {
        _columns = columns;
        _rows = rows; 
        _frameWidth = textureWidth / columns;
        _frameHeight = textureHeight / rows;
    }

    public Rectangle<int> GetFrame(int frame, int row)
    {
        int col = frame % _columns;
        return new Rectangle<int>(
            col * _frameWidth, row * _frameHeight, _frameWidth, _frameHeight
        );
    }

    public int FrameWidth => _frameWidth;
    public int FrameHeight => _frameHeight;

}