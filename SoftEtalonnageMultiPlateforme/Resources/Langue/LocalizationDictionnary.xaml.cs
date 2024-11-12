using System.Collections;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace SoftEtalonnageMultiPlateforme.Resources.Langue;

public partial class LocalizationDictionnary : ResourceDictionary
{
    public LocalizationDictionnary()
    {
        InitializeComponent();

        LoadResources();
    }

    private void LoadResources()
    {
        try
        {
            var culture = CultureInfo.CurrentUICulture;
            string resourceFile = "AppRessources";

            Console.WriteLine($"Loading resource file: {resourceFile} for culture: {culture}");

            var assembly = typeof(LocalizationDictionnary).Assembly;
            var resourceManager = new ResourceManager($"SoftEtalonnageMultiPlateforme.Resources.Langue.{resourceFile}", assembly);

            // Pour de la correction
            var resourceNames = assembly.GetManifestResourceNames();

            Console.WriteLine("Available resources:");
            foreach (var resourceName in resourceNames)
            {
                Console.WriteLine(resourceName);
            }
            // Fin de la correction

            var resourceSet = resourceManager.GetResourceSet(culture, true, true);

            if (resourceSet == null)
            {
                Console.WriteLine($"Resource set for {resourceFile} not found.");
                return;
            }

            foreach (DictionaryEntry entry in resourceSet)
            {
                try
                {
                    if (entry.Key == null || entry.Value == null)
                    {
                        Console.WriteLine("Null key or value found in resource set.");
                        continue;
                    }

                    this.Add(entry.Key.ToString(), entry.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception adding resource {entry.Key}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in LoadResources: {ex.Message}");
        }
    }
}