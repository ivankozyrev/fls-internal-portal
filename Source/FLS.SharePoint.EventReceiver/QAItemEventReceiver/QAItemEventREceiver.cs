using Microsoft.SharePoint;

namespace FLS.SharePoint.EventReceiver.QAItemEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class QAItemEventReceiver : SPItemEventReceiver
    {
       /// <summary>
       /// An item was added.
       /// </summary>
       public override void ItemAdded(SPItemEventProperties properties)
       {
           if (properties.List.Items.Count > 2)
           {
               var firstItem = properties.List.Items[0];
               firstItem["Имя"] = "First item name after deleting";
               firstItem.Update();
               properties.List.Items[1].Delete();
               properties.List.Update();
           }
       }

       /// <summary>
       /// An item was deleted.
       /// </summary>
       public override void ItemDeleted(SPItemEventProperties properties)
       {
           if (properties.List.Items.Count > 0)
           {
               var firstItem = properties.List.Items[0];
               firstItem["Имя"] = "One item was deleted";
               firstItem.Update();
           }
       }

       public override void ItemUpdating(SPItemEventProperties properties)
       {
          if (properties.List.Items.Count > 1)
          {
              SPListItem firstItem = null;

              foreach (var item in properties.List.Items)
              {
                  if (!item.Equals(properties.ListItem))
                  {
                      firstItem = item as SPListItem;
                      break;
                  }
              }

              if (firstItem != null)
              {
                  firstItem["Имя"] = "One item will be updated";
                  firstItem.Update();
              }
          }
       }
    }
}
