using System;
using System.Collections.Generic;
using System.Text;

namespace SignUp.Messaging.Constants
{
    public static class RabbitMqConstants
    {
        public const string RabbitMqUri =
            "amqp://user:password@signup.rabbit:5672/";
        public const string JsonMimeType =
            "application/json";

        public const string RegisterExchange =
            "signup.management.register.exchange";
        public const string RegisterQueue =
            "signup.management.register.queue";

    }
}
