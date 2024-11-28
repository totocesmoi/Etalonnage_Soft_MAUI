using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Newtonsoft.Json;

namespace DAL.Stub
{
    public class StubbedPostTraitement : IPostTraitementService<PostTraitement>
    {
        static StubbedPostTraitement()
        {

        }

        public Task<bool> CreateAsyncPostTraitement(PostTraitement postTraitement)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StopExcutionAsync(PostTraitement postTraitement)
        {
            throw new NotImplementedException();
        }

        public Task<PostTraitement> TestExcutionAsync(PostTraitement postTraitement)
        {
            throw new NotImplementedException();
        }

        public static string ToJson(PostTraitement postTraitement)
        {
            return JsonConvert.SerializeObject(postTraitement);
        }

        public static PostTraitement? FromJson(string json)
        {
            return JsonConvert.DeserializeObject<PostTraitement>(json);
        }
    }
}
