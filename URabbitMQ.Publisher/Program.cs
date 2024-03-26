
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ysynpygm:vY1Dp4qZ437uU3Dojt0nnY9-FFKb8H8G@toad.rmq.cloudamqp.com/ysynpygm");
using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.QueueDeclare(queue:"test-1",durable:true,exclusive:false,autoDelete:false);

// kuruğa mesaj gönderimi
Enumerable.Range(1, 50).ToList().ForEach( x=> 
{
    string message = $"Message {x}";
    var messageBody = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(exchange: string.Empty, routingKey: "test-1", basicProperties: null, body: messageBody);
    Console.WriteLine($"Gönderilen mesaj :{message}");
});

Console.ReadLine();