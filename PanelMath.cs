using System;

namespace PanelWork;

public static class PanelMath {
    public static int Clamp(int value, int min, int max) {
        return Math.Max(min, Math.Min(max, value));
    }
}
