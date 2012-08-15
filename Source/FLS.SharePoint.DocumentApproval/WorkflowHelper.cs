namespace DocumentApproval
{
   public static class WorkflowHelper
    {
       public static string ConvertFieldToString(object fieldValue)
       {
           return fieldValue != null ?
                  fieldValue.ToString() :
                  string.Empty;
       }
    }
}
