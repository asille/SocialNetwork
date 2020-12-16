using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CassandraDAL.Interfaces
{
    interface IUserStream
    {
        void SyncUserStream(ISession session, Guid postId, TimeUuid updatedAtPrev);
    }
}
