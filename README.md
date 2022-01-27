# Tank Game

[![Latest unstable build](https://github.com/D-Generation-S/Tank/actions/workflows/build-debug-version.yml/badge.svg)](https://github.com/D-Generation-S/Tank/actions/workflows/build-debug-version.yml)
[![Latest release](https://badgen.net/github/release/D-Generation-S/Tank)](https://github.com/D-Generation-S/Tank/releases)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FD-Generation-S%2FTank.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2FD-Generation-S%2FTank?ref=badge_shield)


[![GitHub license](https://img.shields.io/github/license/D-Generation-S/Tank.svg)](https://github.com/D-Generation-S/Tank/blob/main/LICENSE)
[![Github All Releases](https://img.shields.io/github/downloads/D-Generation-S/Tank/total.svg)](https://github.com/D-Generation-S/Tank/releases)


[![Windows](https://svgshare.com/i/ZhY.svg)](https://svgshare.com/i/ZhY.svg)
[![Linux](https://svgshare.com/i/Zhy.svg)](https://svgshare.com/i/Zhy.svg)
[![macOS](https://svgshare.com/i/ZjP.svg)](https://svgshare.com/i/ZjP.svg)

This game is some kind of a clone of the game `Cannonhill` based on Monogame using the OpenGL Rendering engine. The game can be played on Mac, Linux and Windows.

## Used Technologies and Software

This project is based on the [MonoGame][monogame] framework. The graphics (Spritesheets)  are created with the software [Aseprite][aseprite]. The sound effects, like the explosions are created with [Bfrx][bfxr] or [Chiptone][chiptone]. The music was created with [Bosca Ceoil][boscaceoil]. The game is getting developed on Windows with Visual Studio 2019/[2022][vs2022].
## External resources

Beside the Technologie and software listed above the following dependencies are integrated into the game.

The [VT323][usedfont] font is used for all the text in the game.

## Get a playable build

Right now there is no `stable` release. The project will be packed with each push to the [devlop][devbranch] branch.

 To find a build to download and test the game please go to the [release][releases] page and download the version for your operation system. Those builds are only really tested by me for Windows so if it is not starting on another operation system feel free to create an [issue][issue] so I can take a look. I will not be able to test the game on MacOs.

Right now the builds are zipped so you will need to unpack it and search for the `Tank` or `Tank.exe` file.

On Windows you should be able to click the `Tank.exe` and the game should start.

### Starting the game on Linux
On Linux you will need to run `sudo chmod +x Tank` and start it with `./Tank` while being in the folder of the build.

### Controls

Right now there is nothing changeable inside the game and nothing is getting shown. The controls are the following

* W/S   -> Increase/Decrease strength for shooting
* A/D   -> Move Barrel to left or right
* Space -> Shoot

Keep in mind that right now there is no visual to see the barrel position. The strength can be seen on the bar to the right of the screen.

### Important

The settings for the game will be saved in your `Application folder`. It will create a folder there with some files in it, feel free to delete it to reset your settings or if you do not want to play the game anymore.

## License

The project is licensed with the [MIT][license].


[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FD-Generation-S%2FTank.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2FD-Generation-S%2FTank?ref=badge_large)

# Contributors
![GitHub Contributors Image](https://contrib.rocks/image?repo=D-Generation-S/Tank)

[usedfont]: https://github.com/phoikoi/VT323
[monogame]: https://www.monogame.net/
[aseprite]: https://www.aseprite.org/
[bfxr]: https://www.bfxr.net/
[chiptone]: https://sfbgames.itch.io/chiptone
[boscaceoil]: https://boscaceoil.net/
[vs2022]: https://visualstudio.microsoft.com/de/vs/
[devbranch]: https://github.com/D-Generation-S/Tank/tree/develop
[issue]: https://github.com/D-Generation-S/Tank/issues
[license]: LICENSE
[releases]: https://github.com/D-Generation-S/Tank/releases