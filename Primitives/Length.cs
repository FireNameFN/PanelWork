namespace PanelWork.Primitives;

public readonly struct Length(double value, LengthUnit unit) {
    public double Value { get; } = value;

    public LengthUnit Unit { get; } = unit;

    public static Length Pixel(double value) {
        return new(value, LengthUnit.Pixel);
    }

    public static Length Share(double value) {
        return new(value, LengthUnit.Share);
    }

    public static Length Star(double value = 1) {
        return new(value, LengthUnit.Star);
    }

    public static implicit operator Length(double value) {
        return Pixel(value);
    }
}
