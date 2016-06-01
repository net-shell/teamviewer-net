using System.Collections.Generic;

namespace TeamViewerNet.Models.Response
{
    public class GetUsersResponse : BaseResponseModel
    {
        public List<GetUserResponse> users { get; set; }
    }
}
