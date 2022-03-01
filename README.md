# Subscriber Example for RS Lead Sharing

SNS API endpoint example for subscribing to a topic and processing the message payload

## Requirements

- .NET 6.0
  - Nuget packages
    - [AWSSDK.SimpleNotificationService](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/SNS/NSNS.html)
    - Microsoft.EntityFrameworkCore (Demo purposes)
    - Microsoft.EntityFrameworkCore.InMemory (Demo purposes)

## Installation

### Terminal

```properties
dotnet restore
dotnet run
```

### Visual Studio

Opening solution file will restore all nuget packages and give the ability to run it from IDE.

## API

`POST https://<DOMAIN>/api/Lead`

### Confirmating Subscription
When the subscription is created, it will send a confirmation request with the following header and body.
This request will only happen once

```http
x-amz-sns-message-type: SubscriptionConfirmation
x-amz-sns-message-id: <string>
x-amz-sns-topic-arn: <TopicName:string>
Content-Length: <number>
Content-Type: text/plain; charset=UTF-8
Host: myhost.example.com
Connection: Keep-Alive
User-Agent: Amazon Simple Notification Service Agent

{
  "Type" : "SubscriptionConfirmation"
  "MessageId" : "165545c9-2a5c-472c-8df2-7ff2be2b3b1b",
  "Token" : "2336412f37...",
  "TopicArn" : "arn:aws:sns:us-west-2:123456789012:MyTopic",
  "Message" : "You have chosen to subscribe to the topic arn:aws:sns:us-west-2:123456789012:MyTopic.\nTo confirm the subscription, visit the SubscribeURL included in this message.",
  "SubscribeURL" : "https://sns.us-west-2.amazonaws.com/?Action=ConfirmSubscription&TopicArn=arn:aws:sns:us-west-2:123456789012:MyTopic&Token=2336412f37...",
  "Timestamp" : "2012-04-26T20:45:04.751Z",
  "SignatureVersion" : "1",
  "Signature" : "EXAMPLEpH+DcEwjAPg8O9mY8dReBSwksfg2S7WKQcikcNKWLQjwu6A4VbeS0QHVCkhRS7fUQvi2egU3N858fiTDN6bkkOxYDVrY0Ad8L10Hs3zH81mtnPk5uvvolIC1CXGu43obcgFxeL3khZl8IKvO61GWB6jI9b5+gLPoBc1Q=",
  "SigningCertURL" : "https://sns.us-west-2.amazonaws.com/SimpleNotificationService-f3ecfb7224c7233fe7bb5f59f96de52f.pem"
}
```

With the help of `AWSSDK.SimpleNotificationService` we can validate parse the payload to a [Amazon.SimpleNotificationService.Util.Message](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/SNS/TMessage.html) Object

The message can be parsed using the following

```csharp
 Amazon.SimpleNotificationService.Util.Message SNSParsedMessage =
    Amazon.SimpleNotificationService.Util.Message.ParseMessage(rawStringPayload);
```

Once parsed the message signature can be verified with `IsMessageSignatureValid`
```csharp
if (!SNSParsedMessage.IsMessageSignatureValid())
{
    return Unauthorized("Signature not valid");
}
```

To confirm the subscription, its required to call `SubscribeURL`

```csharp
if (SNSParsedMessage.IsSubscriptionType)
{
    string SNSSubscribeUrl = SNSParsedMessage.SubscribeURL;
    var httpClient = _httpClientFactory.CreateClient();
    await httpClient.GetAsync(SNSSubscribeUrl);
    return NoContent();
}
```


### Processing Notification

Once the endpoint has been confirmed, all following request will use the following format
```http
x-amz-sns-message-type: Notification
x-amz-sns-message-id: 22b80b92-fdea-4c2c-8f9d-bdfb0c7bf324
x-amz-sns-topic-arn: arn:aws:sns:us-west-2:123456789012:MyTopic
x-amz-sns-subscription-arn: arn:aws:sns:us-west-2:123456789012:MyTopic:c9135db0-26c4-47ec-8998-413945fb5a96
Content-Length: 773
Content-Type: text/plain; charset=UTF-8
Host: myhost.example.com
Connection: Keep-Alive
User-Agent: Amazon Simple Notification Service Agent

{
  "Type" : "Notification",
  "MessageId" : "22b80b92-fdea-4c2c-8f9d-bdfb0c7bf324",
  "TopicArn" : "arn:aws:sns:us-west-2:123456789012:MyTopic",
  "Subject" : "My First Message",
  "Message" : "Hello world!",
  "Timestamp" : "2012-05-02T00:54:06.655Z",
  "SignatureVersion" : "1",
  "Signature" : "EXAMPLEw6JRN...",
  "SigningCertURL" : "https://sns.us-west-2.amazonaws.com/SimpleNotificationService-f3ecfb7224c7233fe7bb5f59f96de52f.pem",
  "UnsubscribeURL" : "https://sns.us-west-2.amazonaws.com/?Action=Unsubscribe&SubscriptionArn=arn:aws:sns:us-west-2:123456789012:MyTopic:c9135db0-26c4-47ec-8998-413945fb5a96"
}
```

The same function `ParseMessage` and `IsMessageSignatureValid` can be used.
`IsSubscriptionType` will return false.

The notification payload will be a JSON stringified in `MessageText` property.

```csharp
SubscriptionPayload message =
    JsonConvert.DeserializeObject<SubscriptionPayload>(SNSParsedMessage.MessageText);
```


## Models

### [SubscriptionPayload](Models/SubscriptionPayload.cs)
```csharp
public class SubscriptionPayload
{
    public Lead lead { get; set; }
    public string callback { get; set; }
    public string token { get; set; }
}
```
### [Lead](Models/Lead.cs)
```csharp
public class Lead
{
    public int id { get; set; }
    public Patient patient { get; set; }
    public Treatment treatment { get; set; }
}
```
### [Patient](Models/Patient.cs)
```csharp
public class Patient
{
    public string id { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public string preferred_contact_method { get; set; }
}
```
### [Treatment](Models/Treatment.cs)
```csharp
public class Treatment
{
    public string id { get; set; }
    public string name { get; set; }
}
```



## License
[MIT](LICENCE.md)