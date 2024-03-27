
using RabbitMQ.Client;
using System.Text;
using URabbitMQ.Publisher;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ysynpygm:vY1Dp4qZ437uU3Dojt0nnY9-FFKb8H8G@toad.rmq.cloudamqp.com/ysynpygm");
using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

// fanout da publisher kuyruk deklere etmeyecek, consumer artık açacak
//channel.QueueDeclare(queue:"test-1",durable:true,exclusive:false,autoDelete:false);
string exhangeName = "log-topic";
// exchange deklera ediyoruz şimdi
// exchange: excahnge ismi
// type    : exchange tipini seçiyoruz
// durable : true ise fiziksel olarak kaydedilsin işlem 
// autodel :
// args    :
channel.ExchangeDeclare(exchange: exhangeName, type: ExchangeType.Topic, durable: false, autoDelete: true, arguments: null);


// kuruğa mesaj gönderimi
Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    Random rnd = new();
    LogTypes logType1 = (LogTypes)rnd.Next(1, 5);
    LogTypes logType2 = (LogTypes)rnd.Next(1, 5);
    LogTypes logType3 = (LogTypes)rnd.Next(1, 5);

    var routeKey = $"{logType1}.{logType2}.{logType3}";

    string message = $"Log-Route-Type : {logType1}-{logType2}-{logType3}";
    var messageBody = Encoding.UTF8.GetBytes(message);

    // exchange : burada ismini veriyoruz
    // routingKey : bu sefer bir kuyruk ismi vermiyoruz fanout da
    channel.BasicPublish(exchange: exhangeName, routingKey: routeKey, basicProperties: null, body: messageBody);
    Console.WriteLine($"Gönderilen mesaj :{message}");
});

Console.ReadLine();