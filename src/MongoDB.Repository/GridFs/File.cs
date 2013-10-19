using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Repository.GridFs
{
    public class File
    {
        [BsonIgnore]
        private byte[] _data;

        public string FileName { get; set; }

        public ObjectId FileObjectId { get; set; }

        public string Id
        {
            get
            {
                return FileObjectId.ToString();
            }
            set
            {
                FileObjectId = ObjectId.Parse(value);
            }
        }

        [BsonIgnore]
        public byte[] Data
        {
            get
            {
                if (_data == null)
                {
                    if(string.IsNullOrEmpty(Id))return new byte[0];
                    var repository = new GridfsRepository();
                    _data = repository.Find(Id).Data;
                }
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public string MD5 { get; set; }

        public void Save()
        {
            var repository = new GridfsRepository();
            repository.Create(this);
        }

        public void Remove()
        {
            if (string.IsNullOrEmpty(Id)) return;
            var repository = new GridfsRepository();
            repository.Delete(Id);
        }
    }
}
