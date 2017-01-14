using CheckList.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CheckList.API.Repository
{
    public class CheckListRepository : RepositoryBase
    {
        public CheckListRepository() : base(System.Configuration.ConfigurationManager.ConnectionStrings["CheckListDB"].ConnectionString) { }

        public List<List> GetList() {
            using (var cmd = new SqlCommand("List_GetList", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var result = SqlHelper.ExecuteCommandAsType<List>(cmd);
                return result;
            }
        }
    }
}