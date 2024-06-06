using OPC.Models;

var randomValue = DataGenerator.GenerateRandomNumber(1, 100);
Console.WriteLine($"Случайно сгенерированное число {randomValue}");

var opcDataSender = new OpcDataSender("opc.tcp://localhost:4840");
opcDataSender.SendData("ns=2;s=Demo.Dynamic.Scalar.Double", randomValue);
opcDataSender.Disconnect();