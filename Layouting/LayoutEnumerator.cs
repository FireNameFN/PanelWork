using PanelWork.Components;

namespace PanelWork.Layouting;

public struct LayoutEnumerator(LayoutUnit[] units, int index, int count) {
    readonly LayoutUnit[] units = units;

    public bool Next(out int subpanelIndex) {
        subpanelIndex = index;

        index = units[index].NextIndex;

        return count-- > 0;
    }

    public bool Next(out int subpanelIndex, out LayoutComponent layout) {
        bool ok = Next(out subpanelIndex);

        layout = units[subpanelIndex].Layout;

        return ok;
    }
}
