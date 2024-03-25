
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
// https://customer.cloudamqp.com/instance daki instance ın deayında AMQP details url bilgisi
factory.Uri = new Uri("amqps://ysynpygm:vY1Dp4qZ437uU3Dojt0nnY9-FFKb8H8G@toad.rmq.cloudamqp.com/ysynpygm");

// bağlantı açıyoruz
using var connection = factory.CreateConnection();

// bağlantıyı açtıktan sonra da kanal açıyoruz.
var channel = connection.CreateModel();

// kuyruk ismi
// durable false olursa memory de, true olursa rabbit de tutulur
// exclusive true olursa, sadece burada oluşmuş kanal üzerinden bağlanılabilir, false olursa subscriberlar bağlanalbilir
// autoDelete true olursa, subscriber down olursa bağlantısı koparsa mesaj silinir, silinsin istemiyıorsak false
channel.QueueDeclare(queue:"test-1",durable:true,exclusive:false,autoDelete:false);

// RabbitMQ üzerinden tüm veriler byte dizisi şeklinde gönderilir

string message = "Hello World";
var messageBody = Encoding.UTF8.GetBytes(message);

// exchange kullanılıyorsa exchange bilgisi
// routingKey mesajın iletileceği kuyruk ismi
// basicProperties
// body byte olarak tanımlı gönderilecek olan bilgi
channel.BasicPublish(exchange: string.Empty, routingKey: "test-1", basicProperties: null,body:messageBody);

Console.WriteLine($"Gönderilen mesaj :{message}");

Console.ReadLine();