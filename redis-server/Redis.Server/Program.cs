using System.Net;
using System.Net.Sockets;
using System.Text;
using Redis.Server;

TcpListener server = null;
try
{
    Int32 port = 6379;
    IPAddress localAddr = IPAddress.Parse("127.0.0.1");
    
    server = new TcpListener(localAddr, port);
    server.Start();

    Byte[] bytes = new Byte[256];
    String data = null;

    var service = new CommandService();

    while(true)
    {
        Console.Write("Waiting for a connection... ");

        // Perform a blocking call to accept requests.
        // You could also use server.AcceptSocket() here.
        using TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Connected!");

        data = null;

        // Get a stream object for reading and writing
        NetworkStream stream = client.GetStream();

        int i;

        // Loop to receive all the data sent by the client.
        while((i = stream.Read(bytes, 0, bytes.Length))!=0)
        {
            // Translate data bytes to a ASCII string.
            data = Encoding.ASCII.GetString(bytes, 0, i);
            Console.WriteLine("Received: {0}", data);
            

            // Process the data sent by the client.
            data = data.ToUpper();
            var result = service.ProcessCommand(data);

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(result.Serialize());

            // Send back a response.
            stream.Write(msg, 0, msg.Length);
            Console.WriteLine("Sent: {0}", data);
        }
    }
}
catch(SocketException e)
{
    Console.WriteLine("SocketException: {0}", e);
}
finally
{
    server.Stop();
}

Console.WriteLine("\nHit enter to continue...");
Console.Read();
  