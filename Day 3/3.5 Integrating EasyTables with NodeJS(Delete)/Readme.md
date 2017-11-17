#3.5 Creating a delete request and sending it to easytables

### Introductioon

We want the user to be able to remove favourites if they change their mind about something 

We will do this the same way we have done the other two functions


### 3.5.1 Intermediary Function And connecting to LUIS

```javascript
exports.deleteFavouriteFood = function deleteFavouriteFood(session,username,favouriteFood){
    var url  = 'https://foodbotmsa.azurewebsites.net/tables/FoodBot';


    rest.getFavouriteFood(url,session, username,function(message,session,username){
     var   allFoods = JSON.parse(message);

        for(var i in allFoods) {

            if (allFoods[i].favouriteFood === favouriteFood && allFoods[i].username === username) {

                console.log(allFoods[i]);

                rest.deleteFavouriteFood(url,session,username,favouriteFood, allFoods[i].id ,handleDeletedFoodResponse)

            }
        }


    });


};

```

This time we first need to get what's stored inside the users database, and then we must match the user to the food that they want deleted, and then we can send a request to delete that particular food.
We need to retrieve all the foods, so that we can use the id of the particular entry to delete the food.

We now want to refer this inside of LUIS so that we can call it when we detect the right intent.

```javascript

    bot.dialog('DeleteFavourite', [
        function (session, args, next) {
            session.dialogData.args = args || {};
            if (!session.conversationData["username"]) {
                builder.Prompts.text(session, "Enter a username to setup your account.");
            } else {
                next(); // Skip if we already have this info.
            }
        },
        function (session, results,next) {
        if (!isAttachment(session)) {

            session.send("You want to delete one of your favourite foods.");

            // Pulls out the food entity from the session if it exists
            var foodEntity = builder.EntityRecognizer.findEntity(session.dialogData.args.intent.entities, 'food');

            // Checks if the for entity was found
            if (foodEntity) {
                session.send('Deleting \'%s\'...', foodEntity.entity);
                food.deleteFavouriteFood(session,session.conversationData['username'],foodEntity.entity); //<--- CALLL WE WANT
            } else {
                session.send("No food identified! Please try again");
            }
        }

    }
    
   ```

### 3.5.2 Delete function

```javascript
exports.deleteFavouriteFood = function deleteData(url,session, username ,favouriteFood, id, callback){
    var options = {
        url: url + "\\" + id,
        method: 'DELETE',
        headers: {
            'ZUMO-API-VERSION': '2.0.0',
            'Content-Type':'application/json'
        }
    };

    request(options,function (err, res, body){
        if( !err && res.statusCode === 200){
            console.log(body);
            callback(body,session,username, favouriteFood);
        }else {
            console.log(err);
            console.log(res);
        }
    })

};
```

We again, like the post request have to attach headers, and the url will be different this time, we need the particular id of the entry we want to delete, and so we must append that to the url.

We then process the request by using a callback or we log the error.


### 3.5.3 Handler function

I will allow you to implement this as a callback function for practice.
