using System.Reflection;
using Fuchu;

namespace QuartzNetWebConsole.Tests {
    public static class Runner {
        public static int Main(string[] args) {
            return Test.FromAssembly(Assembly.GetExecutingAssembly()).RunParallel();
        }
    }
}