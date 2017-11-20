# 1.3 LUIS Natural Language Integration

## Introduction
LUIS is an acronym for Language Understanding Intelligence Service. LUIS can be trained to recognise the intent behind what the user says to your bot.
The goal here is to create a LUIS app. When you're finished, you'll have a LUIS endpoint up and running in the cloud. 

###  SOURCES: 
* [Create your first LUIS app in ten minutes](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-get-started-create-app)
* [Add Intents](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/add-intents)
* [Call a LUIS app using Node.js](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-get-started-node-get-intent)

# Create a new LUIS app 
Start by going to [www.luis.ai](www.luis.ai) and singing in.
You can create and manage your applications on the My Apps page. You can always access this page by clicking My Apps on the top navigation bar of the LUIS web page.

* Start by selecting "New App" to create a new LUIS app.

![New LUIS App](photos/NewApp.png)

1. In the dialog box, name your application "foodbot".
2. The culture field should be "English"
3. Select *Create*

![New LUIS App Dialogue Box](photos/CreateNewApp.png)

LUIS creates the foodbot app and opens its main page which looks like the following screen. Use the navigation links in the left panel to move through your app pages to define data and work on your app.

![Dashboard](photos/Dashboard.png)

## Add intents

Your first task in the app is to add intents. Intents are the intentions or requested actions conveyed by the user's utterances. They are the main building block of your app. You now need to define the intents (for example, book a flight) that you want your application to detect. Go to the Intents page in the side menu to create your intents by clicking the Add Intent button.

To demonstrate how intents can be added we will be adding the "GetCalories" intent. If the user says, "how many calories in pizza" we want LUIS to recognise the user's intent is to get the number of calories in pizza.

1. Click Intents in the left panel
2. On the Intents page, click Add intent.

![Intents Dashboard](photos/IntentsDashboard.png)

3. In the *Add Intent* dialog box, type the intent name "GetCalories" and click *Save*.

![Create Intent](photos/CreateIntent.png)

This takes you directly to the intent details page of the newly added intent "GetCalories", like the following screenshot, to add utterances for this intent.

Before we add an utterance (e.g. "how many calories in pizza") let's create an entity for food. This will allow LUIS to pick out the food that the user wants to get information about. In the example "how many calories in pizza", pizza is the entity.

## Add entities

![Create Entity](photos/CreateEntity.png)

1. Click Entities in the left panel
2. On the Entities page, click *Add custom entity*.
3. Enter the *Entity name*, "food"
4. Select the *Entity type*, "simple"
5. Click *Save* 

![Add Custom Entity](photos/AddEntity.png)

Now we will add an utterance to the entity

1. Click Intents in the left panel
2. Select the intent you created "GetCalories"
3. Enter the utterance "how many calories in pizza" in the text field

![Add Utterance](photos/AddUtterance.png)

Next, you need to label examples of the entities to teach LUIS what this entity can look like. Highlight relevant tokens as entities in the utterances you added.

4. Select pizza
5. Select the entity food
6. Select *Save*

![Select Entity](photos/SelectingEntity.png)

Now create some more utterances for the intent "GetCalories". Think of natural ways you would ask for how calories in food. For Example, "calories in an apple". Make sure to select the entity each time a put it into the food category.

Add eight different utterances before going on to the next step. The more utterances you add and the more variety, the more likely it is that LUIS will be able to determine the user's intent accurately.  

## Training LUIS

1. Click Train & Test in the left panel
2. Click *Train Application*

![Train LUIS](photos/Training.png)

## Test your app
Once you've trained your app, you can test it by typing a test utterance and pressing Enter. The results display the score associated with each intent. Check that the top scoring intent corresponds to the intent of each test utterance.

## Publish your app

1. Select *Publish App* from the left-side menu 
2. click *Publish*.

## Use your app

Copy the endpoint URL from the Publish App page and paste it into a browser. Append a query like "how many calories in pizza" at the end of the URL and submit the request. The JSON containing results should show in the browser window.

## Call a LUIS app using Node.js

In your app.js class you will need the following:

```js 
var restify = require('restify');
var builder = require('botbuilder');
var luis = require('./controller/LuisDialog');
// Some sections have been omitted

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

// This line will call the function in your LuisDialo.js file
luis.startDialog(bot);
 
```

In your LuisDialog.js file you will need the following:

