using System;
using System.Collections.Generic;
using System.Text;

namespace SignUp.Messaging.Constants
{
    public static class RabbitMqConstants
    {
        public const string RabbitMqUri =
            "amqp://guest:guest@127.0.0.1:5672/";
        public const string JsonMimeType =
            "application/json";

        public const string RegisterExchange =
            "signup.management.register.exchange";
        public const string RegisterQueue =
            "signup.management.register.queue";

    }
}
