using AutoMapper;
using CommandService.Models;
using Grpc.Core;
using Grpc.Net.Client;
using PlatformService;

namespace CommandService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public PlatformDataClient(IConfiguration configuration, IMapper mapper)
        {
            this.configuration = configuration;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Platform>> ReturnAllPlatforms()
        {
            Console.WriteLine($"--> Calling gRPC Service {configuration["GrpcPlatform"]}");
            var channel = GrpcChannel.ForAddress(configuration["GrpcPlatform"]);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = await client.GetAllPlatformsAsync(request);
                return mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"--> gRPC Service threw an exception: {ex.Message}");
                return null;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not call gRPC Service: {ex.Message}");
                return null;
            }
            
        }
    }
}
