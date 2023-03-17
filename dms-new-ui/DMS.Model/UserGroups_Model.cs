using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Model
{
   public class UserGroups_Model
    {
       public class UserGroups
       {
           public List<menu> menu { get; set; }
       }
       public partial class menu
       {
           public int menu_gid { get; set; }
           public string menu_name { get; set; }
           public string menu_url { get; set; }
           public int parent_menu_gid { get; set; }
           public int menu_order { get; set; }
           public bool rights_flag { get; set; }
           public string icon_path { get; set; }
       }
       public class UserEntities
       {
           public int user_gid { get; set; }
           public string user_code { get; set; }
           public string user_name { get; set; }
           public int usergroup_gid { get; set; }
           public string user_status { get; set; }
           public DateTime insert_date { get; set; }
           public string insert_by { get; set; }
           public DateTime update_date { get; set; }
           public string update_by { get; set; }
           public string delete_flag { get; set; }
           public string usergroup_name { get; set; }
           public string usergroup_code { get; set; }
           public int result { get; set; }
           public string msg { get; set; }           
           public int org_gid { get; set; }
           public string org_code { get; set; }

       }

       public class RoleMappings
       {
           public int rolemenu_rowid { get; set; }
           public int menu_gid { get; set; }
           public string menu_name { get; set; }
           public string add_perm { get; set; }
           public string mod_perm { get; set; }
           public string view_perm { get; set; }
           public string auth_perm { get; set; }
           public string inactivate_perm { get; set; }
           public string print_perm { get; set; }
           public string deny_flag { get; set; }
           public string role_code { get; set; }
       }
    }
}
