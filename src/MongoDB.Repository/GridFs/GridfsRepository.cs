using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;

namespace MongoDB.Repository.GridFs
{
    public class GridfsRepository
    {
        public File Find(string id)
        {
            var file = new File();
            using (var client = DBFactory.GetClient())
            {
                var oid = new ObjectId(id);
                var fsInfo = client.GridFs.FindOne(Query.EQ("_id", oid));

                var stream = fsInfo.OpenRead();
                file.Data = new byte[stream.Length];
                stream.Read(file.Data, 0, file.Data.Length);

                file.FileName = fsInfo.Name;
                file.Id = fsInfo.Id.ToString();
                file.MD5 = fsInfo.MD5;

                return file;
            }
        }

        public string Create(File file)
        {
            using (var client = DBFactory.GetClient())
            {
                var fs = client.GridFs;
                var ms = new System.IO.MemoryStream(file.Data);

                var fsInfo = fs.Upload(ms, file.FileName);

                file.Id = fsInfo.Id.ToString();
                file.MD5 = fsInfo.MD5;

                return file.FileObjectId.ToString();
            }
        }

        public void Delete(string id)
        {
            using (var client = DBFactory.GetClient())
            {
                var fs = client.GridFs;

                fs.DeleteById(id);
            }
        }

        public void Delete(File file)
        {
            Delete(file.Id);
        }
    }
}
