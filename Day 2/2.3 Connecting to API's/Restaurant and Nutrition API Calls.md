# Restaurant and Nutrition API Calls (Draft)
## 1. Introduction
In this part, we are going to make two different RESTful API calls:
* [Yelp](https://www.yelp.com/developers/documentation/v3/business_search) -  to get a list of restaurants that sell a specific type of food (i.e. pizza)
* [USDA](https://ndb.nal.usda.gov/ndb/doc/index) - to get nutritional information for a specific type of food (i.e. calories, fat etc.)

For Yelp, we will display the restaurants in a carousel using hero cards that show name, address, image and a button that opens up the yelp website with more information.

For USDA, we will display nutrition information such as calories, energy in an adaptive card. 

## 2. Restaurant Cards (Yelp)
* Create a folder called API
* Inside this folder, create a file called - ```RestClient.js```
* In ```RestClient.js```, paste the following: 
```javascript
var request = require('request');

exports.getYelpData = function getData(url,bearer,session, callback){

    request.get(url,{'auth': { 'bearer': bearer}} ,function(err,res,body){
        if(err){
            console.log(err);
        }else {
            callback(body,session);
        }
    });
};
```
The function ```getYelpData``` takes in the yelp URL, auth key, session and callback method. It makes the GET request and sends the content through to the callback function.

Now we are going to create a file that will call ```getYelpData``` function and so that we can display it onto the chat box

* Inside the controller folder, create a file called ```RestaurantCard.js``` 
* Inside ```RestaurantCard.js``` copy the following:
```javascript
var rest = require('../API/Restclient');
var builder = require('botbuilder');

//Calls 'getYelpData' in RestClient.js with 'displayRestaurantCards' as callback to get list of restaurant information
exports.displayRestaurantCards = function getRestaurantData(foodName, location, session){
    var url ='https://api.yelp.com/v3/businesses/search?term='+foodName+'&location='+location + '&limit=5';
    var auth ='cO92idzWqWjpOsV8RdAoB2DZl2GW8OE8pvoTlOjNNI0gbA2J7xXuiAPtLAYCkPCKR-dIXG3ePsSI4ngt8WRNQ4q4RlKMdXyvJr6r4_L3kndI5wpznLN6WUrPmgDYWXYx';
    rest.getYelpData(url,auth,session,displayRestaurantCards);
}
```
This function ```displayRestaurantCards``` will call the ```getYelpData``` function we just created. Notice that we are passing in a callback function called ```displayRestaurantCards``` which we have yet to create. So we now need to create this function which will take the data and display it onto the chat box.

* Right below the ```displayRestaurantCards``` function add in the following
```javascript
function displayRestaurantCards(message, session) {
    var attachment = [];
    var restaurants = JSON.parse(message);
    
    //For each restaurant, add herocard with name, address, image and url in attachment
    for (var index in restaurants.businesses) {
        var restaurant = restaurants.businesses[index];
        var name = restaurant.name;
        var imageURL = restaurant.image_url;
        var url = restaurant.url;
        var address = restaurant.location.address1 + ", " + restaurant.location.city;

        var card = new builder.HeroCard(session)
            .title(name)
            .text(address)
            .images([
                builder.CardImage.create(session, imageURL)])
            .buttons([
                builder.CardAction.openUrl(session, url, 'More Information')
            ]);
        attachment.push(card);

    }

    //Displays restaurant hero card carousel in chat box 
    var message = new builder.Message(session)
        .attachmentLayout(builder.AttachmentLayout.carousel)
        .attachments(attachment);
    session.send(message);
}
```

This is function takes in the content from the Yelp GET request and creates a list of Hero Cards that contain the restaurant name, address, image and link to the website. We then post this list onto the chat box as a carousel!

The last thing we need to do is to call ```displayRestaurantCards``` from LUIS.

*  Open up ```LuisDialog.js``` and at the top (second line after botbuilder), add in 
```javascript
var restaurant = require('./RestaurantCard');
``` 
* Find the 'WantFood' dialog inside ```LuisDialog.js``` and add in the following after the message 'Looking for restaurants which sells...'
```javascript
restaurant.displayRestaurantCards(foodEntity.entity, "auckland", session);
```

The 'WantFood' dialog should now look something like this:
```javascript
bot.dialog('WantFood', function (session, args) {

        if (!isAttachment(session)) {
            // Pulls out the food entity from the session if it exists
            var foodEntity = builder.EntityRecognizer.findEntity(args.intent.entities, 'food');

            // Checks if the for entity was found
            if (foodEntity) {
                session.send('Looking for restaurants which sell %s...', foodEntity.entity);
                restaurant.displayRestaurantCards(foodEntity.entity, "auckland", session);
            } else {
                session.send("No food identified! Please try again");
            }
        }

    }).triggerAction({
        matches: 'WantFood'
    });
```
## 2. Nutrition Information
Making RESTful calls to USDA to get nutrition information is mostly going to be similar. The main differences is that instead of one GET request, we will need to do two. First request is to get the ndbno of the food and the second is to use that ndbno to get nutritional information. 

Another difference is that instead of using herocards, we will use adaptive cards which will give us more control over the layout.

* Open up ```RestClient.js``` and add in the following:
```javascript
exports.getNutritionData = function getData(url, session, foodName, callback){

    request.get(url, function(err,res,body){
        if(err){
            console.log(err);
        }else {
            callback(body, foodName, session);
        }
    });
};
```
* Inside the controller folder, create a file called ```nutritionCard.js```
* In this file, add the following:
```javascript
var rest = require('../API/Restclient');
var builder = require('botbuilder');

//Calls 'getNutritionData' in RestClient.js with 'getFoodNutrition' as callback to get ndbno of food
exports.displayNutritionCards = function getNutritionData(foodName, session){
    var url = "https://api.nal.usda.gov/ndb/search/?format=json&q="+foodName+"&sort=r&max=1&offset=0&api_key=tZyOKm2kvj2EjxXxsv5jCb6ZUfjlYaWEFZ22t4Bu";

    rest.getNutritionData(url, session,foodName, getFoodNutrition);
}
```
Similar to the Yelp part, this function will call the ```getNutritionData``` function we just created, passing in the GET url, session, food name and a callback function called ```getFoodNutrition``` which we have yet to create. We now need to create this function.

* Create a function called ```getFoodNutrition``` by adding in the following:
```javascript
//Parses JSON to get the ndbno. Calls 'getNutritionData' in RestClient.js with 'displayNutritionCards' as callback to get nutrition information
function getFoodNutrition(message, foodName, session){
    var foodNutritionList = JSON.parse(message);
    var ndbno = foodNutritionList.list.item[0].ndbno;
    var url = "https://api.nal.usda.gov/ndb/reports/?ndbno="+ndbno+"&type=f&format=json&api_key=tZyOKm2kvj2EjxXxsv5jCb6ZUfjlYaWEFZ22t4Bu";
    
    rest.getNutritionData(url, session, foodName, displayNutritionCards);

}
```
This function will get the ndbno from the first GET request and make another one to get nutritional information. We now need to create the function ```displayNutritionCards```.

* Create a function called ```displayNutritionCards``` by adding in the following:
```javascript
function displayNutritionCards(message, foodName,session){
    //Parses JSON
    var foodNutrition = JSON.parse(message);

    //Adds first 5 nutrition information (i.e calories, energy) onto list
    var nutrition = foodNutrition.report.food.nutrients;
    var nutritionItems = [];
    for(var i = 0; i < 5; i++){
        var nutritionItem = {};
        nutritionItem.title = nutrition[i].name;
        nutritionItem.value = nutrition[i].value + " " + nutrition[i].unit;
        nutritionItems.push(nutritionItem);
    }

    //Displays nutrition adaptive cards in chat box 
    session.send(new builder.Message(session).addAttachment({
        contentType: "application/vnd.microsoft.card.adaptive",
        content: {
            "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
            "type": "AdaptiveCard",
            "version": "0.5",
            "body": [
                {
                    "type": "Container",
                    "items": [
                        {
                            "type": "TextBlock",
                            "text": foodName.charAt(0).toUpperCase() + foodName.slice(1),
                            "size": "large"
                        },
                        {
                            "type": "TextBlock",
                            "text": "Nutritional Information"
                        }
                    ]
                },
                {
                    "type": "Container",
                    "spacing": "none",
                    "items": [
                        {
                            "type": "ColumnSet",
                            "columns": [
                                {
                                    "type": "Column",
                                    "width": "auto",
                                    "items": [
                                        {
                                            "type": "FactSet",
                                            "facts": nutritionItems
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ]
        }
    }));
}
```
This function gets the nutritional information and adds the first 5 nutritional items onto a list. Using adaptive cards, we display these items onto the chat box.

The final thing to do now is to call ```displayNutritionCards``` from LUIS.

* Open up ```LuisDialog.js``` and add in the reference to ```NutritionCards.js``` by adding the following at the top (just below RestaurantCard):
```javascript
var nutrition = require('./NutritionCard');
```

* Find the 'GetCalories' dialog and add in the following after the message 'Calculating calories in ...'
```javascript
nutrition.displayNutritionCards(foodEntity.entity, session);
```

'GetCalories' dialog should now look like this 
```javascript
bot.dialog('GetCalories', function (session, args) {
        if (!isAttachment(session)) {

            // Pulls out the food entity from the session if it exists
            var foodEntity = builder.EntityRecognizer.findEntity(args.intent.entities, 'food');

            // Checks if the for entity was found
            if (foodEntity) {
                session.send('Calculating calories in %s...', foodEntity.entity);
                nutrition.displayNutritionCards(foodEntity.entity, session);

            } else {
                session.send("No food identified! Please try again");
            }
        }
    }).triggerAction({
        matches: 'GetCalories'
    });
```
## Testing it out!
To test out the restaurant functionality, type 
>I want a burger

To test out the nutritional information functionality, type
>Calories of a burger
