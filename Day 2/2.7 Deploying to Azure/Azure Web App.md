# Deploying and Registering Bot
## 1. Deploy Bot to Azure
1.  Ensure bot is on GitHub
2.  Create "Web App" on Azure
3.  Select "Deployment Options"
4.  Select "GitHub" as source
5.  You may need to do the authentification stuff
6.  Select your GitHub repo and a branch
7.  Show off the continuous deployment
8.  Press "ok" 
9.  Go to https://ngrok.com/downloads  
10. Download correct version for you  
11. Open bot emulator  
12. Click 3 dots on top right  
13. "App Settings"  
14. Browse and select the downloaded "ngrok.exe"  
15. Copy URL to the Azure web app and add "/api/messages"  
16. Try It!  

## 2. Register Bot on Bot Framework
1.  Open https://dev.botframework.com  
2.  Sign in  
3.  Open "My bots"  
4.  "Create A Bot"  
5.  "Register an existing bot built using Bot Builder SDK"  
6.  Fill in the details  
7.  Messaging Endpoint = URL entered into bot emulator  
		&nbsp;&nbsp;&nbsp;&nbsp;*Ensure the URL is https:// instead of http://*  
8.  "Create Microsoft App Id and Password"  
9.  Copy all the details (App ID, Password) to use later  
		&nbsp;&nbsp;&nbsp;&nbsp;*You MUST click Finish and Go Back*  
10. "Now click register"  
11. Open up your app.js file  
12. Replace the "process.env.MICROSOFT_APP_ID" and "process.env.MICROSOFT_APP_PASSWORD" within the var connector method to your App ID and Password given by the bot framework.  
        &nbsp;&nbsp;&nbsp;&nbsp;*Ensure your ID and password are in quotation marks (represented as a string)*  
13. Commit these changes to the branch you deployed  
14. Go back to bot framework site  
15. We now have our bot registered and ready for use on multiple channels!  
16. On the top right, we have a test button - use this to check the bot is working.  
17. Leave this open, we need stuff from this later  

*NOTE: To test on the bot emulator, you must now include the App_Id and Password*
