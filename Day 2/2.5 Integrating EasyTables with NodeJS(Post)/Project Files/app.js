var restify = require('restify');
var builder = require('botbuilder');
var luis = require('./controller/LuisDialog');

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

// Receive messages from the user
var bot = new builder.UniversalBot(connector, function (session) {
    
    session.send('Sorry, I did not understand \'%s\'. Type \'help\' if you need assistance.', session.message.text);
});
    
// This line will call the function in your LuisDialog.js file
luis.startDialog(bot);