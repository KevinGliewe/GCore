using Autofac;
using Microsoft.Extensions.Configuration;
using Zio;

namespace GCore.AppSystem;

public interface IAppSystemManager
{
    IConfiguration Config { get; }
    IContainer Services { get; }
    IFileSystem FileSystem { get; }
}