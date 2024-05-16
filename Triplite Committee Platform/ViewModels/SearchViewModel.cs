using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using Triplite_Committee_Platform.Models;

namespace Triplite_Committee_Platform.ViewModels
{
    public class SearchViewModel
    {
        public string? Search { get; set; }
        public int? SelectedCollege { get; set; }
        public int? SelectedDepartment { get; set; }
        public string? CollegeName { get; set; }
        public string? DeptName { get; set; }
        public List<BoardModel>? Boards { get; set; }
        public string? Name { get; set; }
        public string? RequestType { get; set; }
        public RequestTypeModel RequestStatus { get; set; }
    }
}