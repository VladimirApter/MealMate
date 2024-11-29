using System.Runtime.CompilerServices;

namespace Domain.Logic;

public static class DataBasePathGetter
{
    public static readonly string DataBasePath = Environment.GetEnvironmentVariable("DATABASE_PATH");
}