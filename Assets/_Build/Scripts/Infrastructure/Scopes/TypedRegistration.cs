namespace LostKaiju.Infrastructure.Scopes
{
    public class TypedRegistration<TType, TInstance> where TInstance : new()
    {
        public TInstance Instance { get; }

        public TypedRegistration()
        {
            Instance = new();
        }

        public TypedRegistration(TInstance instance)
        {
            Instance = instance;
        }
    }
}