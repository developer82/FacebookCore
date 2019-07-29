
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

### Facebook Generic Collections
When querying the Facebook API for data with multiple values of the same type, it returns as an object that contains an array of the results and some metadata, along with a cursor indicating the next and previous results set (if any exists).

The `FacebookCollection<T>` abstracts the management of the results as an enumerable collection that you could use with a POCO class (using a mapper), and manages the state of the collection, allowing you to load additional items into the collection from the Facebook API and retrieve the next or previous set of items available.

The APIs already implemented in this library make use of that generic collection and abstract it as a new already typed collection for ease of use. In the case this library does not cover the collection you need, you can use this class to represent and manage collections easily.
