using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFoodApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
        //Initialize a connection with ID and Name
        BotConnection connection = new BotConnection("Paul");

        ObservableCollection<MessageListItem> messageList = new ObservableCollection<MessageListItem>();

        public MainPage ()
		{
			InitializeComponent ();

            //Bind the ListView to the ObservableCollection
            MessageListView.ItemsSource = messageList;

            //Start listening to messages and add any new ones to the collection
            var messageTask = connection.GetMessagesAsync(messageList);
        }

        public void Send(object sender, EventArgs args)
        {
            //Get text in entry
            var message = ((Entry)sender).Text;

            if (message.Length > 0)
            {
                //Clear entry
                ((Entry)sender).Text = "";

                //Make object to be placed in ListView
                var messageListItem = new MessageListItem(message, connection.Account.Name);
                messageList.Add(messageListItem);

                //Send the message to the bot
                connection.SendMessage(message);
            }
        }
    }
}