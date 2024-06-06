using Opc.Ua;
using Opc.Ua.Client;

namespace OPC.Models;

public class OpcDataSender
{
    private Session _session;

    public OpcDataSender(string endpointUrl)
    {
        var config = new ApplicationConfiguration()
        {
            ApplicationName = "OpcDataSender",
            ApplicationType = ApplicationType.Client,
            SecurityConfiguration = new SecurityConfiguration
            {
                ApplicationCertificate = new CertificateIdentifier()
            },
            ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 }
        };

        config.Validate(ApplicationType.Client).GetAwaiter().GetResult();
        config.CertificateValidator.CertificateValidation += (s, e) => e.Accept = (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted);

        var endpoint = CoreClientUtils.SelectEndpoint(endpointUrl, useSecurity: false);
        var endpointConfig = EndpointConfiguration.Create(config);
        var endpointDesc = new ConfiguredEndpoint(null, endpoint, endpointConfig);

        _session = Session.Create(config, endpointDesc, false, "", 60000, null, null).GetAwaiter().GetResult();
    }

    public void SendData(string nodeId, double value)
    {
        var nodeToWrite = new WriteValue
        {
            NodeId = new NodeId(nodeId),
            AttributeId = Attributes.Value,
            Value = new DataValue
            {
                Value = value,
                StatusCode = StatusCodes.Good,
                SourceTimestamp = DateTime.UtcNow
            }
        };

        _session.Write(null, new WriteValueCollection { nodeToWrite }, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos);

        if (StatusCode.IsGood(results[0]))
        {
            Console.WriteLine("OPC Data sent successfully!");
        }
        else
        {
            Console.WriteLine($"Error sending OPC data: {results[0]}");
        }
    }

    public void Disconnect()
    {
        _session.Close();
        _session.Dispose();
    }
}