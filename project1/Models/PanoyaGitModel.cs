namespace project1.Models
{
    public class PanoyaGitModel
    {
        public IEnumerable<TaskModel> CompletedTasks { get; set; }
        public IEnumerable<TaskModel> OngoingTasks { get; set; }
    }

}