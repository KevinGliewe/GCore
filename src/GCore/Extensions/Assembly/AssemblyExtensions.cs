using System;
using System.IO;
using System.Reflection;

namespace GCore.Extensions.AssemblyEx;

public static class AssemblyExtensions
{

    // https://stackoverflow.com/a/283917
    public static string GetBetterLocation(this Assembly self) => 
        Uri.UnescapeDataString(new UriBuilder(self.CodeBase).Path);

    public static string GetBetterLocationDir(this Assembly self) =>
        Path.GetDirectoryName(GetBetterLocation(self));
}