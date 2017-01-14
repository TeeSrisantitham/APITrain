using CheckList.API.Models;
using CheckList.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CheckList.API.Controllers
{
    public class ValuesController : ApiController
    {
        CheckListRepository checkListRepo = new CheckListRepository();
        // GET api/values
        public IHttpActionResult Get()
        {
            List<List> result = checkListRepo.GetList();
            return Json(new
              {
                  success = true,
                  data = result
              });
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
