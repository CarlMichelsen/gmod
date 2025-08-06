namespace Presentation.Dto.Image;

public record SizeDto(
    int X,
    int Y)
{
    public int SquareResolution => (int)Math.Round((double)Math.Max(this.X, this.Y));
    
    public int PixelAmount => this.X * this.Y;
}