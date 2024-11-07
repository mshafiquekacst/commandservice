
using CommandService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CommandService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly IEventProcessor eventProcessor;
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;
        private string queueName;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            this.configuration = configuration;
            this.eventProcessor = eventProcessor;
            InitializeRabbitMQ();
        }
        private void InitializeRabbitMQ()
        {
            factory = new ConnectionFactory() { HostName = configuration["RabbitMQHost"], Port = int.Parse(configuration["RabbitMQPort"]) };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: "trigger", routingKey: "");
            Console.WriteLine("Listening on the message bus...");
            
            connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            //
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            const string logMessage = "RabbitMQ Connection Shutdown";
            Console.WriteLine($"--> {logMessage}");
        }
        public override void Dispose()
        {
            if (this.channel.IsOpen)
            {
                this.channel.Close();
                this.connection.Close();
            }
            base.Dispose();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Event Received!");
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                eventProcessor.ProcessEvent(notificationMessage);
            };
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }
    }
}
