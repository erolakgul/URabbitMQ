
using RabbitMQ.Client;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using URabbitMQ.Publisher;
using URabbitMQ.Shared.Dtos;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ysynpygm:vY1Dp4qZ437uU3Dojt0nnY9-FFKb8H8G@toad.rmq.cloudamqp.com/ysynpygm");
using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

string exhangeName = "exhange-header";
channel.ExchangeDeclare(exchange: exhangeName, type: ExchangeType.Headers, durable: false, autoDelete: true, arguments: null);

// header type ları belirtiliyor
Dictionary<string, object> keyValuePairs = new();
keyValuePairs.Add("format","pdf");
keyValuePairs.Add("shape","a4");

// channel üzerinde properties ler belirtiliyor
var properties = channel.CreateBasicProperties();
properties.Headers = keyValuePairs;

/// mesajların kalıcı hale gelmesi için Persistent property si true olarak setlenmeli
properties.Persistent = true;

#region complex type gönderim
var product = new Product() { ID = 1, Name = "Pen", Price = 50, AvailableStock = 70 };
var productJsonString = JsonSerializer.Serialize(product);
var msg = Encoding.UTF8.GetBytes(productJsonString); 
#endregion

channel.BasicPublish(exchange: exhangeName, routingKey: string.Empty,mandatory:false, basicProperties: properties, body: msg);

Console.WriteLine("Mesaj Gönderildi.");

Console.ReadLine();