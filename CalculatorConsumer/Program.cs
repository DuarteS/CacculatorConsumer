using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.EntityFrameworkCore;
using CalculatorApi.Models;
using CalculatorApi.Data;

namespace CalculatorConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[Consumer] Listening for messages...");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "calculation",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[Consumer] Received: {message}");

                var calculation = JsonSerializer.Deserialize<Calculation>(message);
                if (calculation != null)
                {
                    calculation.Result = EvaluateExpression(calculation.Expression);
                    SaveToDatabase(calculation);
                }

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: "calculation",
                                 autoAck: false,
                                 consumer: consumer);

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }

        static double EvaluateExpression(string expression)
        {
            try
            {
                return Convert.ToDouble(new System.Data.DataTable().Compute(expression, null));
            }
            catch
            {
                Console.WriteLine("[Consumer] Error evaluating expression.");
                return double.NaN;
            }
        }

        static void SaveToDatabase(Calculation calculation)
        {
            using var db = new AppDbContext();
            db.Database.EnsureCreated();
            db.Calculations.Add(calculation);
            db.SaveChanges();
            Console.WriteLine($"[Consumer] Saved to database: {calculation.Expression} = {calculation.Result}");
        }
    }
}
