namespace Coolector.Services.Statistics.Modules
{
    public class HomeModule : ModuleBase
    {
        public HomeModule()
        {
            Get("", args => "Welcome to the Coolector.Services.Statistics!");
        }
    }
}