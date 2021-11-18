using System;
using System.Linq;

namespace GCore.Attributes
{
    /// <summary>
    /// Dynamic attribute to create generic attributes.
    /// 
    /// Example:
    /// <code>
    /// [D(typeof(List&lt;int&gt;), new int[] {1, 2, 3})]
    /// [D("AttributeName", typeof(List&lt;int&gt;), new int[] {1, 2, 3})]
    /// </code>
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class DAttribute : Attribute , Data.INamedValued<object>
    {
        /// <summary>
        /// The name of this attribute
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The value of the attribute.
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Create a new named attribute.
        /// </summary>
        /// <param name="name">The name of this attribute.</param>
        /// <param name="type">The type of <see cref="DAttribute.Value"/>.</param>
        /// <param name="args">
        /// The arguments to pass to the constructor.
        /// </param>
        public DAttribute(
            string name,
            Type type,
            params object[] args)
            : this(type, args)
        {
            Name = name;
        }
        /// <summary>
        /// Create a new attribute.
        /// </summary>
        /// <param name="type">The type of <see cref="DAttribute.Value"/>.</param>
        /// <param name="args">
        /// The arguments to pass to the constructor.
        /// </param>
        public DAttribute(
            Type type,
            params object[] args)
        {

            {
                // Check for constructors with matching argument count and type
                System.Reflection.ConstructorInfo constructor = type.GetConstructors()
                        .Where(cc => cc.IsPublic)
                        .Where(cc => cc.GetParameters().Count() == args.Length)
                        .Where(cc =>
                            cc.GetParameters().Select((p, i) =>
                                p.ParameterType == (args[i]?.GetType() ?? typeof(string))
                            ).All(b => b)
                        ).FirstOrDefault();
                Value = constructor?.Invoke(args);
            }


            // Check for constructors with matching argument count and try converting them to the correct type
            if (Value is null)
            {
                try
                {
                    System.Reflection.ConstructorInfo constructor = type.GetConstructors()
                        .Where(cc => cc.IsPublic)
                        .Where(cc => cc.GetParameters().Count() == args.Length)
                        .FirstOrDefault();

                    Value = constructor?.Invoke(
                        constructor.GetParameters().Select((a, i) =>
                            Convert.ChangeType(args[i], a.ParameterType)
                        ).ToArray());
                }
                catch (Exception) { } // Lets try the fallback
            }

            // Fallback using Activator
            if (Value is null)
                Value =
                    Activator.CreateInstance(type, args);
        }

        public override string ToString()
        {
            return $"DAttribute(Name=\"{Name ?? "<NULL>"}\" Value={Value})";
        }
    }
}