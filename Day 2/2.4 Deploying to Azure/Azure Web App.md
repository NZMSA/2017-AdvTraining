# Azure Web App (Draft)
## 1. Introduction
In this part, we are going create a web app (website) that will have our chat bot embedded within it, and then host the web app on Azure.

## 2. Create Web App Project
* Open Visual Studio
* Select New -> Project -> Web -> ASP.NET Web Application
* Right-click on the project in the Solution Explorer
* Select Add -> New Item -> HTML file
* Do the same but select CSS file (instead of HTMLfile)

## 3. Write Some HTML
In the head tag, we can set the "title", which is the name that appears in the tab, and link any CSS or JavaScript files that the web page requires.

For this website, we have set the title to MSPs Rock, and have linked the CSS file we created before.
```html
<head>
    <meta charset="utf-8" />
    <title>MSPs Rock</title>
    <link href="style-ly.css" rel="stylesheet" />
</head>
```

In the body tag is where we place all of the content for the web page.

For this website, we have added a logo, heading, and the bot. The logo is a simple image tag with the source pointing to a file added to the solution. The heading is just the heading 1 tag. Finally, the bot is added in as an iframe pointing to a url.

To get the code used for the iframe: 
* Go to the bot framework website, [dev.botframework.com](dev.botframework.com).
* Sign in with a microsoft account
* Navigate to the "My bots" page
* If you are yet to register your bot (it does not show up) follow these steps.
  - Select "Create a bot"
  - Select "Create"
  - Select "Register an existing bot..."
  - Fill in all the details
  - Messaging endpoint should be the web address hosting your bot on azure with "/api/messages" added to the end.
* Otherwise, select your bot.
* Select edit on the "Web Chat" channel that was already configured.
* In the Embed code section is the code for the iframe tag needed, so copy that into the html.
* Replace the "YOUR_SECRET_HERE" with one of the "Secret keys" which you can reveal by clicking show.

Your code in the body tag should look as follows:
```html
<body>
    <img src="msp_logo.png" />
    <h1>MSA 2017</h1>
    <iframe src='https://webchat.botframework.com/embed/nz-msp-food-bot?s=2TP8Pglpf2E.cwA.XTg.NJVCqMbLlGEcJsnA9hPZLu12lN64VixaFOwkOrVquVA'></iframe>
</body>
```

## 4. Make it look pretty...
Now we can start editting the CSS file to make the content in the html look how we want it to.

First, we will style the contents of the web page (and more specifically the header). We will align all content to the centre of the page, and set the background colour of the page (you can pick any colour by looking up the hexadecimal colour).
```css
body {
  text-align: center;
  background-color: #235B7E;
}
```

Now, we will style the header to appear as we would like it.
```css
h1 {
  font-family: Arial;
  color: white;
  font-weight: bold;
}
```

Next, we will position the image to the top left corner, and size it appropriately. Setting the position to absolute allows us to specify the image location from the web page, and ssetting just the width of the image will reduce the height to maintain the aspect ratio of the image automatically.
```css
img {
  position: absolute;
  top: 25px;
  left: 25px;
  width: 150px;
}
```

Finally, we will style the iframe that contains the embeded bot.
```css
iframe {
    width: 50%;
    height: 600px;
    background-color: white;
}
```

You should be noticing that before the curly braces, we have been putting the html tag names of the element we wish to style. This is not very practical, as websites will typically have many elements with the same tag that need to be styled differently. To get around this you can set you can set a class or id for the element and reference that in the CSS instead. Note, that if you are specifying the name of a class you add a dot before it, and if you are specifying an id you add a hash before it. See examples below.
```html
<body>
  <h1 id="heading_one">THIS IS USING AN ID<h1>
  <h1 class="headings">THIS IS USING A CLASS<h1>
</body>
```
```css
#heading_one {
  color: black;
  font-family: Arial;
  font-weight: bold;
  font-size: 25px;
}

.headings {
  color: black;
  font-size: 40px;
}
```

## 5. Publish our Azure Web App
We have now finished our web app! However, before we start to publish commit your Web App to a NEW Git repo. 
Now it's time to publish it to Azure!
* Open [Azure](portal.azure.com)
* Select New -> Web App
* Fill in all the info
* Click Create
* Go to the newly created resource
* Select Deployment options
* Select GitHub as the source
* Select the repo you just created and commited your web app to
* Select OK
* Navigate to the url your web app is, and see if it worked (which it hopefully did!)
