# Tank Game

## Used Technologies

This project is based on the [MonoGame][monogame] framework.

## External resources

Using [Font][usedfont] for all the text in the game.

## Getting a build

The project is getting build with each push to the develop branch. To find a build you could download please go to the [release][releases] page and download the version for your operation system.

Right now the builds are zipped so you will need to unpack it and search for the `Tank` or `Tank.exe` file.

On Windows you should be able to click the `Tank.exe` and the game should start.

On Linux you will need to run `sudo chmod +x Tank` and start it with `./Tank` while being in the folder of the build.

### Controls

Right now there is nothing changeable inside the game and nothing is getting shown. The controls are the following

* W/S   -> Increase/Decrease strength for shooting
* A/D   -> Move Barrel to left or right
* Space -> Shoot

Keep in mind that right now there is no visual to see the barrel position. The strength can be seen on the bar to the right of the screen.

### Important

Please keep in mind that I cannot test the MacOs build and only test the Linux build from time to time so I cannot tell you if it works and how stable it will work.

The settings for the game will be saved in your `Application folder`. It will create a folder there with some files in it, feel free to delete it to reset your settings or if you do not want to play the game anymore.

## License

The project is licensed with the [MIT][license].

[usedfont]: https://github.com/phoikoi/VT323
[monogame]: https://www.monogame.net/
[license]: LICENSE
[releases]: https://github.com/D-Generation-S/Tank/releases