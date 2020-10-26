using MessageBird;
using MessageBird.Exceptions;
using MessageBird.Objects;
using MessageBird.Objects.Conversations;
using System;

namespace MessageBirdWhatsAppTemplatedMessage
{
    class Program
    {
        const bool _useWhatsAppSandbox = true;
        const string YourAccessKey = "YOUR_ACCESS_KEY"; // your access key here.
        const string ChannelId = "YOUR_CHANNEL_ID"; // your channel id here
        const string To = "RECEIVER_PHONE_NUMBER"; // receiver phone number here
        const string HsmNamespace = "HSM_NAMESPACE"; // HSM namespace here
        const string TemplateName = "TEMPLATE_NAME"; // template name here
        const string HsmLanguageCode = "HSM_LANGUAGE_CODE"; // HSM language code


        static void Main(string[] args)
        {
            Client _client;

            if (_useWhatsAppSandbox)
            {
                _client = Client.CreateDefault(YourAccessKey, features: new Client.Features[] { Client.Features.EnableWhatsAppSandboxConversations });
            }
            else
            {
                _client = Client.CreateDefault(YourAccessKey);
            }

            try
            {
                Conversation conversation = _client.StartConversation(new ConversationStartRequest()
                {
                    ChannelId = ChannelId,
                    To = To,
                    Type = MessageBird.Objects.Conversations.ContentType.Hsm,
                    Content = new Content()
                    {
                        Hsm = new HsmContent()
                        {
                            Namespace = HsmNamespace,
                            TemplateName = TemplateName,
                            Language = new HsmLanguage()
                            {
                                Code = HsmLanguageCode,
                                Policy = HsmLanguagePolicy.Deterministic
                            },
                            Params = new System.Collections.Generic.List<HsmLocalizableParameter>()
                            {
                                new HsmLocalizableParameter(){Default = "Roberto"},
                                new HsmLocalizableParameter(){Default = "123"},
                                new HsmLocalizableParameter(){Default = "new coffee machine"},
                                new HsmLocalizableParameter(){Default = "MessageBird, Trompenburgstraat 2C, 1079TX Amsterdam"},
                            }
                        }
                    }
                });
                Console.WriteLine("{0}", conversation);
            }
            catch (ErrorException e)
            {
                // Either the request fails with error descriptions from the endpoint.
                if (e.HasErrors)
                {
                    foreach (Error error in e.Errors)
                    {
                        Console.WriteLine("code: {0} description: '{1}' parameter: '{2}'", error.Code, error.Description, error.Parameter);
                    }
                }
                // or fails without error information from the endpoint, in which case the reason contains a 'best effort' description.
                if (e.HasReason)
                {
                    Console.WriteLine(e.Reason);
                }
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

        }

    }
}
