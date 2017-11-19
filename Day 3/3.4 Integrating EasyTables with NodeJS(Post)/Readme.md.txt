# 3.4 Creating a post request and sending it to EasyTablels

### Introduction

Similarly, like the previous tutorial for the GET Request, we want to be able to POST from our application to easytables. This will be a guide on how to do so.


### 3.4.1 Creating the intermediary function

Here we will create a function inside the directory FavouriteFoods that will be called from the LUIS directory to post a request.

```javascript
exports.sendFavouriteFood = function postFavouriteFood(session, username, favouriteFood){
    var url = 'https://foodbotmsa.azurewebsites.net/tables/FoodBot';
    rest.postFavouriteFood(url, username, favouriteFood);
};

```

Here, again like previously we are creating the function that will link easytables to the LUIS dialog, this is because
we want a seperation of concerns between them and so that it is easier to maintain them, and debugging.

The parameters are the current session, the username, and the favourite food we want to post. 

We will now go on to create the function that posts to easytables.

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
along with the inforrmation we want.

So we are specifying the URL we want, the method that we want to use to send it (POST) and the headers so that
the server knows what type of data we are sending to it.

Underneath this is a JSON Payload which contains the data wewant to post, in this case it is the username of the user
and their favourite food.

We then check the response status and logthe response.


### 3.4.5 Extra

If you want, you can create your own callback function to handle the response and to show to users. I will leave it up to you
for practice.