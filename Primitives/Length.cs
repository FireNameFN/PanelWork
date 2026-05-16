namespace PanelWork.Primitives;

public readonly struct Length {
    public static readonly Length Grow = Star(1);

    public double Value { get; }

    public int PixelValue { get; }

    public LengthUnit Unit { get; }

    private Length(double value, LengthUnit unit) {
        Value = value;
        Unit = unit;
    }

    private Length(int pixel) {
        PixelValue = pixel;
    }

    public static Length Pixel(int pixel) {
        return new(pixel);
    }

    public static Length Share(double value) {
        return new(value, LengthUnit.Share);
    }

    public static Length Star(double value) {
        return new(value, LengthUnit.Star);
    }

    public static implicit operator Length(int pixel) {
        return Pixel(pixel);
    }
}
