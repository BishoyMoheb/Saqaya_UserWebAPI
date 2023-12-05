# Saqaya_UserWebAPI
It is a dotNET Core Web API solution that register and fetch a user from database that uses the minimal .NET Core Entity Framework

The “id” is programmatically generated SHA1 hash of the email address, salted with the following “450d0b0db2bcf4adde5032eca1a7c416e560cf44” string. 

The “accessToken” is programmatically generated using unique JWT Token.

The GET endpoint uses “id” and "accessToken" to return the user.

The GET endpoint omits the user “email” property if “IsMarketingConsent” is false.
