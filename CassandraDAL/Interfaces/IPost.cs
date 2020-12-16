using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CassandraDAL.Interfaces
{
    interface IPost
    {
        void CreatePost(ISession session);
        void UpdatePost(ISession session);
    }
}
