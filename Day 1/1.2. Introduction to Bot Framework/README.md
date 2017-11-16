# 1.2. Introduction to Bot Framework

### 1.2.1 Create a bot with the Bot Builder SDK for Node.js [Source](https://docs.microsoft.com/en-us/bot-framework/nodejs/bot-builder-nodejs-quickstart)
The Bot Builder SDK for Node.js is a framework for developing bots. It is easy to use and models frameworks like Express & restify to provide a familiar way for JavaScript developers to write bots.+
This tutorial walks you through building a bot by using the Bot Builder SDK for Node.js. You can test the bot in a console window and with the Bot Framework Emulator.

## Prerequisites

Get started by completing the following prerequisite tasks:+
1. Install Node.js.
2. Create a folder for your bot.
3. From a command prompt or terminal, navigate to the folder you just created.
4. Run the following npm command:

```javascript
npm init
``` 
Follow the prompt on the screen to enter information about your bot and npm will create a package.json file that contains the information you provided.

## Install the SDK

Next, install the Bot Builder SDK for Node.js by running the following npm command:+

```javascript
npm install --save botbuilder
```

Once you have the SDK installed, you are ready to write your first bot.

For your first bot, you will create a bot that simply echoes back any user input. To create your bot, follow these steps:

1. In the folder that you created earlier for your bot, create a new file named app.js.

2. Open app.js in a text editor or an IDE of your choice. Add the following code to the file:

```javascript
var builder = require('botbuilder');

var connector = new builder.ConsoleConnector().listen();
var bot = new builder.UniversalBot(connector, function (session) {
    session.send("You said: %s", session.message.text);
});
```

3. Save the file. Now you are ready to run and test out your bot.

### Start your bot

Navigate to your bot's directory in a console window and start your bot

```javascript
node app.js
```

Your bot is now running locally. Try out your bot by typing a few messages in the console window. You should see that the bot responds to each message you send by echoing back your message prefixed with the text "You said:".

## Install Restify

Console bots are good text-based clients, but in order to use any of the Bot Framework channels (or run your bot in the emulator), your bot will need to run on an API endpoint. Install restify by running the following npm command:

```javascript
npm install --save restify
```
Once you have Restify in place, you're ready to make some changes to your bot.

## Edit your bot

You will need to make some changes to your app.js file.

1. Add a line to require the `restify` module.
2. Change the `ConsoleConnector` to a `ChatConnector`.
3. Include your Microsoft App ID and App Password.
4. Have the connector listen on an API endpoint.

```javascript
var restify = require('restify');
var builder = require('botbuilder');

// Setup Restify Server
var server = restify.createServer();
server.listen(process.env.port || process.env.PORT || 3978, function () {
   console.log('%s listening to %s', server.name, server.url); 
});

// Create chat connector for communicating with the Bot Framework Service
var connector = new builder.ChatConnector({
    appId: process.env.MICROSOFT_APP_ID,
    appPassword: process.env.MICROSOFT_APP_PASSWORD
});

// Listen for messages from users 
server.post('/api/messages', connector.listen());

// Receive messages from the user and respond by echoing each message back (prefixed with 'You said:')
var bot = new builder.UniversalBot(connector, function (session) {
    session.send("You said: %s", session.message.text);
});
```

5. Save the file. Now you are ready to run and test out your bot in the emulator.

`Note: You do not need a Microsoft App ID or Microsoft App Password to run your bot in the Bot Framework Emulator.`

Please note that you do not need a Microsoft App ID or App Password to run your bot in the Bot Framework Emulator.

## Test your bot

Next, test your bot by using the Bot Framework Emulator to see it in action. The emulator is a desktop application that lets you test and debug your bot on localhost or running remotely through a tunnel.+
First, you'll need to download and install the emulator. After the download completes, launch the executable and complete the installation process.

### Start your bot

After installing the emulator, navigate to your bot's directory in a console window and start your bot:+

```javascript
node app.js
```

Your bot is now running locally.

## Start the emulator and connect your bot

After you start your bot, connect to your bot in the emulator:
1. Type http://localhost:3978/api/messages into the address bar. (This is the default endpoint that your bot listens to when hosted locally.)
2. Click Connect. You won't need to specify Microsoft App ID and Microsoft App Password. You can leave these fields blank for now. You'll get this information later when you register your bot.

## Try out your bot

Now that your bot is running locally and is connected to the emulator, try out your bot by typing a few messages in the emulator. You should see that the bot responds to each message you send by echoing back your message prefixed with the text "You said:".
You've successfully created your first bot using the Bot Builder SDK for Node.js!


### Extra Learning Resources
* [Using App Service with Xamarin by Microsoft](https://azure.microsoft.com/en-us/documentation/articles/app-service-mobile-dotnet-how-to-use-client-library/)
* [Using App Service with Xamarin by Xamarin - Outdated but good to understand](https://blog.xamarin.com/getting-started-azure-mobile-apps-easy-tables/)
* [ListView in Xamarin](https://developer.xamarin.com/guides/xamarin-forms/user-interface/listview/)