namespace RMQLib.Messages.Email
{
    public class BaseEmailMessage
    {
        public string SmtpHost { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public string UserName { get; set; } = "tmargraff@gmail.com";
        public string Password { get; set; } = "S@pphire5211";

        public BaseEmailMessage()
        {
        }
    }

}