```js

var builder = require('botbuilder');
// Some sections have been omitted

exports.startDialog = function (bot) {
    
    // Replace {YOUR_APP_ID_HERE} and {YOUR_KEY_HERE} with your LUIS app ID and your LUIS key, respectively.
    var recognizer = new builder.LuisRecognizer('https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/{YOUR_APP_ID_HERE}?subscription-key={YOUR_KEY_HERE}&timezoneOffset=0&q=');

    bot.recognizer(recognizer);

    bot.dialog('GetCalories', function (session, args) {
        if (!isAttachment(session)) {

            // Pulls out the food entity from the session if it exists
            var foodEntity = builder.EntityRecognizer.findEntity(args.intent.entities, 'food');

            // Checks if the for entity was found
            if (foodEntity) {
                session.send('Calculating calories in %s...', foodEntity.entity);
               // Here you would call a function to get the foods nutrition information

            } else {
                session.send("No food identified! Please try again");
            }
        }
    }).triggerAction({
        matches: 'GetCalories'
    });
}
```

## Putting it all together

Once you have gone through the above example for the GetCalories intent, you can apply the same process to add the rest of the intents.

Below are some sample utterances to help you get started. Be creative and put yourself into the mind of the user when generating utterances. The better your utterances match how the user will express them selfs the accurately LUIS will be able to determine the users intent.

Note: [ $food ] represents a food item (e.g. pizza) which has been marked in LUIS as an entity. For more information see "Add entities" above.

1. DeleteFavourite
* "delete [ $food ] from my favourites"
* "remove [ $food ] from my favourites list"
* "I want to delete [ $food ]"
* I don ' t like [ $food ] anymore

2. GetFavouriteFood
* "what are my favourite foods"
* "what do i like"

3. LookForFavourite
* "my favourite food is [ $food ]"
* "[ $food ] is life"
* i like [ $food ]

4. WantFood
* "[ $food ] is what I want"
* "[ $food ] is what I want to eat"
* "I ' m starving , I want a [ $food ]"
* "I want a [ $food ]"

5. WelcomeIntent
* "hi"
* "hello"

Once you are done your LuisDialog.js file should look like this:

```js
var builder = require('botbuilder');


exports.startDialog = function (bot) {

    var recognizer = new builder.LuisRecognizer('https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/{YOUR_APP_ID_HERE}?subscription-key={YOUR_KEY_HERE}&timezoneOffset=0&q=');

    bot.recognizer(recognizer);

    bot.dialog('WantFood', function (session, args) {
        if (!isAttachment(session)) {
            // Pulls out the food entity from the session if it exists
            var foodEntity = builder.EntityRecognizer.findEntity(args.intent.entities, 'food');

            // Checks if the food entity was found
            if (foodEntity) {
                session.send('Looking for restaurants which sell %s...', foodEntity.entity);
                // Insert logic here later
            } else {
                session.send("No food identified! Please try again");
            }
        }

    }).triggerAction({
        matches: 'WantFood'
    });

    bot.dialog('DeleteFavourite', [
        // Insert delete logic here later
    ]).triggerAction({
        matches: 'DeleteFavourite'

    });

    bot.dialog('GetCalories', function (session, args) {
        if (!isAttachment(session)) {

            // Pulls out the food entity from the session if it exists
            var foodEntity = builder.EntityRecognizer.findEntity(args.intent.entities, 'food');

            // Checks if the for entity was found
            if (foodEntity) {
                session.send('Calculating calories in %s...', foodEntity.entity);
                // Insert logic here later

            } else {
                session.send("No food identified! Please try again");
            }
        }
    }).triggerAction({
        matches: 'GetCalories'
    });

    bot.dialog('GetFavouriteFood', [
       // Insert favourite food logic here later
    ]).triggerAction({
        matches: 'GetFavouriteFood'
    });

    bot.dialog('LookForFavourite', [
        // Insert logic here later
    ]).triggerAction({
        matches: 'LookForFavourite'
    });
    

    bot.dialog('WelcomeIntent', [
        // Insert logic here later
    ]).triggerAction({
        matches: 'WelcomeIntent'
    });
}

// Function is called when the user inputs an attachment
function isAttachment(session) { 
    var msg = session.message.text;
    if ((session.message.attachments && session.message.attachments.length > 0) || msg.includes("http")) {
        
        //call custom vision here later
        return true;
    }
    else {
        return false;
    }
}
```
