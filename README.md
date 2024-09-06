# Dinkum-PetFinder

Nexus Mod Link: <https://www.nexusmods.com/dinkum/mods/347>
A simple BepInEx mod for Dinkum that allows to display icons for Pet Animals on Map and Minimap. It's possible to configure which animals to display

**Brief disclaimer: In multiplayer games only the host will see the icons**

## Installation
1. Install [url=https://discord.com/channels/892654052989628436/1060375232642306088/1060375232642306088]BepInEx 6.0.0-pre.1[/url] using the tool and run the game once
2. Download and Install "PetFinder" using Vortex or by downloading "mystikal.dinkum.PetFinder.dll" from Nexus Files or GitHub Nexus and pasting it into "BepInEx\plugins\" folder
3. Run the game again, it will create the file "BepInEx\config\mystikal.dinkum.PetFinder.cfg"
4. If needed, open this file with a text editor and configure which animals to show

## Settings
Inside the config file at BepInEx\config\mystikal.dinkum.PetFinder.cfg you will find NexusID (leave this value as is) and 7 True and False values
* DisplayPetDoggos
* DisplayBabyChook
* DisplayChook
* DisplayVombatJoey
* DisplayVombat
* DisplayPleepPuggle
* DisplayPleep

By default they are all true, but you can edit some of this lines to false to hide them from the map. In most cases, you may want to enable only DisplayPetDoggos, as Pet Doggos are prone to roaming

## Credits
I've got the inspiration to do this mod from [Hide And Seek (Display NPC Icons On Map)](https://www.nexusmods.com/dinkum/mods/258) by [Snatif](https://www.nexusmods.com/dinkum/users/449197)
