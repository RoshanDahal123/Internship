using System;
using System.Collections.Generic;
using System.Text;

namespace abastractClasses
{
  public abstract class Notification
    {

        //Abstract Property - Child must implement 
        public abstract string ChannelName { get; }
        //Abstract method - No body allowed here;
        public abstract void Send(string message);
        // concrete method -fully functional shared code;

        public void LogDeleivery(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] Logged {ChannelName}: {message}");
        }




    }

    public class EmailNotification : Notification
    {
        public override string ChannelName => "Email";
        //Implement the method using 'override' keyword

        public override void Send(string message)
        {
            Console.WriteLine($"Sending Email:{message}");

        }
    }

}
