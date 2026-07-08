using VContainer;

namespace RoboRoutine
{
    public static class ContainerBuilderExtensions
    {
        public static RegistrationBuilder RegisterEager<T>(this IContainerBuilder builder, Lifetime lifetime = Lifetime.Singleton) where T : class
        {
            builder.RegisterBuildCallback(r => r.Resolve<T>());
            return builder.Register<T>(lifetime);
        }

    }
}
