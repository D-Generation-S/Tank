# Tank

## 

This project is a game created as an hobby project within a month. The project is depending on the [Monogame framework](http://www.monogame.net/) which can be found on [GitHub](https://github.com/MonoGame/MonoGame) as well. The project was started 2016 and was never ever polished away from a prototype state which it is currently into.

## How to play

At the moment the game can only be played with real players. Each player will get a tank which can be selected in the lobby screen. In the moment each player will get the same tank with the same stats.

You can select the playercount in the lobby to up to four players. If you press the `Start Game` button the tanks are getting placed randomly on the map you can can start. The Goal for the game is to destroy the other tank by shoting on him. The last player standing wins the game.

## Controls

### Menus

The menus are controlled with your mouse.

You can reaload a menu by pressing `f5`

### Ingame

* `a` Moves the barrel to the right

* `d` Moves the barrel to the left

* `w` Increase the shot power

* `d` Decrease the shot power

* `space` Fire

## Contirbutions

If you have found a bug or an feature idea feeld free to create an issue [here](https://github.com/D-Generation-S/Tank/issues). Before you open up a new isseu, please search to see if it was already reported. In your issue try to be as detailed as possible.

If you want to contribute firex or features to the game, please read our [contributors guide](CONTRIBUTING.md) first.

## Using the Source Code

The full source code and all the reuqired assets for the game are available here on GitHub.

## Prerequirements

To get started with the code you should check the following requirements

1. Monogame 3.7.1 installed on your system, you can download the files [here](http://www.monogame.net/downloads/)

2. An program to write and compile C# code you could use [Visual Studio Community](https://visualstudio.microsoft.com/de/vs/community/)

## Getting a working build

If you fullfill all the prerequirements you can start to create your first working build:

1. Clone this project `git clone git@github.com:D-Generation-S/Tank.git`

2. Download all the dependencies by executing in the root folder`nuget restore`  or by using your IDE.

3. Open the folder `Content` search for the `Content.mgcb` file and open it with the `Monogame Pipeline Tool` 
   
   1. In the Pipeline Tool click on Rebuild in the top menu
   
   2. Close the Pipeline Tool

4. Start to build the project in your IDE

If you got any problems creating a first working build feel free to create an [issue](https://github.com/D-Generation-S/Tank/issues) so we can help you out.

## Credits

* [CarnVanBeck](https://github.com/CarnVanBeck) (Code and Assets)

* [XanatosX](https://github.com/XanatosX) (Code)
