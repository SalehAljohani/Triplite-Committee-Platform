using Triplite_Committee_Platform.Models;

namespace Triplite_Committee_Platform.ViewModels
{
    public class BoardDetailsViewModel
    {
        public int? Id { get; set; }
        public int? ReqTypeID { get; set; }
        public RequestTypeModel? RequestType { get; set; }
        public ReasonsModel? Reason { get; set; }
        public ScholarshipModel? Scholarship { get; set; }
        public BoardModel Board { get; set; }
        public List<UserModel>? Users { get; set; }
    }
}
