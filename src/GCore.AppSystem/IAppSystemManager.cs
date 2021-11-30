using Autofac;
using Microsoft.Extensions.Configuration;

namespace GCore.AppSystem;

public interface IAppSystemManager
{
    IConfiguration Config { get; }
    IContainer Services { get; }
}