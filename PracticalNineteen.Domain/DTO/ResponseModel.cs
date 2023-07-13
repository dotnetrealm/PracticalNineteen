namespace PracticalNineteen.Domain.DTO
{
    public class ResponseModel
    {
        public string Message { get; set; }
        public string Data { get; set; }

        public UserInformation UserInfo { get; set; }  
    }
}
