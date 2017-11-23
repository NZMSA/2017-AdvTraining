# Custom Vision
## Introduction
Custom vision is a way for you to analyze images. In this module we will be training our model to recognize 2 items. 
Before we begin, make sure you sign up for a Microsoft account and sign in via <https://customvision.ai/>

## Set up
After signing in, you a have nice big option for you to create a project. It will then ask for a project name, decription and domains. Fill these in for your type or project

## Training your model
The next step is to train your model and for our FoodBot we would like to identify images of food.
We want to be able to identify coffee and chips. On the left hand pane click on *'+'* and enter the name of each item we want to identify.
Once you've complete this step you should have something like this. 

![Training](photos/1.png?raw=true)

Now we look for 5 images each for *chips* and *coffee* for custom vision to reference to. Upload these images to custom vision via the 'Add images' ribbon and enter the appropriate tag. Repeat this step for chips.

![AddImages](photos/2.png?raw=true)

Finally find the green *Train* button at the top and our model has now been trained. 

## Making a post request via our bot.
We want to be able to send an image URL to our bot and hope that the item we send has been identified.
To do so we must first know when an image url is being sent and the second part is to make a POST request to our custom vision model if it is and use the reponse that we get.

Let's create a new js file and call it `CustomVision.js` where we will make a post request to custom vision

Inside our `LuisDialog.js` insert the following at the top

```
var cognitive = require('./CognitiveDialog');
``` 

Inside LuisDialog.js we include a function to check if it is an image url, for demonstation purposes will assume that the link will always contain *http*.

```js
function isAttachment(session) { 
    var msg = session.message.text;
    if ((session.message.attachments && session.message.attachments.length > 0) || msg.includes("http")) {
        //call custom vision
        customVision.retreiveMessage(session);

        return true;
    }
    else {
        return false;
    }
}
```

Next inside our newly created custom vision file create the following. 

```js
var request = require('request'); //node module for http post requests

exports.retreiveMessage = function (session){

    request.post({
        url: 'YOUR-URL',
        json: true,
        headers: {
            'Content-Type': 'application/json',
            'Prediction-Key': 'YOUR-PREDICTION-KEY'
        },
        body: { 'Url': session.message.text }
    }, function(error, response, body){
        console.log(validResponse(body));
        session.send(validResponse(body));
    });
}

function validResponse(body){
    if (body && body.Predictions && body.Predictions[0].Tag){
        return "This is " + body.Predictions[0].Tag
    } else{
        console.log('Oops, please try again!');
    }
}
```

To find the relevant URL's go back to <https://customvision.ai/>
`YOUR-URL` can be found under the *Performance* tab click on *Prediction URL*

`YOUR-PREDICTION-KEY` can be found by clicking on the cog icon on the right

## Done!









