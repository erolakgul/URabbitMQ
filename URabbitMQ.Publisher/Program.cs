
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ysynpygm:vY1Dp4qZ437uU3Dojt0nnY9-FFKb8H8G@toad.rmq.cloudamqp.com/ysynpygm");
using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

// fanout da publisher kuyruk deklere etmeyecek, consumer artık açacak
//channel.QueueDeclare(queue:"test-1",durable:true,exclusive:false,autoDelete:false);

// exchange deklera ediyoruz şimdi
// exchange: excahnge ismi
// type    : exchange tipini seçiyoruz
// durable : true ise fiziksel olarak kaydedilsin işlem 
// autodel :
// args    :
channel.ExchangeDeclare(exchange: "log-fanout", type: ExchangeType.Fanout, durable: false, autoDelete: true, arguments: null);

// kuruğa mesaj gönderimi
Enumerable.Range(1, 50).ToList().ForEach( x=> 
{
    string message = $"Message {x}";
    var messageBody = Encoding.UTF8.GetBytes(message);
    // exchange : burada ismini veriyoruz
    // routingKey : bu sefer bir kuyruk ismi vermiyoruz fanout da
    channel.BasicPublish(exchange: "log-fanout", routingKey: "", basicProperties: null, body: messageBody);
    Console.WriteLine($"Gönderilen mesaj :{message}");
});

Console.ReadLine();