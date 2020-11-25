# Rock Paper Scissors Bot

## Brief

You need to create a “bot” that can play Rock Paper Scissors (with a slight twist) against me, the bot should be a REST service returning JSON over HTTP, implementing the interface IRPSPlayer. Use whatever best practice you would normally employ when writing code and think about a winning strategy for the bot.

The below git repo contains a C# library that contains the interface and a couple of enums all with comments which should make things clearer. 

https://github.com/lovells4/RPS

## Things I would do to improve if I had more time

Obviously, this is just a test of my coding skills, so I have hopefully given a brief idea of that. Given that this was just a test, there are a number of things that I didn't do that I normally would in a production system, such as:

* Use Autofac for DI
* Use the Mediator pattern (using Mediatr nuget package) to link the controller to the business logic
* Use FluentValidation to handle request validation and make returning errors such as BadRequest easier

I also don't think my algorithm to generate a winning strategy is the best, and could be improved by using some learning to see if there are patterns in the opponents moves and try to counter them.
