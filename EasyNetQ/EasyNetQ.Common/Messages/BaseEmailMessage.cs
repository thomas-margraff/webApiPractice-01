namespace EasyNetQ.Common.Messages
{
    public class BaseEmailMessage: BaseMessage
    {
        public string SmtpHost { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public string UserName { get; set; } = "tmargraff@gmail.com";
        public string Password { get; set; } = "Sapphire5211";

        public BaseEmailMessage():
            base()
        {
        }
    }

}
