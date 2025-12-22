namespace Services.Interfaces {
    public interface IObjectPoolService<T> where T : class {
        T Get(int x, int y);
        void Release(T t);
        void Clear();
    }
}