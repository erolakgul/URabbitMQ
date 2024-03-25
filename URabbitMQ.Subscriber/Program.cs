using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ysynpygm:vY1Dp4qZ437uU3Dojt0nnY9-FFKb8H8G@toad.rmq.cloudamqp.com/ysynpygm");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

//channel.QueueDeclare(queue: "test-1", durable: true, exclusive: false, autoDelete: false);

// alıcı taraf için 
var consumer = new EventingBasicConsumer(channel);
// queue kuyruk ismi
// autoAck true olursa, mesaj alındığında direkt siler,false dersek subscriber lardan haber bekler silmesi için, grrçek dünyada false a set edilr
// consumer kanala ait alıcının bilgisi
channel.BasicConsume(queue: "test-1", autoAck: true,consumer:consumer);

consumer.Received +=
    (sender, args) =>
    {
        var msg = Encoding.UTF8.GetString(args.Body.ToArray());
        Console.WriteLine($"Gelen Mesaj : {msg}");
    };
Console.ReadLine();