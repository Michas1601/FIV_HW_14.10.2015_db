using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstNewDatabaseSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //at the end of the using block MessageContext automatically calls the Dispose() method
            using (var db = new MessageContext())
            {
                //Enter recipient and message content
                Console.Write("Recipient: ");
                var recipient = Console.ReadLine();
                Console.Write("Message: ");
                var content = Console.ReadLine();

                //create new Message with the information from above
                var message = new Message { Recipient = recipient, Content = content };
                //add the message to the DbSet "Messages"
                db.Messages.Add(message);
                db.SaveChanges();

                //query all messages in the DbSet "Messages" and sort them by recipient
                var query = from m in db.Messages
                            orderby m.Recipient
                            select m;
                Console.WriteLine("Messages ordered by recipient:");
                //output each recipient
                foreach (var item in query)
                {
                    Console.WriteLine(item.Recipient);
                }
            }
            Console.ReadLine();
        }
    }

    /// <summary>
    /// a Message which has an ID, recipient, content and a list of attached files
    /// </summary>
    public class Message
    {
        public int MessageId { get; set; }
        public string Recipient { get; set; }
        public string Content { get; set; }

        public virtual List<File> AttachedFiles { get; set; }
    }

    /// <summary>
    /// a File which has an ID, a path where it's stored and a Message where it belongs to
    /// </summary>
    public class File
    {
        public int FileId { get; set; }
        public string Path { get; set; }

        public int MessageId { get; set; }
        public virtual Message Message { get; set; }
    }

    /// <summary>
    /// the bridge to communicate with the database (DbContext)
    /// </summary>
    public class MessageContext : DbContext
    {
        //a Set of Messages
        public DbSet<Message> Messages { get; set; }
        //a Set of Files
        public DbSet<File> Files { get; set; }
    }
}
