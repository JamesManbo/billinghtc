namespace StaffApp.APIGateway.Models.CustomerModels
{
    public class CustomerClassDtoGrpc
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public int ConditionStartPoint { get; set; }
        public int ConditionEndPoint { get; set; }
    }
}
