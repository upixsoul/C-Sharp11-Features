using C_Sharp11.GenericAttributes;
using System.Numerics;
using System.Text;

namespace C_Sharp11
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello, Welcome to DAGG C# 11 Catalog");
            Console.WriteLine("This catalog contains examples of C# 11 features");
            Console.WriteLine("These features are available in .NET Core 7.0+ since 2022");

            #region RawStringLiterals
            Console.WriteLine("1.- Raw String Literals");
            Console.WriteLine("Write multi-line strings without escape characters.");

            string json = """
                          {
                            "name": "David",
                            "role": "Scrum Master"
                          }
                          """;

            Console.WriteLine(json);
            #endregion

            #region GenericMathSupport
            Console.WriteLine();
            Console.WriteLine("2.- Generic Math Support");
            Console.WriteLine("Use mathematical operators in generic types via static abstract members in interfaces.");

            Console.WriteLine(Add(5, 10));          // Output: 15
            Console.WriteLine(Add(3.5, 2.5));      // Output: 6.0
            Console.WriteLine(Add(10m, 20m));      // Output: 30.0
            Console.WriteLine();
            #endregion

            #region GenericAttributes
            Console.WriteLine();
            Console.WriteLine("3.- Generic Attributes");
            Console.WriteLine("Define attributes with type parameters.");

            var carValidator = ValidationHelper.GetValidator<Car>();
            var car = new Car()
            {
                IsConvertible = true
            };
            Console.WriteLine(carValidator?.IsValid(car));
            Console.WriteLine("This approach mimics generic attributes by leveraging the Type class");
            #endregion

            #region UTF8StringLiterals
            Console.WriteLine();
            Console.WriteLine("4.- UTF-8 String Literals");
            Console.WriteLine("Use u8 suffix to create UTF-8 encoded strings.");
            ReadOnlySpan<byte> utf8 = "Hola, mundo!"u8;

            Console.WriteLine(utf8.ToString());
            Console.WriteLine(Encoding.UTF8.GetString(utf8));
            #endregion

            #region RequiredMembers
            Console.WriteLine();
            Console.WriteLine("5.- Required Members");
            Console.WriteLine("Force initialization of properties or fields.");
            User user = new User
            {
                Name = "David"
                //,Age = 30 // Error: 'Age' is required but not set
                ,Age = 30
            };

            Console.WriteLine($"User Name: {user.Name} Age: {user.Age}");
            // Uncommenting the next line will cause a compile-time error
            Console.WriteLine("El miembro requerido 'Program.User.Age' debe establecerse en el inicializador de objeto o constructor de atributos.");
            Console.WriteLine("'Age' is required but not set");
            Console.WriteLine();
            #endregion

            #region ListPatterns
            Console.WriteLine();
            Console.WriteLine("6.- List Patterns");
            Console.WriteLine("Match arrays or lists with specific patterns.");

            int[] numbers = { 1, 2, 3, 4, 5 };

            if (numbers is [1, 2, .., 5])
            {
                Console.WriteLine("Pattern matched!");
            }
            #endregion

            #region AutoDefaultStructs
            Console.WriteLine();
            Console.WriteLine("7.- Auto Default Structs");
            Console.WriteLine("the CategoryId property is not initialized in the constructor. With previous version of C# 11, it leads to the following compilation error:");
            Console.WriteLine("Error  CS0171 Field ‘Product.CategoryId’ must be fully assigned before control is returned to the caller.");
            
            Person person = new Person("David");
            Console.WriteLine($"Person Name: {person.Name}, CategoryId: {person.CategoryId}");
            #endregion

            #region NewlinesInStringInterpolation
            Console.WriteLine();
            Console.WriteLine("8.- Newlines in String Interpolation");
            Console.WriteLine("Multiline expressions inside interpolated strings.");
            var a = 5;
            var b = 10;
            var total = a + b;
            var result = $"""
                          a is: {a}
                          b is: {b}
                          the total is: {total}
                          """;

            Console.WriteLine(result);
            #endregion

            #region RefFieldsAndScopedRef
            Console.WriteLine();
            Console.WriteLine("9.- Ref Fields and Scoped Ref");
            Console.WriteLine("is used to ensure that references (especially ref structs) do not outlive their intended scope, helping to prevent unsafe memory access");
            Span<int> numbersSpan = stackalloc int[] { 1, 2, 3, 4, 5 };
            UseScopedRef(numbersSpan);
            #endregion

            #region ExtendedNameofScope
            Console.WriteLine();
            Console.WriteLine("10.- Extended nameof Scope");
            Console.WriteLine("Use nameof in attribute declarations referencing method parameters. Example in errors");

            //MyMethod("argument one value");
            try
            {
                MyMethod(string.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine();
            #endregion

            #region ImprovedMethodGroupConversionToDelegate
            Console.WriteLine();
            Console.WriteLine("11.- Improved Method Group Conversion to Delegate");
            Console.WriteLine("myAction and anotherAction could reference the same delegate instance, demonstrating the improved method group conversion to delegate feature");
            Action myAction = MyOtherMethod;
            Action anotherAction = MyOtherMethod;

            // In C# 11, myAction and anotherAction may reference the same delegate instance.
            Console.WriteLine(System.Object.ReferenceEquals(myAction, anotherAction)); // Might print 'True'
            Console.WriteLine();
            #endregion
        }

        // 2.- Generic method using generic math support
        public static T Add<T>(T a, T b) where T : INumber<T>
        {
            return a + b;
        }

        // 5.- Required members example
        public class User
        {
            public required string Name { get; init; }
            public required int Age { get; init; }
        }

        // 7.- Auto default structs example
        public struct Person
        {
            public Person(string name)
            {
                Name = name;
            }

            public string Name;
            public int CategoryId;
        }

        // 9.- Ref fields and scoped ref example
        static void UseScopedRef(scoped Span<int> span)
        {
            // The `scoped` keyword ensures `span` cannot escape this method's scope.
            Console.WriteLine($"First element: {span[0]}");
        }
        // 10.- Extended nameof scope example
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
        public class LogAttribute : Attribute
        {
            public LogAttribute(string endpointName, params string[] parameters)
            {
                Console.WriteLine();
                Console.WriteLine($"Endpoint {endpointName} takes the following parameters: {string.Join(", ", parameters)}");
            }
        }

        // 10.- Extended nameof scope example
        [Log("MyMethod", nameof(agumentOne))]
        public static void MyMethod(string agumentOne)
        {
            if (string.IsNullOrWhiteSpace(agumentOne))
            {
                throw new ArgumentException("Data cannot be null or whitespace.", nameof(agumentOne));
            }
            Console.WriteLine($"Method called with argument: {agumentOne}");
        }
        // 11.- Improved method group conversion to delegate example
        static void MyOtherMethod()
        {
            Console.WriteLine("My Other Method executed");
        }
    }


}
