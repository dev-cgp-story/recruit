namespace World.Object
{
    public interface IObject
    {
        string Name { get; }
        void Attach(IObject parent);
        void OnDestroy();
    }
}