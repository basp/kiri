# kiri
This is a small toolkit to create IRC bots.

# getting started
The first thing we need is a `Client<T>` instance. The `T` in this type represents the user data for your particular bot application. We'll take a look at this later. For now, note that `T` needs to be a reference type. And with that in mind we can create a class to hold our state.
```
class Session
{    
}
```

That's just an empty class. We don't need anything else for now. However, we can now create a client instance:
```
var session = new Session():
var client = Client.Create(session);
```

The `Session` will act as a container for all the state we need to keep during the lifetime of our connection. It's ideomatic and recommended to add typed properties to your particular version of `Session` as you are building your application.

Let's assume the **host** we are trying to connect to is `chat.freenode.net` and the **port** is `6667` (we can usually obtain this information from the host's website). Now we can connect:
```
client.Connect("chat.freenode.net", 6667);
```

However, we need to keep `client` alive since it's listening on another thread. This is also a nice way to somewhat control our client since we can basically just keep reading commands and sending them to the server:
```
while(true)
{
    var cmd = Console.ReadLine();
    client.Send(cmd);
}
```

Our final program code looks like the following:
```
class Session
{    
}

class Program
{
    public static void Main(string[] args)
    {
        var session = new Session():
        var client = Client.Create(session);
   
        client.Connect("chat.freenode.net", 6667);
   
        while(true)
        {
            var cmd = Console.ReadLine();
            client.Send(cmd);
        }
    }
}
```

This won't do much though. In fact, it's highly likely you'll be kicked from the server after a short while due to not responding to `IDENT` or `PING` messages. It's easy to fix this as both `IDENT` and `PING` are supported by *middleware* out of the box:
```
client
    .WithPong()
    .WithIdentity()
    .Connect("chat.freenode.net", 6667);
```

However, this will lead to a compile error since our `Session` class doesn't implement the `IIdentityProvider` interface. That's an easy fix though:
```
class Session : IIdentityProvider
{
    private readonly string nick;

    public Session(string nick)
    {
        this.nick = nick;
    }

    public string Nick => this.nick;
    public string Info => "http://github.com/basp/kiri";
    public string Aliases => new [] { this.nick };
}
```

So now that our `Session` class implements `IIdentityProvider` things should compile just fine and also the middleware should kick in as well. Now your client will be able to stay alive and you'll be able to send IRC commands using the `Console.ReadLine` call.

For example you can `JOIN <channel_name>` or `PRIVMSG <channel_name> :<msg>` but of course the bot supports these operations as well. The interactive mode is useful for *out of band* commands though.