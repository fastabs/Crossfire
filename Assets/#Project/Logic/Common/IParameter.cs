namespace Crossfire.Workspace
{
    public interface IParameter<T>
        where T : struct
    {
        T Value { get; }
    }
}

