using CommandService.Models;
using CommandService.SyncDataServices.Grpc;

namespace CommandService.Data
{
    public static class PrepDb
    {
        public async static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
                var platforms = await grpcClient.ReturnAllPlatforms();
                SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
            }
        }

        public async static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("--> Seeding Data...");
            foreach (var platform in platforms)
            {
                if (!await repo.ExternalPlatformExist(platform.ExternalID))
                {
                    repo.CreatePlatform(platform);
                }

            }
            //if (!context.Platforms.Any())
            //{
            //    Console.WriteLine("--> Seeding Data...");

            //    context.Platforms.AddRange(
            //                           new Platform() { Name = "DotNet", Commands = new List<Command>() { new Command() { HowTo = "Generate a Web API", CommandLine = "dotnet new webapi" } } },
            //                                              new Platform() { Name = "Docker", Commands = new List<Command>() { new Command() { HowTo = "Docker PS", CommandLine = "docker ps" } } },
            //                                                                 new Platform() { Name = "Azure", Commands = new List<Command>() { new Command() { HowTo = "List Azure Apps", CommandLine = "az webapp list" } } }
            //                                                                                );

            //    context.SaveChanges();
            //}
            //else
            //{
            //    Console.WriteLine("--> We already have data");
            //}
        }

    }
}
