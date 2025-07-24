using System.Reflection;

namespace C_Sharp11.GenericAttributes
{
    public interface IVehicleValidator<T> where T : class
    {
        bool IsValid(T entity);
    }

    public class Car
    {
        public bool IsConvertible { get; set; }
    }

    public class Truck
    {
        public string? LoadCapacity { get; set; }
    }

    public class CarValidator : IVehicleValidator<Car>
    {
        public bool IsValid(Car car) => car.IsConvertible;
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class VehicleValidatorAttribute<T, TEntity> : Attribute
        where T : class, IVehicleValidator<TEntity>
        where TEntity : class
    {
    }

    public static class ValidationHelper
    {
        public static IVehicleValidator<T>? GetValidator<T>() where T : class
        {
            var modelType = typeof(T);

            var validatorAttr = modelType
                .GetCustomAttribute(typeof(VehicleValidatorAttribute<,>));

            if (validatorAttr is not null)
            {
                var validatorType = validatorAttr
                    .GetType()
                    .GetGenericArguments()
                    .First();

                return Activator.CreateInstance(validatorType) as IVehicleValidator<T>;
            }

            return null;
        }
    }
}
