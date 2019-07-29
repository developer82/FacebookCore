
# FacebookCore

An unofficial .NET Core SDK for Facebook API

[![Build status](https://ci.appveyor.com/api/projects/status/k3dgy4ulri57bs56?svg=true)](https://ci.appveyor.com/project/developer82/facebookcore)

> **Disclaimer**:
    I use this library for my own personal project and I progress with it as per my project needs. If there is any feature you would like to see, please open a new issue and I will try my best to add it, or you could submit a pull request. I have tried to make this library flexible, so even if you do not find what you need it would still allow for extensions and to make calls to the Facebook API directly.
    
## Installation
This package can be installed via NuGet:

    Install-Package FacebookCore

## Usage
The main class that you would need, and that abstract the entire library use is the `FacebookClient` class. To use it, you would need to supply it with your `client_id` and `client_secret` (those could be obtained from your Facebook application in the [Facebook developer portal](https://developers.facebook.com/)).
Initialize the `FacebookClient` in the following way:

    FacebookClient client = new FacebookClient("[your_client_id]", "[your_client_secret]");
