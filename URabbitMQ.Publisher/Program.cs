
using RabbitMQ.Client;
using System.Text;
using URabbitMQ.Publisher;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ysynpygm:vY1Dp4qZ437uU3Dojt0nnY9-FFKb8H8G@toad.rmq.cloudamqp.com/ysynpygm");
using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

// fanout da publisher kuyruk deklere etmeyecek, consumer artık açacak
//channel.QueueDeclare(queue:"test-1",durable:true,exclusive:false,autoDelete:false);
string exhangeName = "log-direct";
// exchange deklera ediyoruz şimdi
// exchange: excahnge ismi
// type    : exchange tipini seçiyoruz
// durable : true ise fiziksel olarak kaydedilsin işlem 
// autodel :
// args    :
channel.ExchangeDeclare(exchange: exhangeName, type: ExchangeType.Direct, durable: false, autoDelete: true, arguments: null);

// var olan tüm log type lar için otomatik olarak kuyruk açıyoruz
Enum.GetNames(typeof(LogTypes)).ToList().ForEach(x =>
{
    var routeKey = $"route-{x}";
    var logQueue = $"{x}-log";
    channel.QueueDeclare(queue: logQueue, durable: true, exclusive: false, autoDelete: false);
    // queue kuyruk ismi
    // exchange exchange ismi
    // routingKey yönlendirme ismi 
    channel.QueueBind(queue: logQueue, exchange: exhangeName, routingKey: routeKey, arguments: null);
});

// kuruğa mesaj gönderimi
Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    LogTypes log = (LogTypes)new Random().Next(1,5);
    string message = $"Log-Type : {log}";
    var messageBody = Encoding.UTF8.GetBytes(message);
    var routeKey = $"route-{log}";
    // exchange : burada ismini veriyoruz
    // routingKey : bu sefer bir kuyruk ismi vermiyoruz fanout da
    channel.BasicPublish(exchange: exhangeName, routingKey: routeKey, basicProperties: null, body: messageBody);
    Console.WriteLine($"Gönderilen mesaj :{message}");
});

Console.ReadLine();