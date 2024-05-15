using Triplite_Committee_Platform.Models;

namespace Triplite_Committee_Platform.ViewModels
{
    public class StatsViewModel
    {
        public string? College { get; set; }
        public List<DepartmentStatsViewModel> Departments { get; set; }
        public string? DeptName { get; set; }
        public int TotalBoard { get; set; }
        public int CompletedBoards { get; set; }
        public int CurrentBoards { get; set; }
    }
}
