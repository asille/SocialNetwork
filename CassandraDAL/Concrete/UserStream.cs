using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassandra;
using DAL.Cassandra.Interfaces;
using DTO.Cassandra;

namespace DAL.Cassandra.Concrete
{
    public class UserStream : IUserStream
    {
        public void SyncUserStream(ISession session, Guid postId, TimeUuid updatedAtPrev)
        {
            // Get follower list
            var getFollowerList = session.Prepare("Select followers from post_followers where post_id = ? ");
            var listFollowers = session.Execute(getFollowerList.Bind(postId));

            List<Guid> list = new List<Guid>();
            foreach (var follower in listFollowers)
            {
                list = follower.GetValue<List<Guid>>("followers");

            }

            //Delete post from user stream
            var deletePost = session.Prepare("DELETE FROM user_stream where user_id =? and last_updated_at = ?");
            foreach (var follower in list)
            {
                session.Execute(deletePost.Bind(follower, updatedAtPrev));
            }


            //Get data from posts
            Guid authorId = new Guid();
            TimeUuid updatedAt = new TimeUuid();
            string content = "";

            var getPosts = session.Prepare("Select * from posts where post_id = ? ");
            var postsList = session.Execute(getPosts.Bind(postId));

            foreach (var post in postsList)
            {
                authorId = post.GetValue<Guid>("author_id");
                updatedAt = post.GetValue<TimeUuid>("updated_at");
                content = post.GetValue<string>("content");
            }


            //Insert post in user stream
            var insertUserStream =
                session.Prepare("INSERT INTO user_stream (user_id, last_updated_at , post_id , author_id , content) VALUES(?,?,?,?,?,?)");

            foreach (var follower in list)
            {
                session.Execute(insertUserStream.Bind(follower, updatedAt, postId, authorId, content));
            }

        }
    }
}