Secrets regarding the BitBracket project
```
We have 6 secret keys in the BitBracket project that are needed to run the project properly. 
The keys are stored as environment variables in the BitBracket project on the server, and as personal git secret keys locally
The syntax for setting a key locally is: dotnet user-secrets set "KeyName" "ValueInfo"
The keys are:
- "AccountSid" - -"StringOfNumbersAndLetters" The Account SID for the Twilio account which can be found on the Twilio Website as a logged in user. https://login.twilio.com/u/signup?state=hKFo2SB2enUyS2FuTFFaOEUzV0M4dXcxRWlKX18zbHU1NE9jR6Fur3VuaXZlcnNhbC1sb2dpbqN0aWTZIFR0SnFKZFFfeGl2ZUlpOENOM1F2bGRkYnRrak9zbmd5o2NpZNkgTW05M1lTTDVSclpmNzdobUlKZFI3QktZYjZPOXV1cks
- "AuthToken" - -"StringOfNumbersAndLetters" The Auth Token is also required for the Twilio account and can be found on the Twilio Website as a logged in user. https://login.twilio.com/u/signup?state=hKFo2SB2enUyS2FuTFFaOEUzV0M4dXcxRWlKX18zbHU1NE9jR6Fur3VuaXZlcnNhbC1sb2dpbqN0aWTZIFR0SnFKZFFfeGl2ZUlpOENOM1F2bGRkYnRrak9zbmd5o2NpZNkgTW05M1lTTDVSclpmNzdobUlKZFI3QktZYjZPOXV1cks
- "FromNumber - -"StringOfNumbers" The From Number is the number that the Twilio account uses to send messages. It is a number that is set by the Developer and is not found on any website. The number must be verified in order to be usable.
- "SendGridKey" - -"StringOfNumbersAndLetters" The SendGrid Key is required for the SendGrid account and used for email purposes. This can be found on the SendGrid Website as a logged in user. https://app.sendgrid.com/settings/api_keys
- "AdminKey" - -"BitBracketKey" The Admin Key is a key that is used to access the admin notifications of the BitBracket project. It is a key that is set by the developer and is not found on any website.
- "BitBracketOpenAI2" - -"StringOfNumbersAndLetters" The OpenAI Key is required for the WisperAPI feature and can be found on the OpenAI Website as a logged in user.  https://platform.openai.com/api-keys