namespace Models.Interfaces {
    public interface IGrid {
        int[] Cells { get; }

        int GetCell(int x, int y);
        void SetCell(int x, int y, int cell);
        void ClearGrid();
    }
}