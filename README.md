# HashCity

HashCity is a 3D infinite runner developed in UnityEngine. This game has been developed as thesis for my Bachelor Degree and represent my very first approach with the game development process, Unity and the C# language (even though I was already familiar with Object-Oriented programming). 

The whole project could bee seen as composed in two parts: a library and the game itself.

The library interfaces the game with the social network Twitter, which is fundamental for the gameplay. It has been designed to simplify the communication between the social network and Unity Engine. The library covers the main action that can be performed with the Twitter APIs. It handle the logging process through the OAuth system, it allow the user to handle the tweet and perform the main actions, such as posting, replying, retweeting and searching tweet respondig to a certain query. It also can take some informations of a certain user specifying his screen name or it and it can handle Twitter timelines. Moreover, it has predisposed a system to withdrown tweet from Twitter streaming channel.

The game is a 3D infinite runner set in a nocturne city. All the animations and the models have been downloaded from the internet for free (in the project all the README and license are available in the project). The special feature of the game is its complete integration with Twitter, since game elements are the representation of Tweets from public timelines. Moreover, Twitter's users can interact with the player using a combination of hashtags and mentions, in order to help or hinder him. 
