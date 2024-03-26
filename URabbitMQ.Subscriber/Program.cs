using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ysynpygm:vY1Dp4qZ437uU3Dojt0nnY9-FFKb8H8G@toad.rmq.cloudamqp.com/ysynpygm");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

// excahnge i burada da tanımlayabiliriz, yoksa insert eder hataya düşmez
//channel.ExchangeDeclare(exchange: "log-fanout", type: ExchangeType.Fanout, durable: false, autoDelete: true, arguments: null);

// rastgele bir şekilde kuyruk ismi alınır
var randomQueueName = channel.QueueDeclare().QueueName;

// bu şekilde tanımlarsak kuyruk oluşur ve silinmez channel.QueueDeclare(randomQueueName,true,true,false); 
// QueueBind ile uygulama her ayağa kalktığında kuyruk oluşan, kapandıığında ise silinecek
channel.QueueBind(queue: randomQueueName, exchange: "log-fanout", routingKey: "", arguments: null);

// prefetchSize : 0 olursa herhangi bir size daki bilgiyi alabilir demek
// prefetchCount: alınacak bilgi sayısı
// global       : false olursa prefetchCount bilgisi ne ise tüm alıcılara o kadar bilgi gider
// true ise, prefetchCount bilgisi / alıcı sayısı olacak şekilde her bir alıcıya o kadar bilgi gider
// örneğin count 6, alıcı sayısı da 2 ise her bir alıcıya 3 veri gider
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

var consumer = new EventingBasicConsumer(channel);
// kuyruk ismi randomQueueName ile değiştirildi
// autoAck parametresi false a set edildi. haberdar edildiğinde kuyruktaki mesaj silinsin isteniyor artık
channel.BasicConsume(queue: randomQueueName, autoAck: false,consumer:consumer);

Console.WriteLine($"{randomQueueName} - Loglar dinleniyor...");

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