using System.Collections.Generic;
using System.Reflection;

namespace GCore.AppSystem;

public interface IAppSystemManagerBuilder
{
    void AddScannableAssemblies(params Assembly[] assembly);

    void AddScannableAssemblies(IEnumerable<Assembly> assembly);

    void AddExtensions(params IAppSystemExtension[] extensions);

    void AddExtensions(IEnumerable<IAppSystemExtension> extensions);


    IAppSystemManager Build();
}