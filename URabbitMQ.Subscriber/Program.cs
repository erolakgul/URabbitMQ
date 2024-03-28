using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ysynpygm:vY1Dp4qZ437uU3Dojt0nnY9-FFKb8H8G@toad.rmq.cloudamqp.com/ysynpygm");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

// exchange ismi
string exhangeName = "exhange-header";
// random kuyruk ismi alınır, birden fazla instance çalıştırılacak
var queueName = channel.QueueDeclare().QueueName;

// dinlemek istediğimiz exchange in header ını belrtiyoruz
Dictionary<string, object> keyValuePairs = new();
keyValuePairs.Add("format", "pdf");
keyValuePairs.Add("shape", "a4");
keyValuePairs.Add("x-match", "all");

// bind edeceğiz, yani subs düştüğünde kuyruk da otomatik olarak dşsün istioyrz.
// deklare edersek subs kapansa bile kuyruk kalmaya devam eder
channel.QueueBind(queue: queueName, exchange: exhangeName, routingKey: "", arguments: keyValuePairs);

var consumer = new EventingBasicConsumer(channel);

// autoAck parametresi false a set edildi. haberdar edildiğinde kuyruktaki mesaj silinsin isteniyor artık
channel.BasicConsume(queue: queueName, autoAck: false,consumer:consumer);

Console.WriteLine($"{queueName} - Loglar dinleniyor...");

consumer.Received +=
    (sender, args) =>
    {
        var msg = Encoding.UTF8.GetString(args.Body.ToArray());
        Console.WriteLine($"Gelen Mesaj : {msg}");
        Thread.Sleep(1500);

        //deliveryTag silinecek olan tag ismi
        //multiple true ise memory de işlenmemiş ama rabibitmq ya gitmemiş tag leri de sil
        // false olursa sadece ilgili mesajın durumunu rabbit e bildir
        channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
    };
Console.ReadLine();