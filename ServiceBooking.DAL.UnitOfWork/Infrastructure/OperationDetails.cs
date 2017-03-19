namespace ServiceBooking.BLL.Infrastructure
{
    public class OperationDetails
    {
        public OperationDetails(bool succedeed, string message, string prop)
        {
            Succedeed = succedeed;
            Message = message;
            Property = prop;
        }

        public bool Succedeed { get; set; }

        public string Message { get; set; }

        public string Property { get; set; }
    }
}
