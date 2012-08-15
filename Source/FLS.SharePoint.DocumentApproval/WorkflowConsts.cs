namespace DocumentApproval
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WorkflowConsts
    {
        // workflow task fields
        public const string TaskAssignedTo = "AssignedTo";
        public const string TaskDueDate = "DueDate";
        public const string TaskStatus = "DocumentStatus";

        // document statuses
        public const string DocumentCreated = "Created";
        public const string DocumentOnAdvisement = "On advisement";
        public const string DocumentApproved = "Approved";
        public const string DocumentRejected = "Rejected";

        // workflow responsible users
        public const string TaskAssignedUser = "SHAREPOINTSRV01\\localhr";

        // workflow fields
        public const string WorkflowName = "Имя";
        public const string WorkflowStepConsolidatedNote = "ConsolidatedComment";
    }
}
