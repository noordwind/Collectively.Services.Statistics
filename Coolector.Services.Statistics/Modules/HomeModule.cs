namespace Coolector.Services.Statistics.Modules
{
    public class HomeModule : ModuleBase
    {
        public HomeModule() : base(requireAuthentication: false)
        {
            Get("", args => "Welcome to the Coolector.Services.Statistics API!");
        }
    }
}