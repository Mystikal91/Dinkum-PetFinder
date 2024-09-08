using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using UnityEngine;

namespace mystikal.dinkum.PetFinder
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	[BepInProcess("Dinkum.exe")]
	public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;

		private static mapIcon[] icons;
		private static bool setup = false;
		public static ConfigEntry<bool> _displayPetDoggo;
		public static ConfigEntry<bool> _displayBabyChook;
		public static ConfigEntry<bool> _displayChook;
		public static ConfigEntry<bool> _displayVombatJoey;
		public static ConfigEntry<bool> _displayVombat;
		public static ConfigEntry<bool> _displayPleepPuggle;
		public static ConfigEntry<bool> _displayPleep;

		private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            // Config
            Config.Bind("!Developer",      // The section under which the option is shown
                    "NexusID",  // The key of the configuration option in the configuration file
                    347, // The default value
                    "Nexus Mod ID"); // Description of the option to show in the config file
									 // Config
			_displayPetDoggo = Config.Bind("General",      // The section under which the option is shown
					"DisplayPetDoggos",  // The key of the configuration option in the configuration file
					true, // The default value
					"Display Pet Doggos on map and minimap"); // Description of the option to show in the config file
			_displayBabyChook = Config.Bind("General",      // The section under which the option is shown
					"DisplayBabyChook",  // The key of the configuration option in the configuration file
					true, // The default value
					"Display Baby Chooks on map and minimap"); // Description of the option to show in the config file
			_displayChook = Config.Bind("General",      // The section under which the option is shown
					"DisplayChook",  // The key of the configuration option in the configuration file
					true, // The default value
					"Display Chooks on map and minimap"); // Description of the option to show in the config file
			_displayVombatJoey = Config.Bind("General",      // The section under which the option is shown
					"DisplayVombatJoey",  // The key of the configuration option in the configuration file
					true, // The default value
					"Display Vombat Joeys on map and minimap"); // Description of the option to show in the config file
			_displayVombat = Config.Bind("General",      // The section under which the option is shown
					"DisplayVombat",  // The key of the configuration option in the configuration file
					true, // The default value
					"Display Vombats on map and minimap"); // Description of the option to show in the config file
			_displayPleepPuggle = Config.Bind("General",      // The section under which the option is shown
					"DisplayPleepPuggle",  // The key of the configuration option in the configuration file
					true, // The default value
					"Display Pleep Puggles on map and minimap"); // Description of the option to show in the config file
			_displayPleep = Config.Bind("General",      // The section under which the option is shown
					"DisplayPleep",  // The key of the configuration option in the configuration file
					true, // The default value
					"Display Pleeps on map and minimap"); // Description of the option to show in the config file

			// Set global plugin logger
			Plugin.Log = base.Logger;
        }

		/*	To save resources, we will run the Setup only when needed
		 *  This method prepare every icon we need for Farm Animals
		 */
		private void Setup()
		{
			// We instantiate a new array, deleting the old one if needed
			icons = new mapIcon[FarmAnimalManager.manage.activeAnimalAgents.Count];

			// For every FarmAnimal...
			for (int i = 0; i < FarmAnimalManager.manage.activeAnimalAgents.Count; i++ )
			{
				// Just to have the code tidy, we set current to the one we're looking at
				FarmAnimal current = FarmAnimalManager.manage.activeAnimalAgents[i];
				// Create new mapIcon
				icons[i] = new();
				// We instantiate the mapIcon with mapIconPrefab
				icons[i] = Object.Instantiate<mapIcon>(RenderMap.Instance.mapIconPrefab, RenderMap.Instance.mapParent).GetComponent<mapIcon>();
				// We set it as "Special"
				icons[i].CurrentIconType = mapIcon.iconType.Special;
				if (current.getDetails().animalVariation == -1)
				{
					// If the FarmAnimal isn't a color variation, we take the default icon
					// Note: defualtIcon is a typo in the original code
					icons[i].Icon.sprite = AnimalManager.manage.allAnimals[current.getDetails().animalId].GetComponent<FarmAnimal>().defualtIcon;
				}
				else
				{
					// If the FarmAnimal IS a color variation we take the icon of that variation
					icons[i].Icon.sprite = AnimalManager.manage.allAnimals[current.getDetails().animalId].GetComponent<AnimalVariation>().variationIcons[current.getDetails().animalVariation];
				}
				// We set the icon name equal to the animal name
				icons[i].IconName = current.NetworkanimalName;
				// We set the icon inactive for now
				icons[i].container.SetActive(false);
			}
			// We've finished the setup
			setup = true;
		}

		/*	We run this code every Update, to keep name and position updated
		 */
		private void Update()
		{
			if (!NetworkMapSharer.Instance.localChar)
				setup = false; // If the player isn't the Host, we just set the setup to false and do nothing
			else
			{
				// If we didn't do the setup yet or we have different number of icon and animal (for example, if one has being sold/bougth we run the setup
				if ((!setup) || (icons.Length != FarmAnimalManager.manage.activeAnimalAgents.Count))
					this.Setup();
				// Now we need to check for every animal if we should display the icon or not
				for (int i = 0; i < FarmAnimalManager.manage.activeAnimalAgents.Count; i++)
				{
					// If the animal Agent is null (this isn't a Pet) or if the current area isn't the Main Island, we need to deactivate this icon
					if ((FarmAnimalManager.manage.activeAnimalAgents[i] == null) ||
						(RealWorldTimeLight.time.GetCurrentMapArea() > WorldArea.MAIN_ISLAND))
					{
						// We set the container to false
						icons[i].container.SetActive(false);

						// If RenderMap or NetworkMapSharer have this icon, we remove it
						if (RenderMap.Instance.mapIcons.Contains(icons[i]))
							RenderMap.Instance.mapIcons.Remove(icons[i]);


						if (NetworkMapSharer.Instance.mapPoints.Contains(icons[i].MyMapPoint))
							NetworkMapSharer.Instance.mapPoints.Remove(icons[i].MyMapPoint);
						continue; // We go to the next Icon
					}

					// Just to have the code tidy, we set current to the one we're looking at
					FarmAnimal current = FarmAnimalManager.manage.activeAnimalAgents[i];

					// We check if the user wants to see this icon, based on configuration
					bool shouldIDisplay = false;
					switch (current.getDetails().animalId)
                    {
						case 9: // Chook
							shouldIDisplay = _displayChook.Value;
							break;
						case 10: // Baby Chook
							shouldIDisplay = _displayBabyChook.Value;
							break;
						case 11: // Vombat
							shouldIDisplay = _displayVombat.Value;
							break;
						case 12: // Vombat Joey
							shouldIDisplay = _displayVombatJoey.Value;
							break;
						case 17: // Pleep
							shouldIDisplay = _displayPleep.Value;
							break;
						case 19: // Pet Doggo
							shouldIDisplay = _displayPetDoggo.Value;
							break;
						case 32: // Pleep Puggle
							shouldIDisplay = _displayPleepPuggle.Value;
							break;
					}
					// Note: If the animalId isn't one of this value, we just have shouldIDisplay = false here
					if (!shouldIDisplay)
					{
						// If we shouldn't display the icon, we set the container inactive
						icons[i].container.SetActive(false);

						// If RenderMap or NetworkMapSharer have this icon, we remove it
						if (RenderMap.Instance.mapIcons.Contains(icons[i]))
							RenderMap.Instance.mapIcons.Remove(icons[i]);

						if (NetworkMapSharer.Instance.mapPoints.Contains(icons[i].MyMapPoint))
							NetworkMapSharer.Instance.mapPoints.Remove(icons[i].MyMapPoint);
						continue;
					}

					// If is inactive, we set it to active
					if (!icons[i].container.activeSelf)
						icons[i].container.SetActive(true);

					// If the name is different, we update it
					if (icons[i].IconName != current.NetworkanimalName)
						icons[i].IconName = current.NetworkanimalName;

					// Due to... reason we need to set the position to half the value of current.transform.position. 
					int tileX = (int)current.transform.position[0] / 2;
					int tileY = (int)current.transform.position[2] / 2;
					icons[i].SetPosition(tileX, tileY);

					// If the user pinged this icon, we add to the NetworkMapSharer.Instance, so it's shared with every player
					if (icons[i].ping.activeSelf)
					{
						if (!NetworkMapSharer.Instance.mapPoints.Contains(icons[i].MyMapPoint))
							NetworkMapSharer.Instance.mapPoints.Add(icons[i].MyMapPoint);
					}

					// If we didn't add this icon yet to RenderMap, we do it now
					if (!RenderMap.Instance.mapIcons.Contains(icons[i]))
						RenderMap.Instance.mapIcons.Add(icons[i]);
				}
			}
		}
	}
}