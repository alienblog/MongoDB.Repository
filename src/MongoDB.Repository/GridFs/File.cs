using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Repository.GridFs
{
    /// <summary>
    ///     文件类
    /// </summary>
    public class File
    {
        [BsonIgnore] private byte[] _data;

        /// <summary>
        ///     文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     文件所在GridFs的ObjectId
        /// </summary>
        public ObjectId FileObjectId { get; set; }

        /// <summary>
        ///     文件所在GridFs的Id
        /// </summary>
        public string Id
        {
            get { return FileObjectId.ToString(); }
            set { FileObjectId = ObjectId.Parse(value); }
        }

        /// <summary>
        ///     文件数据
        /// </summary>
        [BsonIgnore]
        public byte[] Data
        {
            get
            {
                if (_data == null)
                {
                    if (string.IsNullOrEmpty(Id)) return new byte[0];
                    var repository = new GridfsRepository();
                    _data = repository.Find(Id).Data;
                }
                return _data;
            }
            set { _data = value; }
        }

        /// <summary>
        ///     文件MD5码
        /// </summary>
        public string MD5 { get; set; }

        /// <summary>
        ///     保存文件
        /// </summary>
        public void Save()
        {
            var repository = new GridfsRepository();
            repository.Create(this);
        }

        /// <summary>
        ///     删除文件
        /// </summary>
        public void Remove()
        {
            if (string.IsNullOrEmpty(Id)) return;
            var repository = new GridfsRepository();
            repository.Delete(Id);
        }
    }
}