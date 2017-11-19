# 3.4 Creating a post request and sending it to EasyTables

### Introduction

Similarly, like the previous tutorial for the GET Request, we want to be able to POST from our application to easy tables. This will be a guide on how to do so.


### 3.4.1 Creating the intermediary function and LUIS call

Here we will create a function inside the directory FavouriteFoods that will be called from the LUIS directory to post a request.

```javascript
exports.sendFavouriteFood = function postFavouriteFood(session, username, favouriteFood){
    var url = 'https://foodbotmsa.azurewebsites.net/tables/FoodBot';
    rest.postFavouriteFood(url, username, favouriteFood);
};

```

Here, again like previously we are creating the function that will link easy tables to the LUIS dialog, this is because
we want a seperation of concerns between them and so that it is easier to maintain them, and debugging.

The parameters are the current session, the username, and the favourite food we want to post.

We now want to reference this inside LUIS

```javascript

    bot.dialog('LookForFavourite', [
        function (session, args, next) {
            session.dialogData.args = args || {};        
            if (!session.conversationData["username"]) {
                builder.Prompts.text(session, "Enter a username to setup your account.");                
            } else {
                next(); // Skip if we already have this info.
            }
        },
        function (session, results, next) {
            if (!isAttachment(session)) {

                if (results.response) {
                    session.conversationData["username"] = results.response;
                }
                // Pulls out the food entity from the session if it exists
                var foodEntity = builder.EntityRecognizer.findEntity(session.dialogData.args.intent.entities, 'food');

                // Checks if the food entity was found
                if (foodEntity) {
                    session.send('Thanks for telling me that \'%s\' is your favourite food', foodEntity.entity);
                    food.sendFavouriteFood(session, session.conversationData["username"], foodEntity.entity); // <-- LINE WE WANT

                } else {
                    session.send("No food identified!!!");
                }
            }
        }
    ]).triggerAction({
        matches: 'LookForFavourite'
    });
```
We are making a reference to the intermediary function inside of LUIS so that we can call it while processing.

We will now go on to create the function that posts to easy tables.

### 3.4.2 Creating the function that posts
```javascript

exports.postFavouriteFood = function getData(url, username, favouriteFood){
    var options = {
        url: url,
        method: 'POST',
        headers: {
            'ZUMO-API-VERSION': '2.0.0',
            'Content-Type':'application/json'
        },
        json: {
            "username" : username,
            "favouriteFood" : favouriteFood
        }
      };

      request(options, function (error, response, body) {
        if (!error && response.statusCode === 200) {
            console.log(body);
        }
        else{
            console.log(error);
        }
      });
};

```

This time we have to do something different to the GET requests, here we have to specify the headers that we are sending
along with the information we want.

So we are specifying the URL we want, the method that we want to use to send it (POST) and the headers so that
the server knows what type of data we are sending to it.

Underneath this is a JSON Payload which contains the data we want to post, in this case it is the username of the user
and their favourite food.

We then check the response status and log the response.


### 3.4.5 Extra

If you want, you can create your own callback function to handle the response and to show to users. I will leave it up to you
for practice.
