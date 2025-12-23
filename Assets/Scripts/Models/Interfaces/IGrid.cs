namespace Models.Interfaces {
    public interface IGrid {
        int[] Cells { get; }

        void SetCell(int x, int y, int cell);
        void ClearGrid();
    }
}