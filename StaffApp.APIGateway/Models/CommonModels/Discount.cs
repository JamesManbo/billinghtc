namespace StaffApp.APIGateway.Models.CommonModels
{
    public class Discount
    {
        public float Percent { get; set; }
        public decimal Amount { get; set; }
        public bool Type { get; set; } // true: percent, false: amount

        protected Discount()
        {
        }

        public Discount(float percent = 0, bool type = true)
        {
            Percent = percent;
            Type = type;
        }
    }
}